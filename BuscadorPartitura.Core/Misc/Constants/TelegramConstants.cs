using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Misc.Constants
{
    public class TelegramConstants
    {
        public const string OrquestradorDefaultUrl = "http://localhost:8080/api/";
        public static readonly string OrquestradorGetSheetUrl = $"{OrquestradorDefaultUrl}/GetSheet";
    }
}
