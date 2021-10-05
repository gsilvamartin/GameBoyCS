using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoyCS
{
    public class Memory
    {
        public byte[] memory;
        public ushort ROM_ADDRESS = 0x0000;
        public ushort SRAM_ADDRESS = 0xA000;
        public ushort VRAM_ADDRESS = 0x8000;
        public ushort WRAM_ADDRESS = 0xC000;
        public ushort OAM_ADDRESS = 0xFE00;

        public Memory()
        {
            memory = new byte[65536];
        }

        public byte ReadByte(ushort address)
        {
            return memory[address];
        }

        public ushort ReadShort(ushort address)
        {
            return (ushort)(memory[address] | memory[address + 1] << 8);
        }

        public void WriteByte(ushort address, byte value)
        {
            memory[address] = value;
        }

        public void WriteShort(ushort address, ushort value)
        {
            memory[address] = (byte)(value & 0x00FF);
            memory[address + 1] = (byte)((value & 0xFF00) >> 8);
        }

        public void Increment(ushort address, byte value)
        {
            memory[address] += value;
        }

        public void Decrement(ushort address, byte value)
        {
            memory[address] -= value;
        }
    }
}
