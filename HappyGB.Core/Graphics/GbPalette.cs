using System;

using System.Drawing;

namespace HappyGB.Core.Graphics
{
    /// <summary>
    /// Provides mappings from a GBColor to its color value.
    /// </summary>
	public class GbPalette
	{
		private bool transparent;

		public GbColorMonochrome Color0 { get; private set; }
		public GbColorMonochrome Color1 { get; private set; }
		public GbColorMonochrome Color2 { get; private set; }
		public GbColorMonochrome Color3 { get; private set; }

		public Color SDColor0 
		{
			get {
				switch (Color0)
				{
				case GbColorMonochrome.Black:
					return Color.Black;
				case GbColorMonochrome.DarkGrey:
					return Color.DarkGray;
				case GbColorMonochrome.LightGrey:
					return Color.LightGray;
				case GbColorMonochrome.White:
					return Color.White;
				default:
					throw new InvalidOperationException("Invalid color value in palette.");
				}
			}
		}

		public Color SDColor1 
		{
			get {
				switch (Color1)
				{
				case GbColorMonochrome.Black:
					return Color.Black;
				case GbColorMonochrome.DarkGrey:
					return Color.DarkGray;
				case GbColorMonochrome.LightGrey:
					return Color.LightGray;
				case GbColorMonochrome.White:
					return Color.White;
				default:
					throw new InvalidOperationException("Invalid color value in palette.");
				}
			}
		}

		public Color SDColor2 
		{
			get {
				switch (Color2)
				{
				case GbColorMonochrome.Black:
					return Color.Black;
				case GbColorMonochrome.DarkGrey:
					return Color.DarkGray;
				case GbColorMonochrome.LightGrey:
					return Color.LightGray;
				case GbColorMonochrome.White:
					return Color.White;
				default:
					throw new InvalidOperationException("Invalid color value in palette.");
				}
			}
		}

		public Color SDColor3 
		{
			get {
				if (transparent) return Color.Transparent;
				switch (Color3)
				{
				case GbColorMonochrome.Black:
					return Color.Black;
				case GbColorMonochrome.DarkGrey:
					return Color.DarkGray;
				case GbColorMonochrome.LightGrey:
					return Color.LightGray;
				case GbColorMonochrome.White:
					return Color.White;
				default:
					throw new InvalidOperationException("Invalid color value in palette.");
				}
			}
		}

		public byte RegisterValue
		{
			get {
				throw new NotImplementedException();
			}
			set {
				Color0 = (GbColorMonochrome)(value & 0x03);
				Color1 = (GbColorMonochrome)((value >> 2) & 0x03);
				Color2 = (GbColorMonochrome)((value >> 4) & 0x03);
				Color3 = (GbColorMonochrome)((value >> 6) & 0x03);
			}
		}

		public GbPalette(bool transparent)
		{
			Color0 = Color1 = Color2 = Color3 = GbColorMonochrome.White;
			this.transparent = transparent;
		}

		public Color GetColor(byte color)
		{
			switch (color)
			{
			case 0x00:
				return SDColor0;
			case 0x01:
				return SDColor1;
			case 0x02:
				return SDColor2;
			case 0x03:
				return SDColor3;
			default: 
				throw new ArgumentException("Error: invalid color. GetColor expects a value from 0-3.");
			}
		}
	}
}

