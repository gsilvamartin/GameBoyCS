using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public struct Register
    {
        public ushort m, t;
        public ushort pc, sp;
        public byte a, b, c, d, e, h, l;
    }
}
