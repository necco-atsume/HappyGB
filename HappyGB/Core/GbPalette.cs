using System;

namespace HappyGB.Core
{
	public class GbPalette
	{
		public GbColorMonochrome Color0 { get; private set; }
		public GbColorMonochrome Color1 { get; private set; }
		public GbColorMonochrome Color2 { get; private set; }
		public GbColorMonochrome Color3 { get; private set; }

		public byte RegisterValue
		{
			get {
				throw new NotImplementedException();
			}
			set {
				throw new NotImplementedException();
			}
		}

		public GbPalette()
		{
			Color0 = Color1 = Color2 = Color3 = GbColorMonochrome.White;
		}

	}
}

