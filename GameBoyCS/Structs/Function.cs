using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS.Structs
{
    public struct Function
    {
        public byte ticks;
        public ushort opcode;
        public Action function;

        public Function(ushort opcode, byte ticks, Action function)
        {
            this.opcode = opcode;
            this.ticks = ticks;
            this.function = function;
        }
    }
}
