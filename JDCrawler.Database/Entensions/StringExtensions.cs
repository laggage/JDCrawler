using System;
using System.Linq;

namespace JDCrawler.Infrastructure.Entensions
{
    public static class StringExtensions
    {
        private const string ZeroToNine = "0123456789";
        public static int ToInt(this string text)
        {
            if (int.TryParse(text, out int v)) return v;

            string[] splitedText = text.Split(new char[] { ',', '.', '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);

            text = splitedText.Length > 0?splitedText[0]:string.Empty;
            string s = new string(text.Select(c => false == char.IsNumber(c)?' ':c).ToArray());
            s.Trim();
            return int.TryParse(s,out int value)?value:default(int);
        }
    }
}
