using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BUS;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmDanhMucSanPhamTree : Form
    {
        private readonly SanPhamBUS _bus;
        private TreeView tvDanhMuc;
        private ContextMenuStrip cmsTree;
        private ImageList imgListTree;

        // Chi tiết bên phải
        private GroupBox grpChiTiet;
        private Label lblLoaiNode;
        private Label lblTenNode;
        private Label lblMaSP;
        private Label lblDonGia;
        private Label lblSoLuong;
        private Label lblThanhTien;
        private Label lblNgayNhap;
        private Label lblGhiChu;

        public frmDanhMucSanPhamTree()
        {
            _bus = new SanPhamBUS();
            InitializeComponent();
        }

        public frmDanhMucSanPhamTree(SanPhamBUS bus)
        {
            _bus = bus ?? new SanPhamBUS();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 7: TreeView quản lý Danh mục - Sản phẩm có ContextMenuStrip";
            this.Size = new Size(1000, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            CreateIcons();
            SetupContextMenu();

            // Panel công cụ
            var pnlTop = new Panel
            {
                Location = new Point(20, 15),
                Size = new Size(940, 40)
            };

            var btnThemDM = new Button
            {
                Text = "+ Thêm danh mục",
                Location = new Point(0, 4),
                Size = new Size(130, 32)
            };
            btnThemDM.Click += (s, e) => ActionThemDanhMuc();

            var btnThemSP = new Button
            {
                Text = "+ Thêm sản phẩm",
                Location = new Point(140, 4),
                Size = new Size(130, 32)
            };
            btnThemSP.Click += (s, e) => ActionThemSanPham();

            var btnDoiTen = new Button
            {
                Text = "Đổi tên node",
                Location = new Point(280, 4),
                Size = new Size(110, 32)
            };
            btnDoiTen.Click += (s, e) => ActionDoiTen();

            var btnXoaNode = new Button
            {
                Text = "Xóa node",
                Location = new Point(400, 4),
                Size = new Size(100, 32)
            };
            btnXoaNode.Click += (s, e) => ActionXoaNode();

            var btnExpand = new Button
            {
                Text = "Mở rộng",
                Location = new Point(510, 4),
                Size = new Size(85, 32)
            };
            btnExpand.Click += (s, e) => tvDanhMuc.ExpandAll();

            var btnCollapse = new Button
            {
                Text = "Thu gọn",
                Location = new Point(605, 4),
                Size = new Size(85, 32)
            };
            btnCollapse.Click += (s, e) => tvDanhMuc.CollapseAll();

            pnlTop.Controls.AddRange(new Control[] { btnThemDM, btnThemSP, btnDoiTen, btnXoaNode, btnExpand, btnCollapse });

            // TreeView
            tvDanhMuc = new TreeView
            {
                Location = new Point(20, 65),
                Size = new Size(460, 520),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left,
                ContextMenuStrip = cmsTree,
                ImageList = imgListTree,
                LabelEdit = true,
                ShowLines = true,
                ShowPlusMinus = true,
                ShowRootLines = true,
                BorderStyle = BorderStyle.Fixed3D
            };
            tvDanhMuc.AfterSelect += TvDanhMuc_AfterSelect;
            tvDanhMuc.NodeMouseClick += (s, e) => { if (e.Button == MouseButtons.Right) tvDanhMuc.SelectedNode = e.Node; };
            tvDanhMuc.AfterLabelEdit += TvDanhMuc_AfterLabelEdit;

            // GroupBox Chi tiết bên phải
            grpChiTiet = new GroupBox
            {
                Text = "Thông tin chi tiết đối tượng trên cây",
                Location = new Point(495, 65),
                Size = new Size(465, 520),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            var lblHuongDanContext = new Label
            {
                Text = "💡 Mẹo: Nhấp chuột phải vào bất kỳ node nào trên cây để mở ContextMenuStrip (Thêm danh mục, Thêm sản phẩm, Đổi tên, Xóa node).",
                Location = new Point(20, 28),
                Size = new Size(425, 45),
                ForeColor = SystemColors.HotTrack
            };

            lblLoaiNode = CreateRow(grpChiTiet, "Loại node:", 85);
            lblTenNode = CreateRow(grpChiTiet, "Tên hiển thị:", 125);
            lblMaSP = CreateRow(grpChiTiet, "Mã sản phẩm:", 165);
            lblDonGia = CreateRow(grpChiTiet, "Đơn giá:", 205);
            lblSoLuong = CreateRow(grpChiTiet, "Số lượng tồn:", 245);
            lblThanhTien = CreateRow(grpChiTiet, "Tổng thành tiền:", 285);
            lblNgayNhap = CreateRow(grpChiTiet, "Ngày nhập:", 325);
            lblGhiChu = CreateRow(grpChiTiet, "Ghi chú:", 365);

            grpChiTiet.Controls.Add(lblHuongDanContext);

            this.Controls.AddRange(new Control[] { pnlTop, tvDanhMuc, grpChiTiet });
            this.Load += (s, e) => LoadTreeData();
        }

        private Label CreateRow(GroupBox grp, string title, int top)
        {
            var lblT = new Label
            {
                Text = title,
                Location = new Point(20, top),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            var lblV = new Label
            {
                Text = "-",
                Location = new Point(145, top),
                Size = new Size(300, 25),
                ForeColor = SystemColors.WindowText
            };
            grp.Controls.Add(lblT);
            grp.Controls.Add(lblV);
            return lblV;
        }

        private void CreateIcons()
        {
            imgListTree = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };

            // 0: Category folder
            var bmpCat = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmpCat))
            {
                g.Clear(Color.Transparent);
                using (var b = new SolidBrush(Color.Chocolate))
                {
                    g.FillRectangle(b, 1, 3, 14, 11);
                }
                using (var b2 = new SolidBrush(Color.SaddleBrown))
                {
                    g.FillRectangle(b2, 1, 1, 6, 4);
                }
            }
            imgListTree.Images.Add(bmpCat);

            // 1: Product tag / box
            var bmpProd = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(bmpProd))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var b = new SolidBrush(Color.RoyalBlue))
                {
                    g.FillRectangle(b, 2, 2, 12, 12);
                }
                using (var p = new Pen(Color.White, 1.5f))
                {
                    g.DrawLine(p, 5, 8, 11, 8);
                }
            }
            imgListTree.Images.Add(bmpProd);
        }

        // Bài 7: ContextMenuStrip quản lý thêm, đổi tên, xóa node
        private void SetupContextMenu()
        {
            cmsTree = new ContextMenuStrip();

            var mnuAddCategory = new ToolStripMenuItem("Thêm danh mục mới...");
            mnuAddCategory.Click += (s, e) => ActionThemDanhMuc();

            var mnuAddProduct = new ToolStripMenuItem("Thêm sản phẩm vào danh mục này...");
            mnuAddProduct.Click += (s, e) => ActionThemSanPham();

            var mnuRename = new ToolStripMenuItem("Đổi tên node (F2)");
            mnuRename.Click += (s, e) => ActionDoiTen();

            var mnuDelete = new ToolStripMenuItem("Xóa node...");
            mnuDelete.Click += (s, e) => ActionXoaNode();

            cmsTree.Items.AddRange(new ToolStripItem[] {
                mnuAddCategory,
                mnuAddProduct,
                new ToolStripSeparator(),
                mnuRename,
                mnuDelete
            });
        }

        public void LoadTreeData()
        {
            tvDanhMuc.Nodes.Clear();
            var groups = _bus.GetCategoryGroups();

            foreach (var kvp in groups)
            {
                TreeNode catNode = new TreeNode(kvp.Key)
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    Tag = "CATEGORY:" + kvp.Key
                };

                foreach (SanPham sp in kvp.Value)
                {
                    TreeNode spNode = new TreeNode($"{sp.TenSP} ({sp.DonGia:N0} đ)")
                    {
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        Tag = sp
                    };
                    catNode.Nodes.Add(spNode);
                }

                tvDanhMuc.Nodes.Add(catNode);
            }

            tvDanhMuc.ExpandAll();

            if (tvDanhMuc.Nodes.Count > 0)
            {
                tvDanhMuc.SelectedNode = tvDanhMuc.Nodes[0];
            }
        }

        private void TvDanhMuc_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            var sp = e.Node.Tag as SanPham;
            if (sp != null)
            {
                lblLoaiNode.Text = "Sản phẩm cụ thể";
                lblTenNode.Text = sp.TenSP;
                lblMaSP.Text = sp.MaSP;
                lblDonGia.Text = $"{sp.DonGia:N0} VNĐ";
                lblSoLuong.Text = $"{sp.SoLuong:N0} chiếc";
                lblThanhTien.Text = $"{sp.ThanhTien:N0} VNĐ";
                lblNgayNhap.Text = sp.NgayNhap.ToString("dd/MM/yyyy");
                lblGhiChu.Text = string.IsNullOrWhiteSpace(sp.GhiChu) ? "(Không có)" : sp.GhiChu;
            }
            else
            {
                // Danh mục
                string catName = e.Node.Text;
                lblLoaiNode.Text = "Danh mục sản phẩm";
                lblTenNode.Text = catName;
                lblMaSP.Text = "-";
                lblDonGia.Text = "-";
                lblSoLuong.Text = $"{e.Node.Nodes.Count} sản phẩm trực thuộc";
                lblThanhTien.Text = "-";
                lblNgayNhap.Text = "-";
                lblGhiChu.Text = $"Danh mục chứa {e.Node.Nodes.Count} mặt hàng trong kho.";
            }
        }

        private void ActionThemDanhMuc()
        {
            string catName = ShowInputDialog("Nhập tên danh mục mới:", "Thêm danh mục", "Thiết bị mạng");
            if (!string.IsNullOrWhiteSpace(catName))
            {
                // Kiểm tra xem danh mục đã tồn tại chưa
                foreach (TreeNode node in tvDanhMuc.Nodes)
                {
                    if (string.Equals(node.Text, catName.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Danh mục này đã tồn tại trên cây.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                TreeNode newNode = new TreeNode(catName.Trim())
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 0,
                    Tag = "CATEGORY:" + catName.Trim()
                };
                tvDanhMuc.Nodes.Add(newNode);
                tvDanhMuc.SelectedNode = newNode;
                MessageBox.Show($"Đã thêm danh mục: {catName.Trim()}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActionThemSanPham()
        {
            TreeNode targetCatNode = tvDanhMuc.SelectedNode;
            if (targetCatNode == null)
            {
                if (tvDanhMuc.Nodes.Count > 0) targetCatNode = tvDanhMuc.Nodes[0];
                else
                {
                    MessageBox.Show("Vui lòng tạo danh mục trước khi thêm sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            // Nếu đang chọn node con (sản phẩm), lấy node cha (danh mục)
            if (targetCatNode.Parent != null)
            {
                targetCatNode = targetCatNode.Parent;
            }

            string spName = ShowInputDialog($"Nhập tên sản phẩm thuộc '{targetCatNode.Text}':", "Thêm sản phẩm", "Sản phẩm mới");
            if (!string.IsNullOrWhiteSpace(spName))
            {
                string randomMa = "SP" + DateTime.Now.ToString("mmss");
                var newSp = new SanPham(randomMa, spName.Trim(), targetCatNode.Text, 5, 1500000m, DateTime.Today, "Thêm từ TreeView");

                try
                {
                    _bus.Add(newSp);
                    TreeNode spNode = new TreeNode($"{newSp.TenSP} ({newSp.DonGia:N0} đ)")
                    {
                        ImageIndex = 1,
                        SelectedImageIndex = 1,
                        Tag = newSp
                    };
                    targetCatNode.Nodes.Add(spNode);
                    targetCatNode.Expand();
                    tvDanhMuc.SelectedNode = spNode;
                    MessageBox.Show($"Đã thêm sản phẩm '{newSp.TenSP}' vào danh mục '{targetCatNode.Text}'.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ActionDoiTen()
        {
            if (tvDanhMuc.SelectedNode != null)
            {
                tvDanhMuc.SelectedNode.BeginEdit();
            }
        }

        private void TvDanhMuc_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            var sp = e.Node.Tag as SanPham;
            if (sp != null)
            {
                sp.TenSP = e.Label.Trim();
                e.Node.Text = $"{sp.TenSP} ({sp.DonGia:N0} đ)";
                lblTenNode.Text = sp.TenSP;
            }
            else
            {
                e.Node.Tag = "CATEGORY:" + e.Label.Trim();
                lblTenNode.Text = e.Label.Trim();
            }
        }

        private void ActionXoaNode()
        {
            if (tvDanhMuc.SelectedNode == null)
            {
                MessageBox.Show("Vui lòng chọn node cần xóa trên cây.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = tvDanhMuc.SelectedNode;
            var res = MessageBox.Show($"Bạn có chắc chắn muốn xóa node '{selected.Text}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                var sp = selected.Tag as SanPham;
                if (sp != null)
                {
                    _bus.Delete(sp.MaSP);
                }
                selected.Remove();
                MessageBox.Show("Đã xóa node thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string ShowInputDialog(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form
            {
                Width = 420,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false,
                Font = new Font("Segoe UI", 9.5f)
            };

            Label textLabel = new Label { Left = 20, Top = 20, Text = text, AutoSize = true };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 360, Text = defaultValue };
            Button confirmation = new Button { Text = "Xác nhận", Left = 180, Width = 95, Top = 90, DialogResult = DialogResult.OK };
            Button cancel = new Button { Text = "Hủy", Left = 285, Width = 95, Top = 90, DialogResult = DialogResult.Cancel };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog(this) == DialogResult.OK ? textBox.Text : "";
        }
    }
}
