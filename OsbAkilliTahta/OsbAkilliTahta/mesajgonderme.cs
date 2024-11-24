using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsbAkilliTahta
{
    class mesajgonderme
    {
        string _ad, _sifre;
        public int sifre;
        string gidenkisi;

        OsbAkilliTahtaEntities db = new OsbAkilliTahtaEntities();

        public string Ogretmen
        {
            get { return _ad; }
            set { _ad = value; }
        }

        public string Sifre
        {
            get { return _sifre; }
            set { _sifre = value; }
        }

        public void Giris()
        {
            var adminvalue = db.TBL_OGRETMENLER.Where(x => x.AdSoyad == _ad && x.Sifre == _sifre).FirstOrDefault();
            gidenkisi=adminvalue.Gmail;
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
                smtp.Send(mail);
                sifre = randomPassword;
                MessageBox.Show("E-posta başarıyla gönderildi!");
            }
            catch (Exception ex)
            {
                randomPassword = 0;
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
