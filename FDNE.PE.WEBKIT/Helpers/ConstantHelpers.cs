using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FDNE.PE.WEB.UIKIT.Helpers
{
    public static class ConstantHelpers
    {
        public static class MODE
        {
            public const byte PUBLIC = 0;
            public const byte DEVELOPMENT = 1;

            public static Dictionary<byte, string> VALUES = new Dictionary<byte, string>
            {
                { PUBLIC, "Público General" },
                { DEVELOPMENT, "Desarrolladores" }
            };

            public static Dictionary<byte, string> LINKS = new Dictionary<byte, string>
            {
                { PUBLIC, "" },
                { DEVELOPMENT, "dev" }
            };
        }
    }
}