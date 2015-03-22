using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    /// <summary>
    /// The possible types of fields that we'll come across in a dbf file.
    /// All are handled the same way right now, as strings.
    /// </summary>
    internal enum DbFieldType : byte
    {
        C = 0x43, D = 0x44, F = 0x46, N = 0x4E, L = 0x4C,
        c = 0x63, d = 0x64, f = 0x66, n = 0x6E, l = 0x6C,
    };
}
