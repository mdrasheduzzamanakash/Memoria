using Authentication.Configuration;
using Authentication.Models.DTO.Generic;
using Authentication.Models.DTO.Incomming;
using Authentication.Models.DTO.Outgoing;
using AutoMapper;
using Memoria.DataService.IConfiguration;
using Memoria.Entities.DTOs.Incomming;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto userRegistrationRequestDto)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(userRegistrationRequestDto.Email);

                if(userExists != null)
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

                if(!isCreated.Succeeded)
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
                    IdentityId = new Guid(newUser.Id)
                };

                await _unitOfWork.Users.Add(userSingleInDto);
                await _unitOfWork.CompleteAsync();

                // create jwt token

                var token = await GenerateJwtToken(newUser);

                return Ok(new UserRegistrationResponseDto()
                {
                    Success = true,
                    Token = token.JwtToken, 
                    RefreshToken = token.RefreshToken
                });
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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto)
        {
            if(ModelState.IsValid)
            {
                var userExist = await _userManager.FindByEmailAsync(loginRequestDto.Email);

                if(userExist == null)
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

                if(isCorrect)
                {
                    var jwtToken = await GenerateJwtToken(userExist);

                    return Ok(new UserLoginResponseDto()
                    {
                        Success = true,
                        Token = jwtToken.JwtToken,
                        RefreshToken = jwtToken.RefreshToken
                    });
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

        private async Task<TokenData> GenerateJwtToken(IdentityUser user)
        {
            var jwtHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
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
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
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
