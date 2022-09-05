using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using Cofetaria_Sky.Entities;
using Cofetaria_Sky.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Cofetaria_Sky.Pages
{
    [BindProperties]
    public class ForgotPasswordModel : PageModel
    {
        [Required(ErrorMessage = "Adresa de email este obligatorie")]
        [EmailAddress(ErrorMessage = "Adresă de email invalidă")]
        public string Email { get; set; }

        private readonly SkyContext _db;

        private IConfiguration _config { get; }

        public ForgotPasswordModel(SkyContext db, IConfiguration config)
        {
            _config = config;
            _db = db;
        }
        
        public  IActionResult OnPost(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.SingleOrDefault(u => u.Email == Email);

                if (user != null)
                {
                    var code = _db.Codes.SingleOrDefault(c => c.UserId == user.Id);

                    if (code == null)
                    {
                        var check = SendEmailConfirmPassword(Email);

                        if (check != null)
                        {
                            _db.Codes.Add(new CodePassword
                            {
                                UserId = user.Id,
                                Code = check
                            });

                            _db.SaveChanges();

                            return RedirectToPage("/Account/CodReset", new { email = Email });
                        }
                        else ModelState.AddModelError("", "Codul nu s-a putut trimite");
                        
                    }
                    else
                    {
                        var check = SendEmailConfirmPassword(Email);

                        if (check != null)
                        {
                            code.Code = check;

                            _db.SaveChanges();

                            return RedirectToPage("/Account/CodReset", new { email = Email });
                        }
                        else ModelState.AddModelError("", "Codul nu s-a putut trimite");
                        
                    }
                }
                else ModelState.AddModelError("", "Adresa de email incorectă"); 
            }
            return Page();
        }

        private string SendEmailConfirmPassword(string email)
        {
            var code = GenerateCode();

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(_config.GetSection("EmailConfiguration")["SmtpServer"]);

            client.Port = Convert.ToInt32(_config.GetSection("EmailConfiguration")["Port"]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential(_config.GetSection("EmailConfiguration")["Username"], _config.GetSection("EmailConfiguration")["Password"]);
                client.EnableSsl = true;
                client.Credentials = credentials;

                try
                {
                    var mail = new MailMessage(_config.GetSection("EmailConfiguration")["Username"].Trim(), email.Trim());
                    mail.Subject = "Cofetăria Sky - resetare parolă";
                    mail.Body = $"Acesta este codul tău pentru resetarea parolei: {code}. Dacă apar probleme, te rugăm să ne contactezi.";
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            return code;
        
        }

        private string GenerateCode()
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyz0123456789";

            Random number = new Random();

            char[] chars = new char[8];

            for (int i = 0; i < 8; i++)
            {
                chars[i] = allowedChars[(int)(allowedChars.Length * number.NextDouble())];
            }

            return new string(chars);
        }
    }
}
