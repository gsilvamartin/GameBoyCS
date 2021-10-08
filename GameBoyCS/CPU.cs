using GameBoyCS.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameBoyCS
{
    public class CPU
    {
        #region CPU Structure
        public Clock clock;
        public Register register;
        public Memory memory;
        public List<Function> functions;

        public CPU()
        {
            memory = new Memory();

            functions = new List<Function>()
            {
                new Function(0x1, 1, NOP), new Function(0x2, 1, NOP), new Function(0x2, 1, NOP),
                new Function(0x2, 1, NOP), new Function(0x2, 1, NOP), new Function(0x2, 1, NOP),
                new Function(0x2, 1, NOP)
            };
        }

        public void EmulateCycle()
        {
            ushort opcode = 0x1;

            if (functions.Exists(x => x.opcode == opcode))
            {
                var tick = functions.Where(x => x.opcode == opcode).First().ticks;

            }
            else
            {
                Console.WriteLine("OP Code not implemented " + opcode);
            }
        }
        #endregion

        #region ASM Methods
        private int Add(ushort register, ushort value)
        {
            return register + value;
        }

        private byte Increment(ushort register, byte value)
        {
            return (byte)(register - value);
        }

        private byte Decrement(ushort register, byte value)
        {
            return (byte)(register - value);
        }

        private void JR(bool flag)
        {
            if (flag)
            {
                var curr = memory.LoadByte(register.pc);

                register.pc = (ushort)(register.pc + curr);
                register.pc += 12;
            }
            else
            {
                register.pc += 8;
            }

            register.pc += 1;
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
            register.FlagH = register.GetFlagHResult(register.HL, register.BC);
            register.FlagC = result >> 16 != 0;
            register.HL = (ushort)result;
        }

        private void LD_A_BC()
        {
            register.a = memory.LoadByte(register.BC);
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
            register.FlagC = (register.a & 0x1) != 0;
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
            register.pc += 1;
        }

        private void RLA()
        {
            bool c = register.FlagC;

            register.f = 0;
            register.FlagC = ((register.a & 0x80) != 0);
            register.a = (byte)((register.a << 1) | (c ? 1 : 0));
        }

        private void JR_S8()
        {
            JR(true);
        }

        private void ADD_HL_DE()
        {
            var res = Add(register.HL, register.DE);

            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.HL, register.DE);
            register.FlagC = register.GetFlagCResult(res);

            register.HL = (ushort)res;
        }

        private void LD_A_DE()
        {
            register.a = memory.LoadByte(register.DE);
        }

        private void DEC_DE()
        {
            var res = Decrement(register.DE, 1);

            register.DE = res;
        }

        private void INC_E()
        {
            var res = Increment(register.e, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.e, 1);

            register.e = res;
        }

        private void DEC_E()
        {
            var res = Decrement(register.e, 1);

            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = register.GetFlagHResult(register.e, 1);
        }

        private void LD_E_D8()
        {
            register.e = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void RRA()
        {
            register.f = 0;
            register.a = RotateRightByte(register.a, 8);
            register.FlagC = (register.a & 0x1) != 0;
        }

        private void JR_NZ_S8()
        {
            JR(!register.FlagZ);
        }

        private void LD_HL_D16()
        {
            register.DE = memory.LoadShort(register.pc);
            register.pc += 1;
        }

        private void LD_HL_PLUS_A()
        {
            memory.WriteByte(register.HL++, register.a);
        }

        private void INC_HL()
        {
            register.HL = Increment(register.HL, 1);
        }

        private void INC_H()
        {
            var res = Increment(register.h, 1);

            register.h = res;
            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = register.GetFlagHResult(register.h, 1);
        }

        private void DEC_H()
        {
            var res = Decrement(register.h, 1);

            register.h = res;
            register.FlagZ = register.GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = register.GetFlagHResult(register.h, 1);
        }

        
        #endregion
    }
}
