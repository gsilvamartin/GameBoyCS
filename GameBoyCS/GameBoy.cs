using System;

namespace GameBoyCS
{
    public class GameBoy
    {
        public static void Main(string[] args)
        {
            RunEmulator();
        }

        private static void RunEmulator()
        {
            CPU cpu = new CPU();
            cpu.EmulateCycle();
        }
    }
}
