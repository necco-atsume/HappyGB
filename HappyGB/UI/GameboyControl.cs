using System;
using System.Drawing;
using System.Windows.Forms;
using HappyGB.Core;

namespace HappyGB.UI
{
	public class GameboyControl
		: Control
	{
		private Gameboy gb;
		private ISurface gbSurface;

		protected override System.Drawing.Size DefaultSize {
			get {
				return new System.Drawing.Size(160, 144);
			}
		}

		public GameboyControl()
		{
			InitializeComponent();
		}

		public void InitializeComponent()
		{
			gb = new Gameboy();
			gbSurface = gb.GetSurface();
			gb.Initialize();

			this.Invalidate();
		}

		protected override void OnCreateControl()
		{
			System.Diagnostics.Debug.WriteLine("Init'd");
			gb.GetSurface().Initialize();
			base.OnCreateControl();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			gb.RunOneFrame();
			base.OnPaint(e);

			e.Graphics.DrawImageUnscaled(gbSurface.FrontBuffer, Point.Empty);

			this.Invalidate();
		}
	}
}

