using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDNE.PE.CORE.Helpers
{
    public static class ConstantHelpers
    {
        public static class GENERAL
        {
            public const bool SEEDS_ENABLED = true;
        }

        public static class KEYS
        {
            public const string FILE_STORAGE = "FileStorageUrl";
            public const string IMAGE_STORAGE = "ImageStorageUrl";
        }

        public static class ROUTES
        {
            public static class AREA_PREFIX
            {
                public const string ADMIN = "gestion";
                public const string CONTENT = "contenido";
            }

            public static class AREA_NAME
            {
                public const string ADMIN = "Admin";
                public const string CONTENT = "Content";
            }
        }

        public static class DISCIPLINE
        {
            public const int JUMP = 0;
            public const int TRAINING = 1;
            public const int ENDURANCE = 2;
            public const int COMPLETE = 3;

            public static Dictionary<int, string> VALUES = new Dictionary<int, string>
            {
                { JUMP, "Salto" },
                { TRAINING, "Adiestramiento" },
                { ENDURANCE, "Enduro" },
                { COMPLETE, "Concurso Completo de Equitación" }
            };
        }

        public static class ROLE
        {
            public const string ADMIN = "Admin";
            public const string RIDER = "Jinete";
            public const string JUDGE = "Jurado";
            public const string AFFILIATE = "Afiliado";
            public const string CLUB_ADMIN = "Administrador de Club";
        }

        public static class IMAGEFOLDER
        {
            public const string HORSE = "horses";
            public const string NEWS = "news";
            public const string GENERAL = "background";
            public const string GALLERY = "gallery";
            public const string USERS = "users";
            public const string CLUBS = "clubs";
        }

        public static class MESSAGE
        {
            public static class VALIDATION
            {
                public const string COMPARE = "El campo '{0}' no coincide con '{1}'";
                public const string EMAIL_ADDRESS = "El campo '{0}' no es un correo electrónico válido";
                public const string RANGE = "El campo '{0}' debe tener un valor entre {1}-{2}";
                public const string REGULAR_EXPRESSION = "El campo '{0}' no es válido";
                public const string REQUIRED = "El campo '{0}' es obligatorio";
                public const string STRING_LENGTH = "El campo '{0}' debe tener {1}-{2} caracteres";
                public const string NOT_VALID = "El campo '{0}' no es válido'";
                public const string FILE_EXTENSIONS = "El campo '{0}' solo acepta archivos con los formatos: {1}";
            }
        }

        public static class FORMATS
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
        }

        public static class EXTENSIONS
        {
            public const string IMAGES = "jpg,png,gif,bmp";
        }

        public const string TIMEZONE_ID = "SA Pacific Standard Time";
    }
}
