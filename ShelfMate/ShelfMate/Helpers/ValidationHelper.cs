using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfMate.Helpers
{
    public static class ValidationHelper
    {
         public static bool VerifyEmpty(string text)
        {
            if(string.IsNullOrEmpty(text)) return true;

            return false;
        }
    }
}
