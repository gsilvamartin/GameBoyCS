using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public struct Register
    {
        #region Register Variables
        public ushort m, t;
        public ushort pc, sp;
        public byte a, b, c, d, e, f, h, l;

        public ushort Af
        {
            get => (ushort)((a << 8) | f);
            set
            {
                a = (byte)((value & 0xff00) >> 8); //HIGH 8 BITS SHIfTED 8
                f = (byte)(value & 0x00ff); //LOW 8 BITS
            }
        }

        public ushort BC
        {
            get => (ushort)((b << 8) | c);
            set
            {
                b = (byte)((value & 0xff00) >> 8); //HIGH 8 BITS SHIfTED 8
                c = (byte)(value & 0x00ff); //LOW 8 BITS
            }
        }

        public ushort DE
        {
            get => (ushort)((d << 8) | e);
            set
            {
                d = (byte)((value & 0xff00) >> 8); //HIGH 8 BITS SHIfTED 8
                e = (byte)(value & 0x00ff); //LOW 8 BITS
            }
        }

        public ushort HL
        {
            get => (ushort)((h << 8) | l);
            set
            {
                h = (byte)((value & 0xff00) >> 8); //HIGH 8 BITS SHIfTED 8
                l = (byte)(value & 0x00ff); //LOW 8 BITS
            }
        }
        #endregion

        #region Register Flags
        public bool FlagZ
        {
            get => (f & 0x80) != 0;
            set { f = value ? (byte)(f | 0x80) : (byte)(f & ~0x80); }
        }

        public bool FlagN
        {
            get => (f & 0x40) != 0;
            set { f = value ? (byte)(f | 0x40) : (byte)(f & ~0x40); }
        }

        public bool FlagH
        {
            get => (f & 0x20) != 0;
            set { f = value ? (byte)(f | 0x20) : (byte)(f & ~0x20); }
        }

        public bool FlagC
        {
            get => (f & 0x10) != 0;
            set { f = value ? (byte)(f | 0x10) : (byte)(f & ~0x10); }
        }
        #endregion
    }
}
