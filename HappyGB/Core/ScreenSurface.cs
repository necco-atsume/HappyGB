using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HappyGB.Core
{
	public class ScreenSurface
		: ISurface
	{
		private Bitmap front, back;

		public ScreenSurface()
		{

		}

		public void Initialize()
		{
			front = new Bitmap(160, 144);
			back = new Bitmap(160, 144);
		}

		#region IGraphicsAdapter implementation

		public void FlipBuffers()
		{
			var tmp = front;
			front = back;
			back = tmp;
		}

		public Image FrontBuffer {
			get {
				return front;
			}
		}

		public Image BackBuffer {
			get {
				return back;
			}
		}

		#endregion


	}
}

