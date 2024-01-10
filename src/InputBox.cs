using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechBot
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public static InputBox Instance;
        private void InputBox_Load(object sender, EventArgs e)
        {

            this.TopMost = true;



            NameInput.TextChanged += (s, eargs) =>
            {
                if (NameInput.TextLength == 0)
                    OKButton.Enabled = false;
                else
                    OKButton.Enabled = true;
            };

            DataInput.TextChanged += (s, eargs) =>
            {
                if (DataInput.TextLength == 0)
                    OKButton.Enabled = false;
                else
                    OKButton.Enabled = true;
            };

            OKButton.Click += (s, eargs) =>
            {
                ed.Name = NameInput.Text;
                ed.Data = DataInput.Text;
                returnMessage.SetResult(ed);
            };

            CancelButton.Click += (s, eargs) =>
            {
                returnMessage.SetResult(null);
            };

            this.FormClosing += (s, eargs) =>
            {
                eargs.Cancel = true;

                if (returnMessage != null)
                    returnMessage.SetResult(null);
            };
        }

        public void StealthHide()
        {
            Opacity = 0;
            ShowInTaskbar = false;
        }

        public void Init()
        {
            this.Opacity = 1;
            this.ShowInTaskbar = true;
        }

        public void Cancel()
        {
            this.Hide();
        }

        TaskCompletionSource<EditData> returnMessage;
        EditData ed = new EditData();
        public async Task<EditData> Show(string Message, string Title, string DefaultName = "", int maxTextLength = 0, string DefaultData = "", int maxDataLength = 0) //Check if there are issues and things that need to be invoked.
        {

            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height) / 2);
            OKButton.Enabled = false;

            if (DefaultName.Length > 0 && DefaultData.Length > 0)
                OKButton.Enabled = true;

            MessageLabel.Text = Message;
            this.Text = Title;
            NameInput.MaxLength = maxTextLength;
            NameInput.Text = DefaultName;
            DataInput.MaxLength = maxDataLength;
            DataInput.Text = DefaultData;

            this.ShowInTaskbar = true;
            this.Visible = true;

            
            returnMessage = new TaskCompletionSource<EditData>();
            await returnMessage.Task;

            this.Visible = false;
            this.ShowInTaskbar = false;

            return returnMessage.Task.Result;
        }
    }
}
