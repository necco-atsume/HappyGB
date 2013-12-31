using System;
using System.Windows.Forms;

using HappyGB.UI;

namespace HappyGB
{
	public class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new HappyGB.UI.MainWindow());
		}
	}
}

