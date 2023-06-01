using Authentication.Configuration;
using Authentication.Models.DTO.Generic;
using Authentication.Models.DTO.Incomming;
using Authentication.Models.DTO.Outgoing;
using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;
using MemoriaMVC.ViewModel.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MemoriaMVC.Controllers
{
    public class AccountsController : BaseController<AccountsController>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AccountsController(
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            ILogger<AccountsController> logger, 
            TokenValidationParameters tokenValidationParameters, 
            IOptionsMonitor<JwtConfig> optionMonitor, 
            UserManager<IdentityUser> userManager
            ) : base(unitOfWork, mapper, logger)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _jwtConfig = optionMonitor.CurrentValue;
        }

  

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel userRegistrationViewModel)
        {
            var userRegistrationRequestDto = _mapper.Map<UserRegistrationRequestDto>(userRegistrationViewModel);

            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(userRegistrationRequestDto.Email);

                if (userExists != null)
                {
                    return BadRequest(new UserRegistrationResponseDto
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Email is already in use"
                        }
                    });
                }

                var newUser = new IdentityUser()
                {
                    Email = userRegistrationRequestDto.Email,
                    UserName = userRegistrationRequestDto.Email,
                    EmailConfirmed = true // todo to add the functionality to send the email to the user to confirm the email
                };

                var isCreated = await _userManager.CreateAsync(newUser, userRegistrationRequestDto.Password);

                if (!isCreated.Succeeded)
                {
                    return BadRequest(new UserRegistrationResponseDto()
                    {
                        Success = false,
                        Errors = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }

                // save the user to User table
                var userSingleInDto = new UserSingleInDTO
                {
                    FirstName = userRegistrationRequestDto.FirstName,
                    LastName = userRegistrationRequestDto.LastName,
                    Email = userRegistrationRequestDto.Email,
                    Password = userRegistrationRequestDto.Password,
                    IdentityId = new Guid(newUser.Id),
                    Image = userRegistrationRequestDto.Image
                };

                await _unitOfWork.Users.Add(userSingleInDto);
                await _unitOfWork.CompleteAsync();

                // create jwt token

                var token = await GenerateJwtToken(newUser);


                var jwtTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame)
                };
                Response.Cookies.Append("jwt", token.JwtToken, jwtTokenCookieOptions);

                var refreshTokenCookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddMonths(1)
                };
                Response.Cookies.Append("refresh", token.RefreshToken, refreshTokenCookieOptions);

                // view data 
                ViewData["IsUserLoggedIn"] = true;

                return View("EmailConfirmation");
            }
            else
            {
                return BadRequest(new UserRegistrationResponseDto
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {

            var loginRequestDto = _mapper.Map<UserLoginRequestDto>(userLoginViewModel);

            if (ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(loginRequestDto.Email);

                if (userExist == null)
                {
                    return BadRequest(new UserRegistrationResponseDto()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(userExist, loginRequestDto.Password);

                if (isCorrect)
                {
                    var jwtToken = await GenerateJwtToken(userExist);

                    var jwtTokenCookieOptions = new CookieOptions
                    {
                        HttpOnly = false
                    };
                    Response.Cookies.Append("jwt", jwtToken.JwtToken, jwtTokenCookieOptions);

                    var refreshTokenCookieOptions = new CookieOptions
                    {
                        HttpOnly = true
                    };
                    Response.Cookies.Append("refresh", jwtToken.RefreshToken, refreshTokenCookieOptions);

                    TempData["IsUserLoggedIn"] = true;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest(new UserRegistrationResponseDto()
                    {
                        Success = false,
                        Errors = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new UserLoginResponseDto()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> RefreshToken(string retController, string retAction)
        {
            string jwt = Request.Cookies["jwt"];
            string refresh = Request.Cookies["refresh"];

            if(jwt == null || refresh == null)
            {
                return RedirectToAction("Login");
            }

            var tokenRequestDto = new TokenRequestDto()
            {
                Token = jwt,
                RefreshToken = refresh
            };

            if (ModelState.IsValid)
            {
                // check if the token is valid
                var result = await VerifyToken(tokenRequestDto);

                if (result == null)
                {
                    _logger.LogError("the refresh token expired");
                    return RedirectToAction("Login", "Accounts");
                }



                Response.Cookies.Append("jwt", result.Token);
                Response.Cookies.Append("refresh", result.RefreshToken);
                return RedirectToAction(retAction, retController);
            }
            else
            {
                return BadRequest(new UserLoginResponseDto()
                {
                    Success = false,
                    Errors = new List<string>()
                    {
                        "Invalid payload"
                    }
                });
            }
        }


        private async Task<AuthResult> VerifyToken(TokenRequestDto tokenRequestDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var _tokenValidationParametersForRefreshToken = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(tokenRequestDto.Token, _tokenValidationParametersForRefreshToken, out var validatedToken);

                // check if the token is actually a jwt token
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return null;
                    }

                    var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                    var expiryDate = UnixTimeStampToDate(utcExpiryDate);

                    // check the exiry date of the token 
                    if (expiryDate > DateTime.UtcNow)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Jwt token has not expired"
                            }
                        };
                    }

                    // check if the token exist 
                    var refreshTokenExist = await _unitOfWork.RefreshTokens.GetByRefreshToken(tokenRequestDto.RefreshToken);

                    if (refreshTokenExist == null)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Invalid refresh token"
                            }
                        };
                    }

                    // check the expiry date of the refresh token 
                    if (refreshTokenExist.ExpiryDate < DateTime.UtcNow)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh token has expired, please login again"
                            }
                        };
                    }

                    // check if the refresh token is used or not 
                    if (refreshTokenExist.IsUsed)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh token is already used"
                            }
                        };
                    }

                    // check if the refresh token is revoked or not 
                    if (refreshTokenExist.IsRevoked)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh token is revoked, it can't be used"
                            }
                        };
                    }

                    var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                    // match the jwt refrence of the refresh token 
                    if (refreshTokenExist.JwtId != jti)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Errors = new List<string>()
                            {
                                "Refresh token refrence does not match the jwt token"
                            }
                        };
                    }

                    // start the processing of new token 
                    refreshTokenExist.IsUsed = true;

                    var refreshTokenExistDto = _mapper.Map<RefreshTokenSingleInDTO>(refreshTokenExist);

                    var updatedResult = await _unitOfWork.RefreshTokens.MarkRefreshTokenAsUsed(refreshTokenExistDto);

                    if (updatedResult)
                    {
                        await _unitOfWork.CompleteAsync();

                        var dbUser = await _userManager.FindByIdAsync(refreshTokenExist.UserId);

                        if (dbUser == null)
                        {
                            return new AuthResult()
                            {
                                Success = false,
                                Errors = new List<string>()
                                {
                                    "Error processing request"
                                }
                            };
                        }

                        // generate a jwt token 
                        var tokens = await GenerateJwtToken(dbUser);

                        return new AuthResult()
                        {
                            Success = true,
                            Token = tokens.JwtToken,
                            RefreshToken = tokens.RefreshToken
                        };
                    }

                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>()
                            {
                                "Error processing request"
                            }
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }


        private DateTime UnixTimeStampToDate(long utcExpiryDate)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(utcExpiryDate);
            return dateTime;
        }


        private async Task<TokenData> GenerateJwtToken(IdentityUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame), // todo add the expiration times a shorter 
                SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = jwtHandler.CreateToken(tokenDescriptor);

            var jwtToken = jwtHandler.WriteToken(token);

            var refreshToken = new RefreshTokenSingleInDTO
            {
                Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
                UserId = user.Id,
                IsRevoked = false,
                IsUsed = false,
                Status = 1,
                JwtId = token.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(1)
            };


            await _unitOfWork.RefreshTokens.Add(refreshToken);
            await _unitOfWork.CompleteAsync();

            var tokenData = new TokenData
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
