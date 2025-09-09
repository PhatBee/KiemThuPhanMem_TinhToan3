using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buoi07_TinhToan3
{
    public static class BigCalculator
    {
        public static string AddNumbers(string so1Str, string so2Str)
        {
            // Tách dấu phần nguyên và thập phân
            ParseNumber(so1Str, out bool negA, out string intA, out string fracA);
            ParseNumber(so2Str, out bool negB, out string intB, out string fracB);

            // Cùng dấu (cộng abs)
            if (negA == negB)
            {
                string sumAbs = AddAbs(intA, fracA, intB, fracB);
                string formatted = FormatResult(sumAbs);
                if (formatted == "0") return "0"; // Kết quả là 0
                return negA ? "-" + formatted : formatted;
            }
            else // Khác dấu (lớn - nhỏ)
            {
                int cmp = CompareAbs(intA, fracA, intB, fracB);
                if (cmp == 0) return "0"; // Bằng nhau
                if (cmp > 0) // |A| > |B|
                {
                    string diff = SubtractAbs(intA, fracA, intB, fracB); // A - B
                    string formatted = FormatResult(diff);
                    return negA ? "-" + formatted : formatted;
                }
                else // |A| < |B|
                {
                    string diff = SubtractAbs(intB, fracB, intA, fracA); // B - A
                    string formatted = FormatResult(diff);
                    return negB ? "-" + formatted : formatted;
                }
            }
        }

        private static string FormatResult(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return "0";
            string intPart, fracPart;
            if (raw.Contains('.'))
            {
                var p = raw.Split('.');
                intPart = p[0];
                fracPart = p.Length > 1 ? p[1] : "";
            }
            else
            {
                intPart = raw;
                fracPart = "";
            }

            // Xóa số 0 thừa ở phần nguyên
            int idx = 0;
            while (idx < intPart.Length - 1 && intPart[idx] == '0')
                idx++;
            intPart = intPart.Substring(idx);

            // Xóa số 0 thừa ở phần thập phân
            if (!string.IsNullOrEmpty(fracPart))
            {
                int fidx = fracPart.Length - 1;
                while (fidx >= 0 && fracPart[fidx] == '0')
                    fidx--;
                if (fidx >= 0)
                    fracPart = fracPart.Substring(0, fidx + 1);
                else
                    fracPart = "";
            }

            if (string.IsNullOrEmpty(fracPart))
                return intPart;
            else
                return intPart + "." + fracPart;
        }

        private static string SubtractAbs(string intA, string fracA, string intB, string fracB)
        {
            // Phần thập phân
            int fracLen = Math.Max(fracA.Length, fracB.Length);
            fracA = fracA.PadRight(fracLen, '0');
            fracB = fracB.PadRight(fracLen, '0');

            // Phần nguyên
            int intLen = Math.Max(intA.Length, intB.Length);
            intA = intA.PadLeft(intLen, '0');
            intB = intB.PadLeft(intLen, '0');

            // Chuẩn bị kết quả
            StringBuilder sbFrac = new StringBuilder();
            int borrow = 0;

            // Trừ phần thập phân từ phải sang trái
            for (int i = fracLen - 1; i >= 0; i--)
            {
                int da = fracA[i] - '0';
                int db = fracB[i] - '0';
                int sub = da - db - borrow;
                if (sub < 0)
                {
                    sub += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                sbFrac.Append((char)('0' + sub));
            }

            // Trừ phần nguyên từ phải sang trái
            StringBuilder sbInt = new StringBuilder();
            for (int i = intLen - 1; i >= 0; i--)
            {
                int da = intA[i] - '0';
                int db = intB[i] - '0';
                int sub = da - db - borrow;
                if (sub < 0)
                {
                    sub += 10;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                sbInt.Append((char)('0' + sub));
            }

            // Kết hợp phần nguyên và thập phân
            string intRes = Reverse(sbInt.ToString());
            string fracRes = fracLen > 0 ? Reverse(sbFrac.ToString()) : "";

            if (fracLen > 0)
                return intRes + "." + fracRes;
            else
                return intRes;

        }

        private static int CompareAbs(string intA, string fracA, string intB, string fracB)
        {
            // Xóa các số 0 thừa
            string aInt = intA.TrimStart('0');
            string bInt = intB.TrimStart('0');

            // So sánh phần nguyên
            if (aInt.Length != bInt.Length)
                return aInt.Length > bInt.Length ? 1 : -1;
            int cmp = string.CompareOrdinal(aInt, bInt);
            if (cmp != 0)
                return cmp > 0 ? 1 : -1;

            // Nếu phần nguyên bằng nhau, so sánh phần thập phân
            int fracLen = Math.Max(fracA.Length, fracB.Length);
            string fa = fracA.PadRight(fracLen, '0');
            string fb = fracB.PadRight(fracLen, '0');
            int cmpFrac = string.CompareOrdinal(fa, fb);
            if (cmpFrac != 0)
                return cmpFrac > 0 ? 1 : -1;

            return 0;
        }

        private static string AddAbs(string intA, string fracA, string intB, string fracB)
        {
            // Cộng phần thập phân
            int fracLen = Math.Max(fracA.Length, fracB.Length);
            fracA = fracA.PadRight(fracLen, '0');
            fracB = fracB.PadRight(fracLen, '0');

            // Cộng phần nguyên
            int intLen = Math.Max(intA.Length, intB.Length);
            intA = intA.PadLeft(intLen, '0');
            intB = intB.PadLeft(intLen, '0');

            StringBuilder sbFrac = new StringBuilder();
            int carry = 0;

            // Cộng phần thập phân từ phải sang trái
            for (int i = fracLen - 1; i >= 0; i--)
            {
                int da = fracA.Length > 0 ? (fracA[i] - '0') : 0;
                int db = fracB.Length > 0 ? (fracB[i] - '0') : 0;
                int sum = da + db + carry;
                sbFrac.Append((char)('0' + (sum % 10)));
                carry = sum / 10;
            }

            // Cộng phần nguyên từ phải sang trái
            StringBuilder sbInt = new StringBuilder();
            for (int i = intLen - 1; i >= 0; i--)
            {
                int da = intA[i] - '0';
                int db = intB[i] - '0';
                int sum = da + db + carry;
                sbInt.Append((char)('0' + (sum % 10)));
                carry = sum / 10;
            }

            // Nếu còn dư thì thêm vào phần nguyên
            if (carry > 0)
                sbInt.Append((char)('0' + carry));

            // Kết hợp phần nguyên và thập phân
            string intRes = Reverse(sbInt.ToString());
            string fracRes = fracLen > 0 ? Reverse(sbFrac.ToString()) : "";

            if (fracLen > 0)
                return intRes + "." + fracRes;
            else
                return intRes;
        }

        private static string Reverse(string v)
        {
            var arr = v.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private static void ParseNumber(string norm, out bool isNegative, out string intPart, out string fracPart)
        {
            isNegative = false;
            intPart = "0";
            fracPart = "";

            if (string.IsNullOrEmpty(norm)) return; // Trả về mặc định nếu chuỗi rỗng (Kiểm cho chắc)

            string s = norm;
            if (s[0] == '+' || s[0] == '-')
            {
                isNegative = s[0] == '-';
                s = s.Substring(1);
            }
            if (s.Contains("."))
            {
                var p = s.Split('.');
                intPart = string.IsNullOrEmpty(p[0]) ? "0" : p[0];
                fracPart = p.Length > 1 ? p[1] : "";
            }
            else
            {
                intPart = s;
                fracPart = "";
            }
        }
    }
}
