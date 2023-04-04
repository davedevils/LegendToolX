using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Altra_Tool
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            try
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    if (!this.Visible)
                        this.ShowDialog();
                });
            }
            catch (InvalidCastException e) { }
        }

        public void Stop()
        {
            try
            {
                BeginInvoke((Action)delegate { this.Close(); });
            }
            catch (InvalidCastException e) { }
        }

        public void ChangeText(string newText)
        {
            BeginInvoke((Action)delegate { this.Text = newText; });
        }
    }
}
