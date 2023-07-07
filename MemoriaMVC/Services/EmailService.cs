using System.Net.Mail;
using System.Net;
using Memoria.Entities.DTOs.Incomming;
using Memoria.Entities.DTOs.Outgoing;

namespace MemoriaMVC.Services
{
    public class EmailService
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;
        private readonly bool enableSsl;

        public EmailService(string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, bool enableSsl)
        {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
            this.enableSsl = enableSsl;
        }

        public async Task<bool> SendVerificationEmail(string recipientEmail, string verificationToken, string recipientName)
        {
            string mailSubject = "Verify email for Memoria";
            string additionalText = "Thank you for joining Memoria! Please verify your email to complete the registration process.";
            string urlLink = "https://1d9f-103-142-184-112.ngrok-free.app/Accounts/ActivateEmail?email=" + recipientEmail + "&token=" + verificationToken;

            string mailBody = $@"Dear {recipientName},

                    {additionalText}

                    Click the following button or link to verify your email:
                    <a href=""{urlLink}""><button style=""background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; font-size: 16px; margin: 4px 2px; cursor: pointer;"">Activate</button></a>
                    Or pasete the link in url bar of any browser.
                    <a href=""{urlLink}"">{urlLink}</a>";

            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("memoria.bs23@gmail.com"); // Replace with your sender email address
                    message.To.Add(recipientEmail);
                    message.Subject = mailSubject;
                    message.Body = mailBody;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.EnableSsl = enableSsl;
                        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        await smtpClient.SendMailAsync(message);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> SendNewAuthorizationMail(UserSingleOutDTO authorizer, UserSingleOutDTO authorizedUser, NoteSingleOutDTO note, AuthorizationSingleInDTO authorization)
        {
            string mailSubject = "A note has been shared with you";
            string additionalText = authorizer.FirstName + " has shared a note with you as " + authorization.ModeOfAuthorization;
            

            string mailBody = $@"Dear {authorizedUser.FirstName}, <br>

                {additionalText}
                ";

            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("memoria.bs23@gmail.com"); // Replace with your sender email address
                    message.To.Add(authorizedUser.Email);
                    message.Subject = mailSubject;
                    message.Body = mailBody;
                    message.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.EnableSsl = enableSsl;
                        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                        await smtpClient.SendMailAsync(message);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }


    }
}
