using System;
using System.Windows.Forms;
using Extractor.Extractor;

namespace Extractor
{
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new Extractor.Extractor());
		}
	}
}
