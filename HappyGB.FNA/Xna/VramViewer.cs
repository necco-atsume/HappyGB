using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core;
using HappyGB.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HappyGB.Xna
{
    /// <summary>
    /// Displays VRAM state. Draws the tiles to a surface for the emulator to draw somewhere.
    /// May be kinda slow.
    /// </summary>
    public class VramViewer
    {
        private GraphicsController gfx;
        //private SpriteFont font;

        private Color[] tiles, map1, map2;
        
        public Texture2D Tiles
        {
            get;
            private set;
        }

        public Texture2D Map1
        {
            get;
            private set;
        }

        public Texture2D Map2
        {
            get;
            private set;
        }

        public VramViewer(GraphicsController gfx)
        {
            this.gfx = gfx;
            map1 = new Color[256 * 256];
            map2 = new Color[256 * 256];
            tiles = new Color[16 * 8 * 24 * 8];
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            //font = content.Load<SpriteFont>("spritefont");
            Map1 = new Texture2D(graphics, 256, 256, false, SurfaceFormat.Color);
            Map2 = new Texture2D(graphics, 256, 256, false, SurfaceFormat.Color);
            Tiles = new Texture2D(graphics, 128, 192, false, SurfaceFormat.Color);
        }

        public void UpdateSurfaces()
        {
            //update Tiles.
            for (int i = 0; i < 384; i++)
            {
                int offset = (16 * i) + 0x8000;
                int xPosition = (i % 16) * 8;
                int yPosition = (i / 16) * 8;
                BlitTile(tiles, xPosition, yPosition, offset);
            }
            Tiles.SetData<Color>(tiles);
        }

        private void BlitTile(Color[] surface, int x, int y, int tileOffset)
        {
            ushort currentScanAddr = (ushort)tileOffset;

            for (int tileY = 0; tileY < 8; tileY++)
            {
                byte tileLow = gfx.ReadVRAM8(currentScanAddr);
                byte tileHigh = gfx.ReadVRAM8((ushort)(currentScanAddr + 1));

                int currentData = (tileHigh << 8) + tileLow;
                for (int tileX = 0; tileX < 8; tileX++)
                {
                    int calculatedColor = 0;
                    calculatedColor |= ((currentData & 0x8000) == 0x8000) ? 0x01 : 0x00;
                    calculatedColor |= ((currentData & 0x80) == 0x80) ? 0x02 : 0x00;

                    var systemDrawingColor = gfx.bgp.GetColor((byte)calculatedColor);
                    var xnaColor = new Color(systemDrawingColor.R, systemDrawingColor.G, systemDrawingColor.B, systemDrawingColor.A);

                    tiles[(x + tileX) + (128 * (tileY + y))] = xnaColor; //128: Surface width.

                    currentData = currentData << 1;
                }

                currentScanAddr += 2;
            }
        }
    }
}
