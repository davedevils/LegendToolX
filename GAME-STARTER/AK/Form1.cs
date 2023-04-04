using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;

namespace StartGame
{
    public partial class Form1 : Form
    {
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();

		public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
			try {

				if (string.IsNullOrEmpty(textBox1.Text))
                {
					const string message = "Username is Empty !";
					const string caption = "ERROR";
					var result = MessageBox.Show(message, caption);
					return;
                }

				if (string.IsNullOrEmpty(textBox2.Text))
				{
					const string message = "Password is Empty !";
					const string caption = "ERROR";
					var result = MessageBox.Show(message, caption);
					return;
				}

				//AllocConsole();
				if (!File.Exists(Directory.GetCurrentDirectory() + "\\game.bin"))
				{
					const string message = "There is a problem with your client, Please Re-Install!";
					const string caption = "ERROR";
					var result = MessageBox.Show(message, caption);
					return;
				}

				Console.Write("Starting Aura Kingdom...\r\n\r\n");
				Process.Start(new ProcessStartInfo
				{
					FileName = Directory.GetCurrentDirectory() +"\\game.bin",
					Arguments = " EasyFun -a "+ textBox1.Text + " -p "+ MD5_encode(textBox2.Text),
					UseShellExecute = false
				});
				System.Windows.Forms.Application.Exit();
			}
			catch (Exception ex)
			{
				Console.Write("Game start failed! You either need to run this as admin or your game.bin does not exist!\r\n\r\n");
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			
		}

		public string MD5_encode(string str_encode)
		{
			MD5 md5Hash = MD5.Create();
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));
			StringBuilder sBuilder = new StringBuilder();

			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}
	}
}
