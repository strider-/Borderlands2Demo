using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Borderlands2Demo
{
    public static class Extensions
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        // huuurrrr I wonder what this does
        public static byte[] ReadMemory(this Process process, int address, int numBytes, out int bytesRead)
        {
            if(process != null && !process.HasExited)
            {
                IntPtr hProc = process.Handle;

                byte[] buffer = new byte[numBytes];

                ReadProcessMemory(hProc, new IntPtr(address), buffer, numBytes, out bytesRead);
                return buffer;
            }

            bytesRead = 0;
            return new byte[0];
        }
    }
}
