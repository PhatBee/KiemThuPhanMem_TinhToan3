using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Buoi07_TinhToan3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            txtSo1.Validating += TxtSo_Validating;
            txtSo2.Validating += TxtSo_Validating;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtSo1.Text = txtSo2.Text = "0";
            radCong.Checked = true;             //đầu tiên chọn phép cộng
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Bạn có thực sự muốn thoát không?",
                                 "Thông báo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                this.Close();
        }

        private void btnTinh_Click(object sender, EventArgs e)
        {
            //lấy giá trị của 2 ô số
            double so1, so2, kq = 0;
            string kqStr = "";
            string so1Str = txtSo1.Text;
            string so2Str = txtSo2.Text;
            so1 = double.Parse(txtSo1.Text);
            so2 = double.Parse(txtSo2.Text);
            //Thực hiện phép tính dựa vào phép toán được chọn
            if (radCong.Checked) kqStr = AddNumbers(so1Str, so2Str);
            else if (radTru.Checked) kq = so1 - so2;
            else if (radNhan.Checked) kq = so1 * so2;
            else if (radChia.Checked && so2 != 0) kq = so1 / so2;
            //Hiển thị kết quả lên trên ô kết quả
            txtKq.Text = kqStr.ToString();
        }

        private string AddNumbers(string so1Str, string so2Str)
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

        private string FormatResult(string raw)
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
                if (fidx >=0)
                    fracPart = fracPart.Substring(0, fidx + 1);
                else
                    fracPart = "";
            }

            if (string.IsNullOrEmpty(fracPart))
                return intPart;
            else
                return intPart + "." + fracPart;
        }

        private string SubtractAbs(string intA, string fracA, string intB, string fracB)
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
                sbFrac.Append((char) ('0' + sub));
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
                sbInt.Append((char) ('0' + sub));
            }

            // Kết hợp phần nguyên và thập phân
            string intRes = Reverse(sbInt.ToString());
            string fracRes = fracLen > 0 ? Reverse(sbFrac.ToString()) : "";

            if (fracLen > 0)
                return intRes + "." + fracRes;
            else
                return intRes;

        }

        private int CompareAbs(string intA, string fracA, string intB, string fracB)
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
            if ( cmpFrac != 0)
                return cmpFrac > 0 ? 1 : -1;

            return 0;
        }

        private string AddAbs(string intA, string fracA, string intB, string fracB)
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
                sbFrac.Append((char) ('0' + (sum % 10))) ;
                carry = sum / 10;
            }

            // Cộng phần nguyên từ phải sang trái
            StringBuilder sbInt = new StringBuilder();
            for (int i = intLen - 1; i >= 0; i--)
            {
                int da = intA[i] - '0';
                int db = intB[i] - '0';
                int sum = da + db + carry;
                sbInt.Append((char) ('0' + (sum % 10)));
                carry = sum / 10;
            }

            // Nếu còn dư thì thêm vào phần nguyên
            if (carry > 0)
                sbInt.Append((char) ('0' + carry));

            // Kết hợp phần nguyên và thập phân
            string intRes = Reverse(sbInt.ToString());
            string fracRes = fracLen > 0 ? Reverse(sbFrac.ToString()) : "";

            if (fracLen > 0)
                return intRes + "." + fracRes;
            else
                return intRes;
        }

        private string Reverse(string v)
        {
            var arr = v.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        private void ParseNumber(string norm, out bool isNegative, out string intPart, out string fracPart)
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

        private void radChia_CheckedChanged(object sender, EventArgs e)
        {
            if (txtSo2.Text == "0")
            {
                MessageBox.Show("Không thể chia cho số 0. Vui lòng nhập lại 'Số thứ hai'","Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSo2.Focus();
                return;
            }
        }

        private void TxtSo_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                errorProvider1.SetError(tb, "Ô không được để trống!");
                btnTinh.Enabled = false;
                return;
            }

            // Kiểm tra độ dài chuỗi
            if (tb.Text.Length > 30)
            {
                errorProvider1.SetError(tb, "Số không được vượt quá 30 ký tự!");
                btnTinh.Enabled = false;
                return;
            }

            // Kiểm tra số đúng định dạng hay không
            if (!double.TryParse(tb.Text,
                                 NumberStyles.Float | NumberStyles.AllowThousands,
                                 CultureInfo.InvariantCulture,
                                 out _))
            {
                errorProvider1.SetError(tb, "Số không hợp lệ!");
                btnTinh.Enabled = false;
                return;
            }

            // Xóa lỗi nếu mọi thứ hợp lệ
            errorProvider1.SetError(tb, "");
            if (errorProvider1.GetError(txtSo1) == "" && errorProvider1.GetError(txtSo2) == "")
                btnTinh.Enabled = true;
        }
    }
}
