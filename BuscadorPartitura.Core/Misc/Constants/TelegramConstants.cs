using System;
using System.Collections.Generic;
using System.Text;

namespace BuscadorPartitura.Core.Misc.Constants
{
    public class TelegramConstants
    {
        public const string OrquestradorDefaultUrl = "http://localhost:7071/api";
        public static readonly string OrquestradorGetSheetUrl = $"{OrquestradorDefaultUrl}/GetSheet";
    }
}
