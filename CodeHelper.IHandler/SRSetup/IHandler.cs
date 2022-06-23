using System;
using System.IO;
using System.Windows.Forms;
using Helper;

namespace SRSetup
{
    public partial class IHandler : Form
    {
        public IHandler()
        {
            InitializeComponent();

            KH.KeyDown += KH_KeyDown;
            KH.KeyUp += KH_KeyUp;
            MH.MouseActivity += MH_MouseActivity;
            MH.Start();
        }

        readonly MouseHandler MH = new();
        readonly KeyHandler KH = new();

        private void MH_MouseActivity(object? sender, Helper.MouseEventArgs e)
        {
            textBox1.Text = e.Button.ToString();
            if (e.Button == Helper.MouseButtons.XButton1)
                Console.WriteLine("Init");
        }

        private void InputHandler_ThreadExit(object? sender, EventArgs e)
        {
            MH.UnHook();
        }

        private void KH_KeyUp(object o, Helper.KeyEventArgs e)
        {
            if (e.KeyCode == Helper.Keys.LShiftKey) shift = false;
            if (e.KeyCode == Helper.Keys.LControlKey) ctrl = false;
            if (!ctrl && !shift && e.KeyCode == Helper.Keys.A) isHook = false;
            textBox1.Text = e.KeyValue.ToString();
            MH.UnHook();
        }

        bool ctrl, shift, isHook;
        private void KH_KeyDown(object o, Helper.KeyEventArgs e)
        {
            if (e.KeyCode == Helper.Keys.LShiftKey) shift = true;
            if (e.KeyCode == Helper.Keys.LControlKey) ctrl = true;
            if (ctrl && shift && e.KeyCode == Helper.Keys.A)
            {
                isHook = true;

                textBox1.Text = e.KeyValue.ToString();
            }
            
            e.Handled = isHook;
        }

        private void SaveButtonClick(object sender, EventArgs e)
        {
            File.WriteAllText("\\config.ini", $"{textBox1.Text}");
        }
    }
}
