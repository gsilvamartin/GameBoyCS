using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public class CPU
    {
        #region CPU Structure
        public Memory memory;
        public Clock clock;
        public Register register;
        public Dictionary<ushort, Action> functions;

        public CPU()
        {
            memory = new Memory();

            functions = new Dictionary<ushort, Action> {
                { 0x0, NOP }, { 0x1, LD_BC_D16 }, { 0x2, LD_BC_A }
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
        #endregion

        #region ASM Methods
        public int Add(ushort register, ushort value)
        {
            return register + value;
        }

        public byte Increment(ushort register, byte value)
        {
            return (byte)(register - value);
        }

        public byte Decrement(ushort register, byte value)
        {
            return (byte)(register - value);
        }
        #endregion

        #region Bitwise OP
        public byte RotateLeftByte(byte value, byte count)
        {
            return (byte)(value << count | value >> (8 - count));
        }

        public byte RotateRightByte(byte value, byte count)
        {
            return (byte)(value >> count | value << (8 - count));
        }
        #endregion

        #region CPU Instructions
        private void NOP()
        {
            register.pc += 1;
        }

        private void LD_BC_D16()
        {
            register.BC = memory.LoadShort(register.pc);
            register.pc += 2;
        }

        private void LD_BC_A()
        {
            memory.WriteByte(register.BC, register.a);
        }

        private void INC_BC()
        {
            var res = Increment(register.BC, 1);

            register.BC = res;
        }

        private void INC_B()
        {
            var res = Increment(register.b, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(res, 1);
            register.b = res;
        }

        private void DEC_B()
        {
            var res = Decrement(register.b, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = register.GetFlagHResult(res, 1);
            register.b = res;
        }

        private void LD_B_D8()
        {
            register.b = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void RLCA()
        {
            register.f = 0;
            register.FlagC = ((register.a & 0x80) != 0);
            register.a = RotateLeftByte(register.a, 8);
        }

        private void LD_A16_SP()
        {
            memory.WriteShort(memory.LoadShort(register.pc), register.sp);
            register.pc += 2;
        }

        private void ADD_HL_BC()
        {
            var result = Add(register.HL, register.BC);

            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.HL, register.h);
            register.FlagC = result >> 16 != 0;
            register.HL = (ushort)result;
        }

        private void LD_A_BC()
        {
            register.a = (byte)register.BC;
        }

        private void DEC_BC()
        {
            register.BC = Decrement(register.BC, 1);
        }

        private void INC_C()
        {
            var res = Increment(register.c, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.c, 1);

            register.c = res;
        }

        private void DEC_C()
        {
            var res = Decrement(register.c, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = register.GetFlagHResult(register.c, 1);

            register.c = res;
        }

        private void LD_C_D8()
        {
            register.c = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void RRCA()
        {
            register.f = 0;
            register.FlagC = ((register.a & 0x1) != 0);
            register.a = RotateRightByte(register.a, 8);
        }

        private void STOP()
        {
            throw new NotImplementedException();
        }

        private void LD_DE_D16()
        {
            register.DE = memory.LoadShort(register.pc);
        }

        private void LD_DE_A()
        {
            memory.WriteByte(register.DE, register.a);
        }

        private void INC_DE()
        {
            register.DE = Increment(register.DE, 1);
        }

        private void INC_D()
        {
            var res = Increment(register.d, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.d, 1);

            register.d = res;
        }

        private void DEC_D()
        {
            var res = Decrement(register.d, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = register.GetFlagHResult(register.d, 1);

            register.d = res;
        }

        private void LD_D_D8()
        {
            register.d = memory.LoadByte(register.pc);
        }
        #endregion
    }
}
