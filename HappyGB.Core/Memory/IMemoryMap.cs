using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HappyGB.Core.Memory
{
    /// <summary>
    /// Defines a generic memory mapping interface.
    /// </summary>
    public interface IMemoryMap
    {
        //FIXME: Refactor these outta here!
        //They're basically just spots in ram, so they could be /.
        byte IE { get; set; }
        byte IF { get; set; }
        
        //FIXME: Code smell...
        bool BiosEnabled { get; }

        byte this[ushort addr] { get; set; }
    }

    /// <summary>
    /// Extension methods for Read() and Write().
    /// </summary>
    public static class IMemoryMapExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Read8(this IMemoryMap map, ushort addr)
        {
            return map[addr];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write8(this IMemoryMap map, ushort addr, byte value)
        {
            map[addr] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Read16(this IMemoryMap map, ushort addr)
        {
            //FIXME: Endianness right?
            ushort ret = 0;
            ushort vr = (ushort)map[(ushort)(addr)];
            ushort vl = (ushort)(map[(ushort)(addr + 1)] << 8);
            ret = (ushort)(vl | vr);
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write16(this IMemoryMap map, ushort addr, ushort value)
        {
            //FIXME: Endianness right?
            map[addr] = (byte)(value & 0xFF);
            map[(ushort)(addr + 1)] = (byte)((value >> 8) & 0xFF);
        }
    }
}