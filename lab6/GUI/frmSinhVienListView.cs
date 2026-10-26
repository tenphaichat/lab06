using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BUS;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmSinhVienListView : Form
    {
        private readonly SinhVienBUS _bus;
        private ListView lvSinhVien;
        private ImageList imgListSmall;
        private ImageList imgListLarge;

        // Vùng xem chi tiết bên phải
        private GroupBox grpChiTiet;
        private Label lblValMaSV;
        private Label lblValHoTen;
        private Label lblValNgaySinh;
        private Label lblValGioiTinh;
        private Label lblValKhoa;
        private Label lblValDiemTB;
        private Label lblValXepLoai;
        private Label lblValTuoi;
        private Label lblValSoThich;
        private PictureBox picAvatar;

        private ComboBox cboViewMode;
        private ComboBox cboLocKhoa;

        public frmSinhVienListView()
        {
            _bus = new SinhVienBUS();
            InitializeComponent();
        }

        public frmSinhVienListView(SinhVienBUS bus)
        {
            _bus = bus ?? new SinhVienBUS();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 3 & 8: Danh sách Sinh viên bằng ListView có ImageList biểu tượng học lực";
            this.Size = new Size(1020, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            CreateImageLists();

            // Panel bộ lọc trên cùng
            var pnlTop = new Panel
            {
                Location = new Point(20, 15),
                Size = new Size(960, 40)
            };

            var lblLoc = new Label
            {
                Text = "Lọc theo khoa:",
                Location = new Point(0, 8),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cboLocKhoa = new ComboBox
            {
                Location = new Point(105, 7),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboLocKhoa.Items.AddRange(new object[] { "Tất cả các khoa", "Công nghệ thông tin", "Kinh tế", "Ngoại ngữ" });
            cboLocKhoa.SelectedIndex = 0;
            cboLocKhoa.SelectedIndexChanged += (s, e) => FilterAndLoad();

            var lblCheDoXem = new Label
            {
                Text = "Chế độ xem (View):",
                Location = new Point(340, 8),
                Size = new Size(130, 25),
                TextAlign = ContentAlignment.MiddleLeft
            };

            cboViewMode = new ComboBox
            {
                Location = new Point(475, 7),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboViewMode.Items.AddRange(new object[] { "Details (Chi tiết)", "LargeIcon (Icon lớn)", "SmallIcon (Icon nhỏ)", "List (Danh sách)", "Tile (Thẻ)" });
            cboViewMode.SelectedIndex = 0;
            cboViewMode.SelectedIndexChanged += CboViewMode_SelectedIndexChanged;

            pnlTop.Controls.AddRange(new Control[] { lblLoc, cboLocKhoa, lblCheDoXem, cboViewMode });

            // ListView bên trái
            lvSinhVien = new ListView
            {
                Location = new Point(20, 65),
                Size = new Size(610, 525),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false,
                SmallImageList = imgListSmall,
                LargeImageList = imgListLarge
            };
            lvSinhVien.SelectedIndexChanged += LvSinhVien_SelectedIndexChanged;

            SetupListViewColumns();

            // GroupBox Chi tiết bên phải
            grpChiTiet = new GroupBox
            {
                Text = "Hồ sơ chi tiết sinh viên (Tag Binding)",
                Location = new Point(645, 65),
                Size = new Size(335, 525),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right
            };

            picAvatar = new PictureBox
            {
                Location = new Point(25, 30),
                Size = new Size(100, 120),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            lblValMaSV = CreateDetailRow(grpChiTiet, "Mã SV:", 165);
            lblValHoTen = CreateDetailRow(grpChiTiet, "Họ và tên:", 200);
            lblValNgaySinh = CreateDetailRow(grpChiTiet, "Ngày sinh:", 235);
            lblValTuoi = CreateDetailRow(grpChiTiet, "Tuổi:", 270);
            lblValGioiTinh = CreateDetailRow(grpChiTiet, "Giới tính:", 305);
            lblValKhoa = CreateDetailRow(grpChiTiet, "Khoa:", 340);
            lblValDiemTB = CreateDetailRow(grpChiTiet, "Điểm TB:", 375);
            lblValXepLoai = CreateDetailRow(grpChiTiet, "Xếp loại:", 410);
            lblValSoThich = CreateDetailRow(grpChiTiet, "Sở thích:", 445);

            grpChiTiet.Controls.Add(picAvatar);

            this.Controls.AddRange(new Control[] { pnlTop, lvSinhVien, grpChiTiet });
            this.Load += (s, e) => FilterAndLoad();
        }

        private Label CreateDetailRow(GroupBox grp, string labelText, int top)
        {
            var lblTitle = new Label
            {
                Text = labelText,
                Location = new Point(20, top),
                Size = new Size(85, 25),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            var lblValue = new Label
            {
                Text = "-",
                Location = new Point(110, top),
                Size = new Size(210, 25),
                ForeColor = SystemColors.WindowText
            };
            grp.Controls.Add(lblTitle);
            grp.Controls.Add(lblValue);
            return lblValue;
        }

        // Bài 8: Khởi tạo ImageList biểu tượng xếp loại học lực sinh viên
        private void CreateImageLists()
        {
            imgListSmall = new ImageList { ImageSize = new Size(20, 20), ColorDepth = ColorDepth.Depth32Bit };
            imgListLarge = new ImageList { ImageSize = new Size(40, 40), ColorDepth = ColorDepth.Depth32Bit };

            // 0: Xuất sắc (Gold star / circle)
            imgListSmall.Images.Add(DrawGradeIcon(20, "★", Color.DarkGoldenrod, Color.Gold));
            imgListLarge.Images.Add(DrawGradeIcon(40, "XS", Color.DarkGoldenrod, Color.Gold));

            // 1: Giỏi (Blue)
            imgListSmall.Images.Add(DrawGradeIcon(20, "G", Color.DarkBlue, Color.DodgerBlue));
            imgListLarge.Images.Add(DrawGradeIcon(40, "G", Color.DarkBlue, Color.DodgerBlue));

            // 2: Khá (Green)
            imgListSmall.Images.Add(DrawGradeIcon(20, "K", Color.DarkGreen, Color.SeaGreen));
            imgListLarge.Images.Add(DrawGradeIcon(40, "K", Color.DarkGreen, Color.SeaGreen));

            // 3: Trung bình (Orange)
            imgListSmall.Images.Add(DrawGradeIcon(20, "TB", Color.DarkOrange, Color.Orange));
            imgListLarge.Images.Add(DrawGradeIcon(40, "TB", Color.DarkOrange, Color.Orange));

            // 4: Yếu (Gray / Red)
            imgListSmall.Images.Add(DrawGradeIcon(20, "Y", Color.DarkRed, Color.Crimson));
            imgListLarge.Images.Add(DrawGradeIcon(40, "Y", Color.DarkRed, Color.Crimson));
        }

        private Image DrawGradeIcon(int size, string text, Color borderColor, Color fillColor)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(fillColor))
                using (var pen = new Pen(borderColor, 1.5f))
                {
                    g.FillEllipse(brush, 1, 1, size - 3, size - 3);
                    g.DrawEllipse(pen, 1, 1, size - 3, size - 3);
                }

                using (var textBrush = new SolidBrush(Color.White))
                {
                    float fontSize = size >= 32 ? 10f : (text.Length > 1 ? 6.5f : 7.5f);
                    using (var font = new Font("Segoe UI", fontSize, FontStyle.Bold))
                    {
                        var sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString(text, font, textBrush, new RectangleF(0, 0, size, size), sf);
                    }
                }
            }
            return bmp;
        }

        private int GetImageIndexForGrade(string xepLoai)
        {
            switch (xepLoai)
            {
                case "Xuất sắc": return 0;
                case "Giỏi": return 1;
                case "Khá": return 2;
                case "Trung bình": return 3;
                default: return 4;
            }
        }

        // Bài 3: Danh sách chi tiết bằng ListView
        private void SetupListViewColumns()
        {
            lvSinhVien.Columns.Clear();
            lvSinhVien.Columns.Add("Mã SV", 95);
            lvSinhVien.Columns.Add("Họ và tên", 185);
            lvSinhVien.Columns.Add("Khoa đào tạo", 150);
            lvSinhVien.Columns.Add("Điểm TB", 75);
            lvSinhVien.Columns.Add("Xếp loại", 95);
        }

        private void FilterAndLoad()
        {
            string khoa = cboLocKhoa.SelectedItem?.ToString() ?? "Tất cả các khoa";
            var all = _bus.GetAll();
            var filtered = new List<SinhVien>();

            foreach (var sv in all)
            {
                if (khoa == "Tất cả các khoa" || sv.Khoa == khoa)
                {
                    filtered.Add(sv);
                }
            }

            LoadListView(filtered);
        }

        private void LoadListView(IEnumerable<SinhVien> data)
        {
            lvSinhVien.Items.Clear();
            foreach (SinhVien sv in data)
            {
                // Bài 8: Gán ImageIndex theo xếp loại
                int imgIdx = GetImageIndexForGrade(sv.XepLoai);
                ListViewItem item = new ListViewItem(sv.MaSV, imgIdx);
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(sv.Khoa);
                item.SubItems.Add(sv.DiemTB.ToString("N2"));
                item.SubItems.Add(sv.XepLoai);

                // Bài 3: Gán Tag = sv
                item.Tag = sv;
                lvSinhVien.Items.Add(item);
            }

            if (lvSinhVien.Items.Count > 0)
            {
                lvSinhVien.Items[0].Selected = true;
                DisplayDetails(lvSinhVien.Items[0].Tag as SinhVien);
            }
            else
            {
                ClearDetails();
            }
        }

        private void LvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSinhVien.SelectedItems.Count > 0)
            {
                var sv = lvSinhVien.SelectedItems[0].Tag as SinhVien;
                DisplayDetails(sv);
            }
        }

        private void DisplayDetails(SinhVien sv)
        {
            if (sv == null) return;
            lblValMaSV.Text = sv.MaSV;
            lblValHoTen.Text = sv.HoTen;
            lblValNgaySinh.Text = sv.NgaySinh.ToString("dd/MM/yyyy");
            lblValTuoi.Text = $"{sv.Tuoi} tuổi";
            lblValGioiTinh.Text = sv.GioiTinh;
            lblValKhoa.Text = sv.Khoa;
            lblValDiemTB.Text = sv.DiemTB.ToString("0.00");
            lblValXepLoai.Text = sv.XepLoai;
            lblValSoThich.Text = string.IsNullOrWhiteSpace(sv.SoThich) ? "(Không)" : sv.SoThich;

            string imgFile = !string.IsNullOrWhiteSpace(sv.DuongDanAnh) ? sv.DuongDanAnh : "avatar_thang.png";
            string fullPath = Path.Combine(Application.StartupPath, "Images", imgFile);
            if (File.Exists(fullPath))
            {
                using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    picAvatar.Image = Image.FromStream(fs);
                }
            }
            else
            {
                picAvatar.Image = null;
            }
        }

        private void ClearDetails()
        {
            lblValMaSV.Text = "-";
            lblValHoTen.Text = "-";
            lblValNgaySinh.Text = "-";
            lblValTuoi.Text = "-";
            lblValGioiTinh.Text = "-";
            lblValKhoa.Text = "-";
            lblValDiemTB.Text = "-";
            lblValXepLoai.Text = "-";
            lblValSoThich.Text = "-";
            picAvatar.Image = null;
        }

        private void CboViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboViewMode.SelectedIndex)
            {
                case 0: lvSinhVien.View = View.Details; break;
                case 1: lvSinhVien.View = View.LargeIcon; break;
                case 2: lvSinhVien.View = View.SmallIcon; break;
                case 3: lvSinhVien.View = View.List; break;
                case 4: lvSinhVien.View = View.Tile; break;
            }
        }
    }
}
