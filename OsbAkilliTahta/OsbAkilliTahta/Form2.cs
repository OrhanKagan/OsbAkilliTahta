using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Drawing.Drawing2D;

namespace OsbAkilliTahta
{
    public partial class Form2 : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelKeyboardProc _proc = HookCallback;
        private IntPtr _hookID = IntPtr.Zero;

        private mesajgonderme aa = new mesajgonderme();
        private OsbAkilliTahtaEntities db = new OsbAkilliTahtaEntities();

        public Form2()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeSozler();
            InitializeHaritalar();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_HIDE);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = ("Hz.Eyyub(as): 'Sabır ve şükür, tüm sıkıntıları hafifletir.'");
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);

                // Windows + Tab tuş kombinasyonunu engelle
                if ((Control.ModifierKeys & Keys.LWin) == Keys.LWin && vkCode == (int)Keys.Tab)
                {
                    return (IntPtr)1; // Tuşu engelle
                }

                // Alt + Tab tuş kombinasyonunu engelle
                if ((Control.ModifierKeys & Keys.LMenu) == Keys.LMenu && vkCode == (int)Keys.Tab)
                {
                    return (IntPtr)1; // Tuşu engelle
                }
            }
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);

            // Görev çubuğunu geri göster
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_SHOW);

            base.OnFormClosing(e);
        }

        private void InitializeTimer()
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        int id = 0;

        private void InitializeSozler()
        {
            Timer timer = new Timer();
            timer.Interval = 4000;
            timer.Tick += new EventHandler(Sozler_Tick);
            timer.Start();

        }

        private void InitializeHaritalar()
        {
            Timer timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += new EventHandler(Haritalar_Tick);
            timer.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm");
            label2.Text = DateTime.Now.ToShortDateString();
        }

        int sayi = 1;

        private void Haritalar_Tick(object sender, EventArgs e)
        {
            int dosyaSayisi = 1;
            string klasorYolu = "C:\\Users\\EXECOMPUTER\\source\\repos\\OsbAkilliTahta\\OsbAkilliTahta\\Haritalar";

            string[] dosyalar = Directory.GetFiles(klasorYolu);
            dosyaSayisi = dosyalar.Length;

            if (sayi > dosyaSayisi)
            {
                sayi = 1;
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"C:\Users\EXECOMPUTER\source\repos\OsbAkilliTahta\OsbAkilliTahta\Haritalar\Harita-" + sayi + ".png");
                sayi += 1;
            }
        }

        private void Sozler_Tick(object sender, EventArgs e)
        {
            if (id < db.TBL_SOZLER.Count())
            {
                id += 1;
                var deger = db.TBL_SOZLER.Find(id);
                label3.Text = deger.Soz;
            }
            else
            {
                id = 1;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }

        private void txtsifregiris_TextChanged(object sender, EventArgs e)
        {
            if (txtsifregiris.Text.Length > 6)
            {
                txtsifregiris.Text = txtsifregiris.Text.Substring(0, 6);
                txtsifregiris.SelectionStart = txtsifregiris.Text.Length;
            }
        }

        private void btnkapat_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_SHOW);

            Application.Exit();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 1;
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 2;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 3;
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 4;
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 5;
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 6;
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 7;
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 8;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 9;
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text += 0;
        }

        private void btntamam_Click(object sender, EventArgs e)
        {
            int sifre = int.Parse(txtsifregiris.Text);
            var _id = db.TBL_OGRETMENLER.Where(x => x.TekKulanımlıkŞifre == sifre).FirstOrDefault();
            if (_id != null && _id.TekKulanımlıkŞifre == sifre)
            {
                int id = _id.ID;
                var table = db.TBL_OGRETMENLER.Find(id);
                table.TekKulanımlıkŞifre = 364663411;
                db.SaveChanges();
                MessageBox.Show("Şifreniz Doğru Giriş Yapılıyor");

                int hwnd = FindWindow("Shell_TrayWnd", "");
                ShowWindow(hwnd, SW_SHOW);

                Application.Exit();
            }
            else
            {
                MessageBox.Show("Şifrenizi Yanlış Girdiniz");
            }
        }

        private void btntemizle_Click(object sender, EventArgs e)
        {
            txtsifregiris.Text ="";
        }

        private void btngerial_Click(object sender, EventArgs e)
        {
            if (txtsifregiris.Text.Length > 0)
            {
                txtsifregiris.Text = txtsifregiris.Text.Remove(txtsifregiris.Text.Length - 1, 1);
            }
        }
    }
}