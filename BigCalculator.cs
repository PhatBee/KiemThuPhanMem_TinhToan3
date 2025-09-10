using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static string SubtractNumbers(string so1Str, string so2Str)
        {
            // Tách dấu phần nguyên và phần thập phân
            ParseNumber(so1Str, out bool negA, out string intA, out string fracA);
            ParseNumber(so2Str, out bool negB, out string intB, out string fracB);

            // Cùng dương --> A - B
            if (!negA && !negB)
            {
                int cmp = CompareAbs(intA, fracA, intB, fracB);
                if (cmp == 0)
                    return "0";
                if (cmp > 0)
                    return FormatWithSign(SubtractAbs(intA, fracA, intB, fracB), false);
                else
                    return FormatWithSign(SubtractAbs(intB, fracB, intA, fracA), true); // Số âm
            }

            // Cùng âm --> -A - (-B) = B - A
            if (negA && negB) 
            {
                int cmp = CompareAbs(intB, fracB, intA, fracA);
                if (cmp == 0)
                    return "0";
                if (cmp > 0)
                    return FormatWithSign(SubtractAbs(intB, fracB, intA, fracA), false);
                else
                    return FormatWithSign(SubtractAbs(intA, fracA, intB, fracB), true); // Số âm
            }

            // A dương, B âm --> A - (-B) = A + |B|
            if (!negA && negB)
            {
                string sumAbs = AddAbs(intA, fracA, intB, fracB);
                return FormatWithSign(sumAbs, false);
            }

            // A âm, B dương --> -A - B = - ( |A| + |B| )
            string sum = AddAbs(intA, fracA, fracB, fracB);
            return FormatWithSign(sum, true);
        }

        private static string FormatWithSign(string raw, bool isNegative)
        {
            string f = FormatResult(raw);
            if (f == "0")
                return "0";
            return isNegative ? "-" + f : f; 
        }

        public static string MultiplyNumbers(string so1Str, string so2Str)
        {
            // Tách dấu phần nguyên và phần thập phân
            ParseNumber(so1Str, out bool negA, out string intA, out string fracA);
            ParseNumber(so2Str, out bool negB, out string intB, out string fracB);

            string raw = MultiplyAbs(intA,  intB, fracA, fracB);

            string formatted = FormatResult(raw);
            if (formatted == "0")
                return "0";
            bool resNeg = negA ^ negB;

            return resNeg ? "-" + formatted : formatted;
        }

        private static string MultiplyAbs(string intA, string intB, string fracA, string fracB)
        {
            // Lấy tất cả phần nguyên và phần thập phân
            string wholeA = intA + fracA;
            string wholeB = intB + fracB;

            // Xoá các số 0 thừa (giữ lại 1 số 0) 
            wholeA = wholeA.TrimStart('0');
            if (wholeA == "")
                wholeA = "0";
            wholeB = wholeB.TrimStart('0');
            if (wholeB == "")
                wholeB = "0";

            // Nếu whole đúng bằng 0
            if (wholeA == "0" || wholeB == "0")
                return "0";

            int n = wholeA.Length;
            int m = wholeB.Length;
            int[] res = new int[n + m];

            // Nhân từ phải sang trái
            for (int i = n - 1; i >= 0; i--)
            {
                int ai = wholeA[i] - '0';
                for (int j = m - 1; j >= 0; j--)
                {
                    int bj = wholeB[j] - '0';
                    int posLow = i + j + 1;
                    int mul = ai * bj + res[posLow];
                    res[posLow] = mul % 10;
                    res[posLow - 1] += mul / 10; // Nhớ đến cột phía trước
                }
            }

            // Chuyển mảng thành chuỗi
            StringBuilder sb = new StringBuilder();
            foreach (int d in res)
                sb.Append((char)('0' + d));
            string productStr = sb.ToString().TrimStart('0');
            if (productStr == "")
                productStr = "0";

            // Thêm phần thập phân
            int totalFrac = fracA.Length + fracB.Length;
            if (totalFrac == 0)
                return productStr;

            int pLen = productStr.Length;
            if (pLen > totalFrac)
            {
                string intPart = productStr.Substring(0, pLen - totalFrac);
                string fracPart = productStr.Substring(pLen - totalFrac);
                return intPart + "." + fracPart;
            }
            else // Phần nguyên là 0
            {
                string fracPart = productStr.PadLeft(totalFrac, '0');
                return "0." + fracPart;
            }
        }
                
        public static string DivideNumbers(string so1Str, string so2Str, int precision = 30, bool rounding = true)
        // precision: Số chữ số thập phân tối đa trả về
        // rounding: làm tròn
        {
            // Tách dấu phần nguyên và phần thập phân
            ParseNumber(so1Str, out bool negA, out string intA, out string fracA);
            ParseNumber(so2Str, out bool negB, out string intB, out string fracB);

            // Kiểm tra chia với 0
            if (TrimLeadingZeros(intB) == "0" && (string.IsNullOrEmpty(fracB) || TrimLeadingZeros(fracB) == "0"))
                throw new DivideByZeroException("Chia cho 0");

            // Lấy tất cả phần nguyên và phần thập phân
            string wholeA = TrimLeadingZeros(intA + (fracA ?? ""));
            string wholeB = TrimLeadingZeros(intB + (fracB ?? ""));

            // Từ fracA và fracB (độ dài phần thập phân), scale số để chia hai số nguyên
            // numerator = wholeA * 10 ^{ len(fracB)}
            // denominator = wholeB * 10 ^{ len(fracA)}
            string numerator = wholeA + new string('0', (fracB ?? "").Length);
            string denominator = wholeB + new string('0', (fracA ?? "").Length);

            numerator = TrimLeadingZeros(numerator);
            denominator = TrimLeadingZeros(denominator);

            // Chia lấy phần nguyên và phần dư
            var qr = DivideIntegerStrings(numerator, denominator);
            string integerPart = qr.quotient;
            string remainder = qr.remainder;

            // Xử lý phần dư
            List<char> fracDigits = new List<char>();
            if (remainder != "0")
            {
                // Tính toán làm tròn
                int target = precision;
                int maxCompute = precision + (rounding ? 1 : 0);
                for (int i = 0; i < maxCompute && remainder != "0"; i++)
                {
                    // Nhân 10 Reminder tiếp tục chia lấy phần thập phân
                    remainder = remainder + "0";
                    var qr2 = DivideIntegerStrings(remainder, denominator);
                    string digitStr = qr2.quotient; // lấy phần thương (0 --> 9))

                    string d = TrimLeadingZeros(digitStr);
                    if (d == "") d = "0";

                    // Đảm bảo kết quả chỉ từ (0 --> 9)
                    foreach (char c in d) fracDigits.Add(c);
                    remainder = qr2.remainder;
                }
            }

            // Xử lý làm tròn
            if (rounding && fracDigits.Count > precision)
            {
                // Lấy số cuối thập phân kiểm tra làm tròn chỉ số
                char extra = fracDigits[precision];
                // Cắt
                fracDigits.RemoveRange(precision, fracDigits.Count - precision);
                if (extra >= '5') // Lớn hơn 5 làm tròn 
                {
                    int carry = 1;
                    for (int i = fracDigits.Count - 1; i >= 0 && carry > 0; i--)
                    {
                        int d = (fracDigits[i] - '0') + carry;
                        fracDigits[i] = (char)('0' + (d % 10));
                        carry = d / 10;
                    }
                    if (carry > 0)
                    {
                        // Nếu làm tròn qua phần nguyên
                        integerPart = (integerPart == "" ? "0" : integerPart);
                        integerPart = AddOneToIntegerString(integerPart);
                    }
                }
            }
            else
            {
                // Cắt
                if (fracDigits.Count > precision) 
                    fracDigits.RemoveRange(precision, fracDigits.Count - precision);
            }

            // Kết quả
            string fracStr = fracDigits.Count > 0 ? new string(fracDigits.ToArray()) : "";
            if (string.IsNullOrEmpty(integerPart)) 
                integerPart = "0";
            string raw = string.IsNullOrEmpty(fracStr) ? integerPart : (integerPart + "." + fracStr);

            bool resultNeg = negA ^ negB;
            return FormatWithSign(raw, resultNeg);
        }

        private static string AddOneToIntegerString(string s)
        {
            // s là chuỗi số không âm, cộng thêm 1
            StringBuilder sb = new StringBuilder();
            int carry = 1;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int d = s[i] - '0';
                int sum = d + carry;
                sb.Append((char)('0' + (sum % 10)));
                carry = sum / 10;
            }
            if (carry > 0) sb.Append((char)('0' + carry));
            var res = Reverse(sb.ToString());
            return TrimLeadingZeros(res);
        }

        private static (string quotient, string remainder) DivideIntegerStrings(string numerator, string denominator)
        {
            numerator = TrimLeadingZeros(numerator);
            denominator = TrimLeadingZeros(denominator);
            if (denominator == "0") throw new DivideByZeroException();

            // Nếu tử số nhỏ hơn mẫu số
            if (CompareNumericStrings(numerator, denominator) < 0) return ("0", numerator);

            StringBuilder quotient = new StringBuilder();
            StringBuilder current = new StringBuilder();

            foreach (char ch in numerator)
            {
                // Lấy số tiếp theo
                if (!(current.Length == 1 && current[0] == '0')) // Bỏ qua số 0 đứng đầu
                    current.Append(ch);
                else
                {
                    // Nếu current = 0, chữ số mới thêm vào
                    current[0] = ch;
                }

                string curStr = TrimLeadingZeros(current.ToString());
                if (CompareNumericStrings(curStr, denominator) < 0)
                {
                    quotient.Append('0');
                    current.Clear();
                    current.Append(curStr); // Giữ curStr hiện tại và thêm tiếp theo vào phía sau 
                    continue;
                }

                // Tìm kết quả thương qdigit (9 --> 1)
                int qdigit = 0;
                for (int d = 9; d >= 1; d--)
                {
                    string prod = MultiplyStringByDigit(denominator, d);
                    if (CompareNumericStrings(prod, curStr) <= 0)
                    {
                        qdigit = d;
                        current.Clear();
                        current.Append(SubtractIntegerStrings(curStr, prod));
                        break;
                    }
                }
                quotient.Append((char)('0' + qdigit));
            }

            string q = TrimLeadingZeros(quotient.ToString());
            string r = TrimLeadingZeros(current.ToString());
            if (r == "") r = "0";
            return (q, r);
        }

        private static string SubtractIntegerStrings(string a, string b)
        {
            // a > b, cả hai đều là số nguyên
            a = TrimLeadingZeros(a);
            b = TrimLeadingZeros(b);
            int la = a.Length;
            int lb = b.Length;
            int carry = 0;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < la; i++)
            {
                int ai = a[la - 1 - i] - '0';
                int bi = (i < lb) ? b[lb - 1 - i] - '0' : 0;
                int d = ai - bi - carry;
                if (d < 0) { d += 10; carry = 1; } else carry = 0;
                sb.Append((char)('0' + d));
            }

            var res = Reverse(sb.ToString());
            return TrimLeadingZeros(res);
        }

        private static string MultiplyStringByDigit(string s, int digit)
        {
            if (digit == 0) return "0";
            if (digit == 1) return TrimLeadingZeros(s);
            int carry = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int prod = (s[i] - '0') * digit + carry;
                sb.Append((char)('0' + (prod % 10)));
                carry = prod / 10;
            }
            while (carry > 0) { sb.Append((char)('0' + (carry % 10))); carry /= 10; }
            var res = Reverse(sb.ToString());
            return TrimLeadingZeros(res);

        }

        private static int CompareNumericStrings(string a, string b)
        {
            a = TrimLeadingZeros(a); b = TrimLeadingZeros(b);
            if (a.Length != b.Length) return a.Length > b.Length ? 1 : -1;
            return string.CompareOrdinal(a, b);
        }

        private static string TrimLeadingZeros(string s)
        {
            if (string.IsNullOrEmpty(s)) return "0";
            int i = 0;
            while (i < s.Length - 1 && s[i] == '0') i++;
            string t = s.Substring(i);
            if (t == "") return "0";
            return t;
        }
    }
}
