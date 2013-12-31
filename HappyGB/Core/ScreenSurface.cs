using System;
using System.Drawing;

namespace HappyGB.Core
{
	public class ScreenSurface
		: ISurface
	{
		private Image front, back;

		public ScreenSurface()
		{
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

