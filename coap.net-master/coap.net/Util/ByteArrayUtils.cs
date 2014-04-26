﻿using System;
using System.Text;

namespace CoAP.Util
{
    static class ByteArrayUtils
    {
        const String digits = "0123456789ABCDEF";

        /// <summary>
        /// Returns a hex string representation of the given bytes array.
        /// </summary>
        public static String ToHexString(Byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                StringBuilder builder = new StringBuilder(data.Length * 3);
                for (Int32 i = 0; i < data.Length; i++)
                {
                    builder.Append(digits[(data[i] >> 4) & 0xF]);
                    builder.Append(digits[data[i] & 0xF]);
                    if (i < data.Length - 1)
                        builder.Append(' ');
                }
                return builder.ToString();
            }
            else
            {
                return "--";
            }
        }

        public static String ToHexStream(Byte[] data)
        {
            if (data != null && data.Length > 0)
            {
                StringBuilder builder = new StringBuilder(data.Length * 3);
                for (Int32 i = 0; i < data.Length; i++)
                {
                    builder.Append(digits[(data[i] >> 4) & 0xF]);
                    builder.Append(digits[data[i] & 0xF]);
                }
                return builder.ToString();
            }
            else
                return String.Empty;
        }

        public static Byte[] FromHexStream(String hex)
        {
            try
            {
                hex = hex.Replace("\"", String.Empty);
                if (hex.Length % 2 == 1)
                    hex = "0" + hex;

                Byte[] tmp = new Byte[hex.Length / 2];
                for (Int32 i = 0, j = 0; i < hex.Length; i += 2)
                {
                    Int16 high = Int16.Parse(hex[i].ToString(), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                    Int16 low = Int16.Parse(hex[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
                    tmp[j++] = Convert.ToByte(high * 16 + low);
                }

                return tmp;
            }
            catch { return null; }
        }
    }
}
