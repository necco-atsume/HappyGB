using System;
using System.Windows.Forms;

using HappyGB.Xna;

namespace HappyGB
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (GbGame gb = new GbGame())
			{
				gb.Run();
			}

		}
	}
}

