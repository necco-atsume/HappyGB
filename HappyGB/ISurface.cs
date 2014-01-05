using System;
using System.Drawing;

namespace HappyGB
{
	public interface ISurface
	{
		Image FrontBuffer 
		{
			get;
		}

		Image BackBuffer 
		{
			get;
		}

		void Initialize();
		void FlipBuffers();
	}
}

