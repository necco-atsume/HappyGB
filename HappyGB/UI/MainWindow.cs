using System;
using System.Windows.Forms;

namespace HappyGB.UI
{
	public class MainWindow
		: Form
	{
		private GameboyControl gbControl;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();

			gbControl = new GameboyControl();
			Controls.Add(gbControl);

			this.ResumeLayout();

			this.Name = "HappyGB";
			this.StartPosition = FormStartPosition.WindowsDefaultLocation;
			this.Text = "HappyGB -- Title";
		}
	}
}

