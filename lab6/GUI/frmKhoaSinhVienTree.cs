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
    public class frmKhoaSinhVienTree : Form
    {
        private readonly SinhVienBUS _bus;
        private TreeView tvKhoa;
        private ImageList imgListTree;

        // Panel chi tiết bên phải
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
        private Label lblHuongDan;

        public frmKhoaSinhVienTree()
        {
            _bus = new SinhVienBUS();
            InitializeComponent();
        }

        public frmKhoaSinhVienTree(SinhVienBUS bus)
        {
            _bus = bus ?? new SinhVienBUS();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 4: Cây phân cấp Khoa - Sinh viên bằng TreeView";
            this.Size = new Size(1000, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            CreateTreeIcons();

            // Panel công cụ trên
            var pnlTop = new Panel
            {
                Location = new Point(20, 15),
                Size = new Size(940, 40)
            };

            var btnExpand = new Button
            {
                Text = "Mở rộng tất cả",
                Location = new Point(0, 4),
                Size = new Size(120, 32)
            };
            btnExpand.Click += (s, e) => tvKhoa.ExpandAll();

            var btnCollapse = new Button
            {
                Text = "Thu gọn tất cả",
                Location = new Point(130, 4),
                Size = new Size(120, 32)
            };
            btnCollapse.Click += (s, e) => tvKhoa.CollapseAll();

            var btnRefresh = new Button
            {
                Text = "Làm mới cây",
                Location = new Point(260, 4),
                Size = new Size(110, 32)
            };
            btnRefresh.Click += (s, e) => LoadDataToTree();

            pnlTop.Controls.AddRange(new Control[] { btnExpand, btnCollapse, btnRefresh });

            // TreeView bên trái
            tvKhoa = new TreeView
            {
                Location = new Point(20, 65),
                Size = new Size(450, 520),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                ImageList = imgListTree,
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true,
                BorderStyle = BorderStyle.Fixed3D
            };
            tvKhoa.AfterSelect += TvKhoa_AfterSelect;

            // GroupBox Chi tiết bên phải
            grpChiTiet = new GroupBox
            {
                Text = "Hồ sơ chi tiết sinh viên được chọn",
                Location = new Point(485, 65),
                Size = new Size(475, 520),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            lblHuongDan = new Label
            {
                Text = "<- Chọn một sinh viên trên cây bên trái để xem hồ sơ chi tiết",
                Location = new Point(25, 30),
                Size = new Size(420, 25),
                ForeColor = SystemColors.GrayText,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Italic)
            };

            picAvatar = new PictureBox
            {
                Location = new Point(25, 65),
                Size = new Size(110, 135),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            lblValMaSV = CreateDetailField(grpChiTiet, "Mã sinh viên:", 215);
            lblValHoTen = CreateDetailField(grpChiTiet, "Họ và tên:", 250);
            lblValNgaySinh = CreateDetailField(grpChiTiet, "Ngày sinh:", 285);
            lblValTuoi = CreateDetailField(grpChiTiet, "Tuổi:", 320);
            lblValGioiTinh = CreateDetailField(grpChiTiet, "Giới tính:", 355);
            lblValKhoa = CreateDetailField(grpChiTiet, "Khoa đào tạo:", 390);
            lblValDiemTB = CreateDetailField(grpChiTiet, "Điểm trung bình:", 425);
            lblValXepLoai = CreateDetailField(grpChiTiet, "Xếp loại học lực:", 460);
            lblValSoThich = CreateDetailField(grpChiTiet, "Sở thích cá nhân:", 490);

            grpChiTiet.Controls.AddRange(new Control[] { lblHuongDan, picAvatar });

            this.Controls.AddRange(new Control[] { pnlTop, tvKhoa, grpChiTiet });
            this.Load += (s, e) => LoadDataToTree();
        }

        private Label CreateDetailField(GroupBox grp, string title, int top)
        {
            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(25, top),
                Size = new Size(125, 25),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            var lblVal = new Label
            {
                Text = "-",
                Location = new Point(155, top),
                Size = new Size(300, 25),
                ForeColor = SystemColors.WindowText
            };
            grp.Controls.Add(lblTitle);
            grp.Controls.Add(lblVal);
            return lblVal;
        }

        private void CreateTreeIcons()
        {
            imgListTree = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };

            // 0: Khoa icon (Folder)
            var bmpFolder = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmpFolder))
            {
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.Goldenrod))
                {
                    g.FillRectangle(brush, 1, 3, 14, 11);
                }
                using (var brushTab = new SolidBrush(Color.DarkGoldenrod))
                {
                    g.FillRectangle(brushTab, 1, 1, 6, 4);
                }
            }
            imgListTree.Images.Add(bmpFolder);

            // 1: Sinh viên icon (Person)
            var bmpPerson = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmpPerson))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.SteelBlue))
                {
                    g.FillEllipse(brush, 5, 1, 6, 6);
                    g.FillPie(brush, 2, 8, 12, 12, 180, 180);
                }
            }
            imgListTree.Images.Add(bmpPerson);
        }

        // Bài 4: Nạp cây Khoa - Sinh viên
        public void LoadDataToTree()
        {
            tvKhoa.Nodes.Clear();
            var data = _bus.GetFacultyGroups();

            foreach (var group in data)
            {
                TreeNode khoaNode = new TreeNode($"{group.Key} ({group.Value.Count} SV)")
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    Tag = null // Node gốc không có Tag sinh viên
                };

                foreach (SinhVien sv in group.Value)
                {
                    TreeNode svNode = new TreeNode($"{sv.MaSV} - {sv.HoTen}")
                    {
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        Tag = sv // Lưu đối tượng trong thuộc tính Tag
                    };
                    khoaNode.Nodes.Add(svNode);
                }

                tvKhoa.Nodes.Add(khoaNode);
            }

            tvKhoa.ExpandAll();

            // Chọn sinh viên đầu tiên
            if (tvKhoa.Nodes.Count > 0 && tvKhoa.Nodes[0].Nodes.Count > 0)
            {
                tvKhoa.SelectedNode = tvKhoa.Nodes[0].Nodes[0];
            }
        }

        private void TvKhoa_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            var sv = e.Node.Tag as SinhVien;
            if (sv != null)
            {
                lblHuongDan.Visible = false;
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
            else
            {
                lblHuongDan.Visible = true;
                lblHuongDan.Text = $"Đang chọn: {e.Node.Text}. Hãy bấm vào một sinh viên bên dưới để xem hồ sơ.";
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
        }
    }
}
