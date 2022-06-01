using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using siteibs.Data;
using siteibs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace siteibs.Controllers
{
    //[Authorize]
    public class MailController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly ApplicationDbContext _context;
        public MailController(IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMail(Models.MailModel objModelMail)
        {
            objModelMail.Persiste = true;
            string ourMail =this.Configuration["serverInfo:ourMail"].ToString();
            string recepMail =this.Configuration["serverInfo:receptionMail"].ToString();
            string ourMailPassword = Configuration["serverInfo:ourMailPassword"].ToString();
            using (MailMessage mail = new MailMessage(ourMail, recepMail))
            {
                try
                {
                    mail.Subject = objModelMail.Subject;
                    mail.Body =  @"
                         <div>
        <style>
            span{
                color:#4e0e0e;
                font-weight:bold;
                font-size:1.2em;
            }
            address{
               padding:15px;
               background-color:#caedd8;
            }
        </style>
        <fieldset>
            <legend><span>Sujet: </span>" + mail.Subject + @"</legend>
            <address>
                <p><span>Nom: </span> "+ objModelMail.Nom + @"</p>
                <p><span>Email: </span> "+ objModelMail.To+ @"</p>
                <hr />
                <span>Message: </span>
                <blockquote>
                    " + objModelMail .Body+ @"
                </blockquote>
            </address>
        </fieldset>
    </div>
                        ";

                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = Configuration["serverInfo:smtpserver"].ToString();
                    smtp.EnableSsl = Convert.ToBoolean(Configuration["serverInfo:IsSSL"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(ourMail, ourMailPassword);
                    smtp.Port = Convert.ToInt16(Configuration["serverInfo:portnumber"]);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mail);

                    if (objModelMail.Persiste && false)
                    {
                        var dateNow = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "W. Central Africa Standard Time");
                        var mailObjt = new Mail()
                        {
                            Date = dateNow,
                            AdresseMail = objModelMail.To,
                            Objet = objModelMail.Subject,
                            Message = objModelMail.Body,
                            Nom = objModelMail.Nom,
                        };
                        _context.Mails.Add(mailObjt);
                        _context.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    var vvv = e.Message;
                }
            }
            return RedirectToPage("/contact","ok");
            //return Content("<script>alert('Votre message a bien été transmis à notre nos services. Nous revenons vers vous très bientôt')</script>");
        }
    }
}
