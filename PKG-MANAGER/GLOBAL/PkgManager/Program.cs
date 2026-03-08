using System;
using System.Windows.Forms;
using PkgManager.PkgManager;

namespace PkgManager
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new PkgManager.PkgManager());
		}
	}
}
