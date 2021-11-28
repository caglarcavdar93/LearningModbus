using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertData
{
    public static class ConvertData
    {
		public static int ToInt32(this ushort[] registers)
		{

			if (registers.Length != 2)
				throw new ArgumentException("Input Array length invalid - Array langth must be '2'");

			var highRegister = BitConverter.IsLittleEndian ? registers[1] : registers[0];
			var lowRegister = BitConverter.IsLittleEndian ? registers[0] : registers[1];


			byte[] highRegisterBytes = BitConverter.GetBytes(highRegister);
			byte[] lowRegisterBytes = BitConverter.GetBytes(lowRegister);

			byte[] doubleBytes = {
							highRegisterBytes[0],
							highRegisterBytes[1],
							lowRegisterBytes[0],
							lowRegisterBytes[1],
						};
			
			return BitConverter.ToInt32(doubleBytes, 0);

		}

		public static ushort[] ToUnsignedShortArray(this int value)
		{
			ushort low = (ushort)(value & 0x0000ffff);
			ushort high = (ushort)((value & 0xffff0000) >> 16);
			
			var highRegister = BitConverter.IsLittleEndian ? low : high;
			var lowRegister = BitConverter.IsLittleEndian ? high : low;

			var registers = new ushort[2] { lowRegister, highRegister };
			return registers;
		}
	}
}
