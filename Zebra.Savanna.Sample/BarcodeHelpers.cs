using System;
using System.Linq;

namespace Zebra.Savanna.Sample
{
    public static class BarcodeHelpers
    {
        /// <summary>
        /// Converts an EAN8 barcode to a UPC-A.
        /// </summary>
        /// <param name="ean8">The EAN8 barcode to convert.</param>
        /// <returns>The derived UPC-A barcode.</returns>
        public static string EAN8ToUPCA(string ean8)
        {
            if (ean8.Length < 8)
            {
                ean8 = EANChecksum(ean8);
            }
            if ("012".Contains(ean8[6]))
            {
                return ean8.Substring(0, 3) + ean8[6] + "0000" + ean8.Substring(3, 3) + ean8[7];
            }
            if (ean8[6] == '3')
            {
                return ean8.Substring(0, 4) + "00000" + ean8.Substring(4, 2) + ean8[7];
            }
            if (ean8[6] == '4')
            {
                return ean8.Substring(0, 5) + "00000" + ean8[5] + ean8[7];
            }
            if ("56789".Contains(ean8[6]))
            {
                return ean8.Substring(0, 6) + "0000" + ean8.Substring(6, 2);
            }
            throw new ArgumentException("Invalid EAN8 barcode.", nameof(ean8));
        }

        /// <summary>
        /// Calculates the checksum digit for an EAN8 barcode.
        /// </summary>
        /// <param name="code">A 6- or 7-digit fragment of the EAN8 barcode without the checksum.</param>
        /// <returns>The full EAN8 barcode.</returns>
        public static string EANChecksum(string code)
        {
            if (code.Length == 6)
            {
                code = "0" + code;
            }
            int[] barcode = code.Select(d => int.Parse(d.ToString())).ToArray();
            int sum1 = barcode[1] + barcode[3] + barcode[5];
            int sum2 = 3 * (barcode[0] + barcode[2] + barcode[4] + barcode[6]);
            int checksum_value = sum1 + sum2;

            int checksum_digit = 10 - (checksum_value % 10);
            if (checksum_digit == 10)
            {
                checksum_digit = 0;
            }
            if (barcode.Length == 8 && barcode[^1] != checksum_digit)
            {
                throw new Exception($"Provided checksum digit {barcode[^1]} does not match expected checksum of {checksum_digit}.");
            }
            return code + checksum_digit;
        }
    }
}