using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public class CPU
    {
        public Memory memory;
        public Clock clock;
        public Register register;
        public Dictionary<ushort, Action> functions;

        public CPU()
        {
            memory = new Memory();

            functions = new Dictionary<ushort, Action> {
                { 0x0, NOP }
            };
        }

        public void EmulateCycle()
        {
            ushort opcode = 0x1;

            if (functions.ContainsKey(opcode))
            {

            }
            else
            {
                Console.WriteLine("OP Code not implemented " + opcode);
            }
        }

        private void NOP()
        {
            register.pc += 1;
        }

        private void LD_BC_D16()
        {
            memory.WriteShort(register.BC, memory.ReadShort(register.pc));
        }

        private void LD_BC_A()
        {
            memory.WriteByte(register.BC, memory.ReadByte(register.a));
        }

        private void INC_BC()
        {
            memory.Increment(register.BC, 1);
        }

        private void INC_B()
        {
            memory.Increment(register.b, 1);
        }

        private void DEC_B()
        {
            memory.Decrement(register.b, 1);
        }

        private void LD_B_D8()
        {

        }
    }
}
