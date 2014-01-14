using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Core
{
    public class MBC1
        : IMemoryBankController
    {
        private int ROM_BANK_SIZE = 0x4000;
        private int RAM_BANK_SIZE = 0x2000;

        private bool hasRam, battery;

        private bool ramMode;

        private byte[] rom, ram;

        private ushort romModeRomBankOffset, ramModeRomBankOffset;
        private ushort ramBankOffset;

        private int romBankSelect, highBankSelect;

        private byte MemoryModel
        {
            set
            {
                //Set memory model.
                ramMode = (value &= 0x01) == 0x01;
                UpdateOffsets(); //FIXME: I'm tired but I don't think we need this call.
            }
        }

        private byte RomBankSelect
        {
            set
            {
                if (value == 0)
                    romBankSelect = 1;
                else
                    romBankSelect = value;

                UpdateOffsets();
            }
        }


        private byte HighBankSelect
        {
            set
            {
                value &= 0x03;
                highBankSelect = value;
                UpdateOffsets();
            }
        }

        public MBC1(CartReader cart, byte mbc, int romSizeKB, int ramSizeKB)
        {
            //Get abilities.
            switch (mbc)
            {
                case 1:
                    hasRam = false;
                    battery = false;
                    break;
                case 2:
                    hasRam = true;
                    battery = false;
                    break;
                case 3:
                    hasRam = true;
                    battery = true;
                    break;
                default:
                    throw new ArgumentException("Error: Wrong MBC type passed to MBC1 constructor.");
            }

            rom = cart.GetBuffer(romSizeKB * 1024);

            if (ramSizeKB < 0x2000) //Fill the rest with 0s, don't want to cause errors when a rom
                ramSizeKB = 0x2000; //tries to write to dead space.
            ram = new byte[ramSizeKB]; 

            Reset();
        }

        public void Reset()
        {
            ramMode = false;
            MemoryModel = 0;
            RomBankSelect = 1;
            HighBankSelect = 0;
        }

        public byte Read8(ushort address)
        {
            if (address < 0x4000)
            {
                return rom[address];
            }
            else if (address < 0x8000)
            {
                return rom[address + romModeRomBankOffset];
            }

            //RAM.
            else if (ramMode) return ram[address - 0xA000 + ramBankOffset];
            else return ram[address - 0xA000];
        }

        public void Write8(ushort address, byte value)
        {
            if (address < 0x2000) //RAM Enable
            { 
                //Do nothing.
            }
            else if (address < 0x4000) //ROM Bank Number
            {
                RomBankSelect = value;
            }
            else if (address < 0x6000) //RAM Bank Number or Upper Bits of ROM Bank Number
            {
                HighBankSelect = value;
            }
            else if (address < 0x8000) //ROM/RAM Mode Select
            {
                MemoryModel = value;
            }
            else
            {
                if (ramMode)
                {
                    ram[address - 0xA000 + ramBankOffset] = value;
                }
                else ram[address - 0xA000] = value;
            }
        }

        private void UpdateOffsets()
        {
            int romModeBank = romBankSelect | (highBankSelect << 5);

            //Subtract one because we add this to 0x4000 to get our offset in the ROM.
            romModeRomBankOffset = (ushort)((romModeBank - 1) * ROM_BANK_SIZE);
            ramModeRomBankOffset = (ushort)((romBankSelect - 1) * ROM_BANK_SIZE);

            ramBankOffset = (ushort)(highBankSelect * RAM_BANK_SIZE);
        }
    }
}
