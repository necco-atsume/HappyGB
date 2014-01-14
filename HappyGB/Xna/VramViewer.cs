﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyGB.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HappyGB.Xna
{
    public class VramViewer
    {
        private GraphicsController gfx;
        private SpriteFont font;

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
                int off = (16 * i) + 0x8000;
                int xpos = (i % 16) * 8;
                int ypos = (i / 16) * 8;
                DrawTile(tiles, xpos, ypos, off);
            }
            Tiles.SetData<Color>(tiles);
        }

        private void DrawTile(Color[] surface, int x, int y, int tileOffset)
        {
            ushort currentScanAddr = (ushort)tileOffset;

            for (int ty = 0; ty < 8; ty++)
            {
                byte tileLow = gfx.ReadVRAM8(currentScanAddr);
                byte tileHigh = gfx.ReadVRAM8((ushort)(currentScanAddr + 1));

                int currentData = (tileHigh << 8) + tileLow;
                for (int tx = 0; tx < 8; tx++)
                {
                    if (currentData != 0)
                    {
                    }

                    int color = 0;
                    color |= ((currentData & 0x8000) == 0x8000) ? 0x01 : 0x00;
                    color |= ((currentData & 0x80) == 0x80) ? 0x02 : 0x00;

                    Color pc = gfx.bgp.GetColor((byte)color);
                    tiles[(x + tx) + (128 * (ty + y))] = pc; //128: Surface width.

                    currentData = currentData << 1;
                }

                currentScanAddr += 2;
            }
        }
    }
}
