using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OsbAkilliTahta
{
    public partial class Form1 : Form
    {
        // Windows API çağrıları
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

        public Form1()
        {
            InitializeComponent();

            // Form tam ekran ayarları
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Görev çubuğunu gizle
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_HIDE);

            // Hook başlat
            _hookID = SetHook(_proc);
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

        private void girisyap_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtogretmen.Text) && !string.IsNullOrEmpty(txtsifre.Text))
            {
                aa.Ogretmen = txtogretmen.Text;
                aa.Sifre = txtsifre.Text;
                aa.Giris();
                aa.Gonder();

                groupBox1.Visible = true;
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void onayla_Click(object sender, EventArgs e)
        {
            if (int.TryParse(tekkullanımlıktxt.Text, out int userInput))
            {
                if (userInput == aa.sifre)
                {
                    MessageBox.Show($"Giriş başarılı! Tek kullanımlık şifreniz: {aa.sifre}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Görev çubuğunu geri göster
                    int hwnd = FindWindow("Shell_TrayWnd", "");
                    ShowWindow(hwnd, SW_SHOW);

                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("Hatalı tek kullanımlık şifre girdiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir sayı giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tekrargonder_Click(object sender, EventArgs e)
        {
            aa.Gonder();
            MessageBox.Show("Tek kullanımlık şifre yeniden gönderildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Görev çubuğunu geri göster
            int hwnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(hwnd, SW_SHOW);

            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
