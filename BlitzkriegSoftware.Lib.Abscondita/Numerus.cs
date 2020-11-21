using System;
using System.Text;

namespace BlitzkriegSoftware.Lib.Abscondita
{
    /// <summary>
    /// Library: Hider for Number, Integer and Long
    /// </summary>
    public static class Numerus
    {
        // Tunable: Seed
        private const long Seed = 423276432;
        // Tunable: Pad Size
        private const int PadTo = 20;

        // https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string 
        private static string ReverseXor(string s)
        {
            if (s == null) return null;
            char[] charArray = s.ToCharArray();
            int len = s.Length - 1;

            for (int i = 0; i < len; i++, len--)
            {
                charArray[i] ^= charArray[len];
                charArray[len] ^= charArray[i];
                charArray[i] ^= charArray[len];
            }

            return new string(charArray);
        }

        private static Tuple<string,string> MakeLR(long value)
        {
            var hash = value.GetHashCode() ^ Seed.GetHashCode();
            var hash2 = hash.ToString();
            hash2 = hash2.PadLeft(PadTo, '0');
            var left = ReverseXor(hash2.Substring(PadTo / 2));
            var right = hash2.Substring(0, PadTo / 2);

            var lim = (left.Length / 2);
            var sb = new StringBuilder();
            for (int i = 0; i < lim; i++)
            {
                sb.Append(left.Substring(i * 2, 2));
                sb.Append(right.Substring(i * 2, 2));
            }

            hash2 = sb.ToString();

            left = hash2.Substring(0, hash2.Length / 2);
            right = hash2.Substring(hash2.Length / 2);

            return new Tuple<string, string>(left, right);
        }

        /// <summary>
        /// string to long 
        /// </summary>
        /// <param name="hidden">hide-string</param>
        /// <returns>value</returns>
        public static long Unhide(string hidden)
        {
            long value = 0;

            if(!string.IsNullOrWhiteSpace(hidden)) {
                var left  = hidden.Substring(0, PadTo / 2);
                var right = hidden.Substring(hidden.Length - (PadTo / 2));
                var text = hidden.Replace(right,"").Replace(left,"");
                if(!long.TryParse(text, out value))
                {
                    value = 0;
                }

                var lr = MakeLR(value);
                if ((left != lr.Item1) || (right != lr.Item2)) throw new InvalidOperationException("Hash does not match");
            }
            return value;
        }

        /// <summary>
        /// long to hide-string
        /// </summary>
        /// <param name="value">long</param>
        /// <returns>hide-string</returns>
        public static string Hide(long value)
        {
            var lr = MakeLR(value);
            return lr.Item1 + value.ToString() + lr.Item2; 
        }
    }
}
