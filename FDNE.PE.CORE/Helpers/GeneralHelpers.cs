using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.CORE.Helpers
{
    public static class GeneralHelpers
    {
        public static bool DateTimeConflictOpened(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA < endB && startB < endA;
        public static bool DateTimeConflictClosed(DateTime startA, DateTime endA, DateTime startB, DateTime endB) => startA <= endB && startB <= endA;
    }
}
