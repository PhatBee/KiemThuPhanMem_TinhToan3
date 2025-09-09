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
            if (radCong.Checked) kqStr = BigCalculator.AddNumbers(so1Str, so2Str);
            else if (radTru.Checked) kqStr = BigCalculator.SubtractNumbers(so1Str, so2Str);
            else if (radNhan.Checked) kq = so1 * so2;
            else if (radChia.Checked && so2 != 0) kq = so1 / so2;
            //Hiển thị kết quả lên trên ô kết quả
            txtKq.Text = kqStr.ToString();
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
