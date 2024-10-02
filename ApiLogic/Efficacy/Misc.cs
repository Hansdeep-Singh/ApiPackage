using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Efficacy
{
   public static class Misc
    {
        public static DateTime ConvertFromUnixTimeStamp(int? timestamp)
        {
            DateTime origin = new(1970,1,1,0,0,0,0);
            return origin.AddSeconds((double)timestamp);
        }
    }
}
