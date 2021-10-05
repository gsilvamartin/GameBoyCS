using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public struct Register
    {
        public ushort m, t;
        public ushort pc, sp;
        public byte a, b, c, d, e, f, h, l;

        public ushort AF
        {
            get => (ushort)((a << 8) | f);
            set
            {
                a = (byte)((value & 0xFF00) >> 8); //HIGH 8 BITS SHIFTED 8
                f = (byte)(value & 0x00FF); //LOW 8 BITS
            }
        }

        public ushort BC
        {
            get => (ushort)((b << 8) | c);
            set
            {
                b = (byte)((value & 0xFF00) >> 8); //HIGH 8 BITS SHIFTED 8
                c = (byte)(value & 0x00FF); //LOW 8 BITS
            }
        }

        public ushort DE
        {
            get => (ushort)((d << 8) | e);
            set
            {
                d = (byte)((value & 0xFF00) >> 8); //HIGH 8 BITS SHIFTED 8
                e = (byte)(value & 0x00FF); //LOW 8 BITS
            }
        }

        public ushort HL
        {
            get => (ushort)((h << 8) | l);
            set
            {
                h = (byte)((value & 0xFF00) >> 8); //HIGH 8 BITS SHIFTED 8
                l = (byte)(value & 0x00FF); //LOW 8 BITS
            }
        }
    }
}
