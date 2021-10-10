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

        public bool GetFlagZResult(int b)
        {
            return b == 0;
        }

        public bool GetFlagCResult(int i)
        {
            return (i >> 8) != 0;
        }

        public bool GetFlagHResult(byte v1, byte v2)
        {
            return ((v1 & 0xF) + (v2 & 0xF)) > 0xF;
        }

        public bool GetFlagHResult(ushort v1, ushort v2)
        {
            return ((v1 & 0xFFF) + (v2 & 0xFFF)) > 0xFFF;
        }

        public bool GetFlagCarry(byte v1, byte v2)
        {
            return ((v1 & 0xF) + (v2 & 0xF)) >= 0xF;
        }

        public bool GetFlagHSub(byte v1, byte v2)
        {
            return (v1 & 0xF) < (v2 & 0xF);
        }

        public bool GetFlagHSubCarry(byte v1, byte b2, bool flagC)
        {
            int carry = flagC ? 1 : 0;
            return (v1 & 0xF) < ((b2 & 0xF) + carry);
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

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(res, 1);
            register.b = res;
        }

        private void DEC_B()
        {
            var res = Decrement(register.b, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(res, 1);
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
            register.FlagH = GetFlagHResult(register.HL, register.BC);
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

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.c, 1);

            register.c = res;
        }

        private void DEC_C()
        {
            var res = Decrement(register.c, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.c, 1);

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

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.d, 1);

            register.d = res;
        }

        private void DEC_D()
        {
            var res = Decrement(register.d, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.d, 1);

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
            register.FlagH = GetFlagHResult(register.HL, register.DE);
            register.FlagC = GetFlagCResult(res);

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

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.e, 1);

            register.e = res;
        }

        private void DEC_E()
        {
            var res = Decrement(register.e, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.e, 1);
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
            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.h, 1);
        }

        private void DEC_H()
        {
            var res = Decrement(register.h, 1);

            register.h = res;
            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.h, 1);
        }

        private void LD_H_D8()
        {
            register.h = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void DAA()
        {
            //TODO
        }

        private void JR_Z_S8()
        {
            JR(register.FlagZ);
        }

        private void ADD_HL_HL()
        {
            var res = (ushort)Add(register.HL, register.HL);

            register.HL = res;

            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.HL, register.HL);
            register.FlagC = GetFlagCResult(res);
        }

        private void LD_A_HL_PLUS()
        {
            register.a = memory.LoadByte(register.HL++);
        }

        private void DEC_HL()
        {
            register.HL = Decrement(register.HL, 1);
        }

        private void INC_L()
        {
            var res = Increment(register.l, 1);

            register.l = res;
            register.FlagN = false;
            register.FlagZ = GetFlagZResult(res);
            register.FlagH = GetFlagHResult(register.l, 1);
        }

        private void DEC_L()
        {
            var res = Decrement(register.l, 1);

            register.l = res;
            register.FlagN = true;
            register.FlagZ = GetFlagZResult(res);
            register.FlagH = GetFlagHResult(register.l, 1);
        }

        private void LD_L_D8()
        {
            register.l = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void CPL()
        {
            //TODO
        }

        private void JR_NC_S8()
        {
            JR(!register.FlagC);
        }

        private void LD_SP_D16()
        {
            register.pc = memory.LoadShort(register.pc);
            register.pc += 2;
        }

        private void LD_HL_MINUS_A()
        {
            memory.WriteByte(register.HL--, register.a);
        }

        private void INC_SP()
        {
            register.sp = Increment(register.sp, 1);
        }

        private void INC_M_HL()
        {
            var res = Increment(memory.LoadByte(register.HL), 1);
            memory.WriteByte(register.HL, res);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.HL, 1);
        }

        private void DEC_M_HL()
        {
            var res = Decrement(memory.LoadByte(register.HL), 1);
            memory.WriteByte(register.HL, res);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.HL, 1);
        }

        private void LD_HL_D8()
        {
            memory.WriteByte(register.HL, memory.LoadByte(register.pc));
            register.pc += 1;
        }

        private void SCF()
        {
            //TODO
        }

        private void JR_C_S8()
        {
            JR(register.FlagC);
        }

        private void ADD_HL_SP()
        {
            var res = Add(register.HL, register.pc);

            register.HL = (ushort)res;
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.HL, register.sp);
            register.FlagC = GetFlagCResult(res);
        }

        private void LD_A_HL()
        {
            register.a = memory.LoadByte(register.HL--);
        }

        private void DEC_SP()
        {
            register.pc -= 1;
        }

        private void INC_A()
        {
            var res = Increment(register.a, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = false;
            register.FlagH = GetFlagHResult(register.a, 1);
        }

        private void DEC_A()
        {
            var res = Decrement(register.a, 1);

            register.FlagZ = GetFlagZResult(res);
            register.FlagN = true;
            register.FlagH = GetFlagHResult(register.a, 1);
        }

        private void LD_A_D8()
        {
            register.a = memory.LoadByte(register.pc);
            register.pc += 1;
        }

        private void CCF()
        {
            //TODO
        }

        private void LD_B_B()
        {
            //register.b = register.b;
        }

        private void LD_B_C()
        {
            register.b = register.c;
        }

        private void LD_B_D()
        {
            register.b = register.d;
        }

        private void LD_B_E()
        {
            register.b = register.e;
        }

        private void LD_B_H()
        {
            register.b = register.h;
        }

        private void LD_B_L()
        {
            register.b = register.l;
        }

        private void LD_B_HL()
        {
            register.b = memory.LoadByte(register.HL);
        }

        private void LD_B_A()
        {
            register.b = register.a;
        }

        private void LD_C_B()
        {
            register.c = register.b;
        }

        private void LD_C_C()
        {
            //register.c = register.c;
        }

        private void LD_C_D()
        {
            register.c = register.d;
        }

        private void LD_C_E()
        {
            register.c = register.e;
        }

        private void LD_C_H()
        {
            register.c = register.h;
        }

        private void LD_C_L()
        {
            register.c = register.l;
        }

        private void LD_C_HL()
        {
            register.c = memory.LoadByte(register.HL);
        }

        private void LD_C_A()
        {
            register.c = register.a;
        }

        private void LD_D_B()
        {
            register.d = register.b;
        }

        private void LD_D_C()
        {
            register.d = register.c;
        }

        private void LD_D_D()
        {
            //register.d = register.d;
        }

        private void LD_D_E()
        {
            register.d = register.e;
        }

        private void LD_D_H()
        {
            register.d = register.h;
        }

        private void LD_D_L()
        {
            register.d = register.l;
        }

        private void LD_D_HL()
        {
            register.d = memory.LoadByte(register.HL);
        }

        private void LD_D_A()
        {
            register.d = register.a;
        }

        private void LD_E_B()
        {
            register.e = register.b;
        }

        private void LD_E_C()
        {
            register.e = register.c;
        }

        private void LD_E_D()
        {
            register.e = register.d;
        }

        private void LD_E_E()
        {
            //register.e = register.e;
        }

        private void LD_E_H()
        {
            register.e = register.h;
        }

        private void LD_E_L()
        {
            register.e = register.l;
        }

        private void LD_E_HL()
        {
            register.e = memory.LoadByte(register.HL);
        }

        private void LD_E_A()
        {
            register.e = register.a;
        }

        private void LD_H_B()
        {
            register.h = register.b;
        }

        private void LD_H_C()
        {
            register.h = register.c;
        }

        private void LD_H_D()
        {
            register.h = register.d;
        }

        private void LD_H_E()
        {
            register.h = register.e;
        }

        private void LD_H_H()
        {
            //register.h = register.h;
        }

        private void LD_H_L()
        {
            register.h = register.l;
        }

        private void LD_H_HL()
        {
            register.h = memory.LoadByte(register.HL);
        }

        private void LD_H_A()
        {
            register.h = register.a;
        }

        private void LD_L_B()
        {
            register.l = register.b;
        }

        private void LD_L_C()
        {
            register.l = register.c;
        }

        private void LD_L_D()
        {
            register.l = register.d;
        }

        private void LD_L_E()
        {
            register.l = register.e;
        }

        private void LD_L_H()
        {
            register.l = register.h;
        }

        private void LD_L_L()
        {
            //register.l = register.l;
        }

        private void LD_L_HL()
        {
            register.l = memory.LoadByte(register.HL);
        }

        private void LD_L_A()
        {
            register.l = register.a;
        }

        private void LD_HL_B()
        {
            memory.WriteByte(register.HL, register.b);
        }

        private void LD_HL_C()
        {
            memory.WriteByte(register.HL, register.c);
        }

        private void LD_HL_D()
        {
            memory.WriteByte(register.HL, register.d);
        }

        private void LD_HL_E()
        {
            memory.WriteByte(register.HL, register.e);
        }

        private void LD_HL_H()
        {
            memory.WriteByte(register.HL, register.h);
        }

        private void LD_HL_L()
        {
            memory.WriteByte(register.HL, register.l);
        }

        private void HALT()
        {
            //TODO
        }

        private void LD_HL_A()
        {
            memory.WriteByte(register.HL, register.a);
        }

        private void LD_A_B()
        {
            memory.WriteByte(register.HL, register.l);

        }

        private void LD_A_C()
        {
            register.a = register.c;
        }

        private void LD_A_D()
        {
            register.a = register.d;
        }

        private void LD_A_E()
        {
            register.a = register.e;
        }

        private void LD_A_H()
        {
            register.a = register.h;
        }

        private void LD_A_L()
        {
            register.a = register.l;
        }

        private void LD_A_M_HL()
        {
            register.a = memory.LoadByte(register.HL);
        }

        private void LD_A_A()
        {
            //register.a = register.a;
        }

        private void ADD_A_B() { }

        private void ADD_A_C() { }

        private void ADD_A_D() { }

        private void ADD_A_E() { }

        private void ADD_A_H() { }

        private void ADD_A_L() { }

        private void ADD_A_HL() { }

        private void ADD_A_A() { }

        private void ADC_A_B() { }

        private void ADC_A_C() { }

        private void ADC_A_D() { }

        private void ADC_A_E() { }

        private void ADC_A_H() { }

        private void ADC_A_L() { }

        private void ADC_A_HL() { }

        private void ADC_A_A() { }

        private void SUB_B() { }

        private void SUB_C() { }

        private void SUB_D() { }

        private void SUB_E() { }

        private void SUB_H() { }

        private void SUB_L() { }

        private void SUB_HL() { }

        private void SUB_A() { }

        private void SBC_A_B() { }

        private void SBC_A_C() { }

        private void SBC_A_D() { }

        private void SBC_A_E() { }

        private void SBC_A_H() { }

        private void SBC_A_L() { }

        private void SBC_A_HL() { }

        private void SBC_A_A() { }

        private void AND_B() { }

        private void AND_C() { }

        private void AND_D() { }

        private void AND_E() { }

        private void AND_H() { }

        private void AND_L() { }

        private void AND_HL() { }

        private void AND_A() { }

        private void XOR_B() { }

        private void XOR_C() { }

        private void XOR_D() { }

        private void XOR_E() { }

        private void XOR_H() { }

        private void XOR_L() { }

        private void XOR_HL() { }

        private void XOR_A() { }

        private void OR_B() { }

        private void OR_C() { }

        private void OR_D() { }

        private void OR_E() { }

        private void OR_H() { }

        private void OR_L() { }

        private void OR_HL() { }

        private void OR_A() { }

        private void CP_B() { }

        private void CP_C() { }

        private void CP_D() { }

        private void CP_E() { }

        private void CP_H() { }

        private void CP_L() { }

        private void CP_HL() { }

        private void CP_A() { }


        #endregion
    }
}
