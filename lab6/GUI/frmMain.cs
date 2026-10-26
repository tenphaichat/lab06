using System;
using System.Drawing;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BUS;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmMain : Form
    {
        private MenuStrip msMain;
        private ToolStrip tsMain;
        private StatusStrip ssMain;
        private ToolStripStatusLabel tslUser;
        private ToolStripStatusLabel tslSpring;
        private ToolStripStatusLabel tslClock;
        private System.Windows.Forms.Timer tmrClock;

        // Shared BUS instances across forms to keep in-memory changes synchronized
        private readonly SinhVienBUS _sinhVienBus = new SinhVienBUS();
        private readonly SanPhamBUS _sanPhamBus = new SanPhamBUS();

        public frmMain()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "LAB 06: DATAGRIDVIEW, LISTVIEW, TREEVIEW & MÔ HÌNH 3 LỚP - PHAN VĂN THẮNG (24002545)";
            this.Size = new Size(1150, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            SetupMenuStrip();
            SetupToolStrip();
            SetupStatusStrip();

            this.FormClosing += FrmMain_FormClosing;
        }

        private void SetupMenuStrip()
        {
            msMain = new MenuStrip { Font = new Font("Segoe UI", 9.5f) };

            // 1. Hệ thống
            var mnuHeThong = new ToolStripMenuItem("Hệ thống (&S)");
            var mnuDangXuat = new ToolStripMenuItem("Đăng xuất", null, (s, e) => MessageBox.Show("Đã đăng xuất phiên làm việc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information));
            var mnuThoat = new ToolStripMenuItem("Thoát ứng dụng (&X)", null, (s, e) => this.Close());
            mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] { mnuDangXuat, new ToolStripSeparator(), mnuThoat });

            // 2. Quản lý Sinh viên
            var mnuSinhVien = new ToolStripMenuItem("Quản lý Sinh viên (&V)");
            var mnuDgvSV = new ToolStripMenuItem("1. Sinh viên - DataGridView (Bài 1, 2, 5, 9)", null, (s, e) => OpenChildForm(() => new frmSinhVienDGV(_sinhVienBus)));
            var mnuLvSV = new ToolStripMenuItem("2. Sinh viên - ListView Details & ImageList (Bài 3, 8)", null, (s, e) => OpenChildForm(() => new frmSinhVienListView(_sinhVienBus)));
            var mnuTvSV = new ToolStripMenuItem("3. Cây Khoa - Sinh viên - TreeView (Bài 4)", null, (s, e) => OpenChildForm(() => new frmKhoaSinhVienTree(_sinhVienBus)));
            mnuSinhVien.DropDownItems.AddRange(new ToolStripItem[] { mnuDgvSV, mnuLvSV, mnuTvSV });

            // 3. Quản lý Sản phẩm
            var mnuSanPham = new ToolStripMenuItem("Quản lý Sản phẩm (&P)");
            var mnuDgvSP = new ToolStripMenuItem("4. Sản phẩm - DataGridView (Bài 6)", null, (s, e) => OpenChildForm(() => new frmSanPhamDGV(_sanPhamBus)));
            var mnuTvSP = new ToolStripMenuItem("5. Danh mục - Sản phẩm TreeView & ContextMenu (Bài 7)", null, (s, e) => OpenChildForm(() => new frmDanhMucSanPhamTree(_sanPhamBus)));
            mnuSanPham.DropDownItems.AddRange(new ToolStripItem[] { mnuDgvSP, mnuTvSP });

            // 4. Kiến trúc 3 Lớp
            var mnuKienTruc = new ToolStripMenuItem("Kiến trúc 3 Lớp (&A)");
            var mnuThongTin3Lop = new ToolStripMenuItem("6. Sơ đồ kiến trúc 3 lớp DTO - DAL - BUS - GUI (Bài 10)", null, (s, e) => OpenChildForm(() => new frmKienTruc3Lop()));
            mnuKienTruc.DropDownItems.Add(mnuThongTin3Lop);

            // 5. Cửa sổ MDI
            var mnuCuaSo = new ToolStripMenuItem("Cửa sổ (&W)");
            var mnuCascade = new ToolStripMenuItem("Xếp tầng (Cascade)", null, (s, e) => this.LayoutMdi(MdiLayout.Cascade));
            var mnuTileH = new ToolStripMenuItem("Xếp ngang (Tile Horizontal)", null, (s, e) => this.LayoutMdi(MdiLayout.TileHorizontal));
            var mnuTileV = new ToolStripMenuItem("Xếp dọc (Tile Vertical)", null, (s, e) => this.LayoutMdi(MdiLayout.TileVertical));
            var mnuCloseAll = new ToolStripMenuItem("Đóng tất cả cửa sổ con", null, (s, e) => {
                foreach (Form child in this.MdiChildren) child.Close();
            });
            mnuCuaSo.DropDownItems.AddRange(new ToolStripItem[] { mnuCascade, mnuTileH, mnuTileV, new ToolStripSeparator(), mnuCloseAll });

            // 6. Trợ giúp
            var mnuTroGiup = new ToolStripMenuItem("Trợ giúp (&H)");
            var mnuGioiThieu = new ToolStripMenuItem("Giới thiệu tác giả (Phan Văn Thắng - 24002545)", null, (s, e) => new frmGioiThieu().ShowDialog(this));
            mnuTroGiup.DropDownItems.Add(mnuGioiThieu);

            msMain.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuSinhVien, mnuSanPham, mnuKienTruc, mnuCuaSo, mnuTroGiup });
            this.MainMenuStrip = msMain;
            this.Controls.Add(msMain);
        }

        private void SetupToolStrip()
        {
            tsMain = new ToolStrip
            {
                Font = new Font("Segoe UI", 9f),
                GripStyle = ToolStripGripStyle.Hidden,
                Padding = new Padding(5, 3, 5, 3)
            };

            var btnSV_DGV = new ToolStripButton("Sinh viên (DataGridView)", null, (s, e) => OpenChildForm(() => new frmSinhVienDGV(_sinhVienBus)));
            var btnSV_LV = new ToolStripButton("Sinh viên (ListView)", null, (s, e) => OpenChildForm(() => new frmSinhVienListView(_sinhVienBus)));
            var btnSV_Tree = new ToolStripButton("Khoa - Sinh viên (TreeView)", null, (s, e) => OpenChildForm(() => new frmKhoaSinhVienTree(_sinhVienBus)));

            var btnSP_DGV = new ToolStripButton("Sản phẩm (DataGridView)", null, (s, e) => OpenChildForm(() => new frmSanPhamDGV(_sanPhamBus)));
            var btnSP_Tree = new ToolStripButton("Danh mục SP (TreeView)", null, (s, e) => OpenChildForm(() => new frmDanhMucSanPhamTree(_sanPhamBus)));

            var btn3Lop = new ToolStripButton("Kiến trúc 3 lớp (Bài 10)", null, (s, e) => OpenChildForm(() => new frmKienTruc3Lop()));
            var btnAbout = new ToolStripButton("Giới thiệu tác giả", null, (s, e) => new frmGioiThieu().ShowDialog(this));

            tsMain.Items.AddRange(new ToolStripItem[] {
                btnSV_DGV, btnSV_LV, btnSV_Tree,
                new ToolStripSeparator(),
                btnSP_DGV, btnSP_Tree,
                new ToolStripSeparator(),
                btn3Lop, btnAbout
            });

            this.Controls.Add(tsMain);
        }

        private void SetupStatusStrip()
        {
            ssMain = new StatusStrip { Font = new Font("Segoe UI", 9f) };

            tslUser = new ToolStripStatusLabel
            {
                Text = "Sinh viên: Phan Văn Thắng | MSSV: 24002545 | Lớp: IT3212 | ĐH Công nghệ Đông Á",
                AutoSize = true
            };

            tslSpring = new ToolStripStatusLabel { Spring = true };

            tslClock = new ToolStripStatusLabel
            {
                Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                AutoSize = true
            };

            ssMain.Items.AddRange(new ToolStripItem[] { tslUser, tslSpring, tslClock });
            this.Controls.Add(ssMain);

            tmrClock = new System.Windows.Forms.Timer { Interval = 1000, Enabled = true };
            tmrClock.Tick += (s, e) => tslClock.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }

        private void OpenChildForm<T>(Func<T> createForm) where T : Form
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T)
                {
                    child.WindowState = FormWindowState.Normal;
                    child.Activate();
                    return;
                }
            }

            T form = createForm();
            form.MdiParent = this;
            form.Show();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát ứng dụng không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
