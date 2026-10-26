using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmGioiThieu : Form
    {
        public frmGioiThieu()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Giới thiệu - Tác giả & Học phần";
            this.Size = new Size(580, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            var picAvatar = new PictureBox
            {
                Location = new Point(25, 30),
                Size = new Size(130, 160),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            string avatarPath = Path.Combine(Application.StartupPath, "Images", "avatar_thang.png");
            if (File.Exists(avatarPath))
            {
                using (var fs = new FileStream(avatarPath, FileMode.Open, FileAccess.Read))
                {
                    picAvatar.Image = Image.FromStream(fs);
                }
            }

            var grpInfo = new GroupBox
            {
                Text = "Thông tin sinh viên thực hiện",
                Location = new Point(175, 20),
                Size = new Size(365, 290)
            };

            var lblHoTen = new Label
            {
                Text = "Họ và tên: Phan Văn Thắng",
                Location = new Point(20, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5f, FontStyle.Bold)
            };

            var lblMSSV = new Label
            {
                Text = "Mã sinh viên: 24002545",
                Location = new Point(20, 75),
                AutoSize = true
            };

            var lblNgaySinh = new Label
            {
                Text = "Ngày sinh: 26/03/2006",
                Location = new Point(20, 110),
                AutoSize = true
            };

            var lblLop = new Label
            {
                Text = "Lớp học phần: IT3212 (Lập trình .NET)",
                Location = new Point(20, 145),
                AutoSize = true
            };

            var lblKhoa = new Label
            {
                Text = "Khoa: Công nghệ Thông tin",
                Location = new Point(20, 180),
                AutoSize = true
            };

            var lblTruong = new Label
            {
                Text = "Trường: Đại học Công nghệ Đông Á (EAUT)",
                Location = new Point(20, 215),
                AutoSize = true
            };

            var lblDeTai = new Label
            {
                Text = "Bài thực hành: LAB 06 - Mô hình 3 lớp chưa kết nối CSDL",
                Location = new Point(20, 250),
                AutoSize = true,
                ForeColor = SystemColors.HotTrack
            };

            grpInfo.Controls.AddRange(new Control[] { lblHoTen, lblMSSV, lblNgaySinh, lblLop, lblKhoa, lblTruong, lblDeTai });

            var btnDong = new Button
            {
                Text = "Đóng",
                Location = new Point(440, 325),
                Size = new Size(100, 35),
                DialogResult = DialogResult.OK
            };
            btnDong.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { picAvatar, grpInfo, btnDong });
            this.AcceptButton = btnDong;
        }
    }
}
