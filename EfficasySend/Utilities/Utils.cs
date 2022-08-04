using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace EfficacySend.Utilities
{
   public class Utils
    {
        public static async Task<bool> CheckHtml(string Html)
        {
            var doc = new HtmlDocument();
            await Task.Run(() => { doc.LoadHtml(Html); });
            return !doc.ParseErrors.Any();
        }

        public static bool CheckHtmlNotAsync(string Html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(Html);
            return !doc.ParseErrors.Any();
        }

    }
}
