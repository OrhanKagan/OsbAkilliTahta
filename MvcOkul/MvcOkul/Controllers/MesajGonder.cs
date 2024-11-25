using MvcOkul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MvcOkul.Controllers
{
    public class MesajGonder
    {
        string _mail;
        int _id;
        public int sifre;
        public int veri = 0;
        string gidenkisi;

        OsbAkilliTahtaEntities db = new OsbAkilliTahtaEntities();

        public string Mail
        {
            get { return _mail; }
            set { _mail = value; }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public void Giris()
        {
            var adminvalue = db.TBL_OGRETMENLER.Where(x => x.Gmail == _mail).FirstOrDefault();
            gidenkisi = adminvalue.Gmail;
        }

        public void Gonder()
        {
            int randomPassword = 0;
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("3411okk@gmail.com", "txfkviyjekgnmcoc"),

                    EnableSsl = true
                };

                Random random = new Random();
                randomPassword = random.Next(100000, 1000000);

                // E-posta içeriği
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("3411okk@gmail.com"),
                    Subject = "Test Mesajı",
                    Body = "Tek kullanımlık şifreniz " + randomPassword,
                    IsBodyHtml = true // HTML destekli mesaj göndermek için
                };

                // Alıcı adresi
                mail.To.Add(gidenkisi); // Alıcı adresini buraya yaz

                // E-posta gönderme
                sifre = randomPassword;
                smtp.Send(mail);
            }
            catch (Exception)
            {
                randomPassword = 0;
            }
        }
    }
}