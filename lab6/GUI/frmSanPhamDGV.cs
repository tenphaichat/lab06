using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BUS;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmSanPhamDGV : Form
    {
        private readonly SanPhamBUS _bus;
        private BindingList<SanPham> _dsHienThi;

        private TextBox txtMaSP;
        private TextBox txtTenSP;
        private ComboBox cboLoaiSP;
        private NumericUpDown numSoLuong;
        private TextBox txtDonGia;
        private DateTimePicker dtpNgayNhap;
        private TextBox txtGhiChu;

        private TextBox txtTimKiem;
        private Button btnTimKiem;
        private Button btnHienThiTatCa;
        private ComboBox cboSapXep;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;

        private DataGridView dgvSanPham;
        private Label lblTongMatHang;
        private Label lblTongSoLuong;
        private Label lblTongGiaTri;

        public frmSanPhamDGV()
        {
            _bus = new SanPhamBUS();
            InitializeComponent();
        }

        public frmSanPhamDGV(SanPhamBUS bus)
        {
            _bus = bus ?? new SanPhamBUS();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 6: Quản lý Sản phẩm trên DataGridView (Tìm kiếm, Sắp xếp, Tổng tồn kho)";
            this.Size = new Size(1020, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // GroupBox Nhập thông tin sản phẩm
            var grpInput = new GroupBox
            {
                Text = "Thông tin chi tiết sản phẩm",
                Location = new Point(20, 15),
                Size = new Size(960, 150)
            };

            // Hàng 1: Mã SP, Tên SP, Loại SP
            var lblMaSP = new Label { Text = "Mã sản phẩm:", Location = new Point(20, 28), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtMaSP = new TextBox { Location = new Point(120, 28), Size = new Size(160, 25) };

            var lblTenSP = new Label { Text = "Tên sản phẩm:", Location = new Point(300, 28), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtTenSP = new TextBox { Location = new Point(400, 28), Size = new Size(240, 25) };

            var lblLoaiSP = new Label { Text = "Loại sản phẩm:", Location = new Point(660, 28), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            cboLoaiSP = new ComboBox { Location = new Point(760, 28), Size = new Size(180, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboLoaiSP.Items.AddRange(new object[] { "Laptop", "Bàn phím", "Chuột", "Màn hình", "Âm thanh", "Linh kiện" });
            cboLoaiSP.SelectedIndex = 0;

            // Hàng 2: Số lượng, Đơn giá, Ngày nhập
            var lblSoLuong = new Label { Text = "Số lượng tồn:", Location = new Point(20, 68), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            numSoLuong = new NumericUpDown { Location = new Point(120, 68), Size = new Size(160, 25), Maximum = 10000, Minimum = 0, Value = 1 };

            var lblDonGia = new Label { Text = "Đơn giá (VNĐ):", Location = new Point(300, 68), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtDonGia = new TextBox { Location = new Point(400, 68), Size = new Size(240, 25) };

            var lblNgayNhap = new Label { Text = "Ngày nhập kho:", Location = new Point(660, 68), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            dtpNgayNhap = new DateTimePicker { Location = new Point(760, 68), Size = new Size(180, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };

            // Hàng 3: Ghi chú
            var lblGhiChu = new Label { Text = "Ghi chú:", Location = new Point(20, 108), Size = new Size(95, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtGhiChu = new TextBox { Location = new Point(120, 108), Size = new Size(820, 25) };

            grpInput.Controls.AddRange(new Control[] {
                lblMaSP, txtMaSP,
                lblTenSP, txtTenSP,
                lblLoaiSP, cboLoaiSP,
                lblSoLuong, numSoLuong,
                lblDonGia, txtDonGia,
                lblNgayNhap, dtpNgayNhap,
                lblGhiChu, txtGhiChu
            });

            // Panel thao tác và Tìm kiếm, Sắp xếp
            var pnlToolbar = new Panel
            {
                Location = new Point(20, 170),
                Size = new Size(960, 45)
            };

            btnThem = new Button { Text = "Thêm mới", Location = new Point(0, 7), Size = new Size(95, 32) };
            btnSua = new Button { Text = "Cập nhật", Location = new Point(102, 7), Size = new Size(95, 32) };
            btnXoa = new Button { Text = "Xóa bỏ", Location = new Point(204, 7), Size = new Size(95, 32) };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(306, 7), Size = new Size(95, 32) };

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();

            // Tìm kiếm
            var lblTim = new Label { Text = "Tìm:", Location = new Point(415, 12), Size = new Size(35, 25), TextAlign = ContentAlignment.MiddleRight };
            txtTimKiem = new TextBox { Location = new Point(455, 10), Size = new Size(130, 25) };
            btnTimKiem = new Button { Text = "Tìm", Location = new Point(590, 8), Size = new Size(60, 30) };
            btnHienThiTatCa = new Button { Text = "Tất cả", Location = new Point(655, 8), Size = new Size(65, 30) };

            btnTimKiem.Click += BtnTimKiem_Click;
            btnHienThiTatCa.Click += (s, e) => { txtTimKiem.Clear(); LoadData(); };

            // Sắp xếp
            var lblSapXep = new Label { Text = "Sắp xếp:", Location = new Point(730, 12), Size = new Size(60, 25), TextAlign = ContentAlignment.MiddleRight };
            cboSapXep = new ComboBox { Location = new Point(795, 10), Size = new Size(165, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboSapXep.Items.AddRange(new object[] {
                "Mã sản phẩm",
                "Tên sản phẩm (A-Z)",
                "Tên sản phẩm (Z-A)",
                "Đơn giá tăng dần",
                "Đơn giá giảm dần",
                "Số lượng tồn kho",
                "Tổng thành tiền"
            });
            cboSapXep.SelectedIndex = 0;
            cboSapXep.SelectedIndexChanged += CboSapXep_SelectedIndexChanged;

            pnlToolbar.Controls.AddRange(new Control[] {
                btnThem, btnSua, btnXoa, btnLamMoi,
                lblTim, txtTimKiem, btnTimKiem, btnHienThiTatCa,
                lblSapXep, cboSapXep
            });

            // DataGridView
            dgvSanPham = new DataGridView
            {
                Location = new Point(20, 220),
                Size = new Size(960, 360),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.Fixed3D,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvSanPham.CellClick += DgvSanPham_CellClick;

            SetupGridColumns();

            // Panel Thống kê tồn kho bên dưới
            var pnlSummary = new Panel
            {
                Location = new Point(20, 590),
                Size = new Size(960, 35),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblTongMatHang = new Label
            {
                Text = "Mặt hàng: 0",
                Location = new Point(15, 6),
                Size = new Size(180, 23),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            lblTongSoLuong = new Label
            {
                Text = "Tổng tồn kho: 0 chiếc",
                Location = new Point(220, 6),
                Size = new Size(220, 23),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            lblTongGiaTri = new Label
            {
                Text = "TỔNG GIÁ TRỊ HÀNG TỒN: 0 VNĐ",
                Location = new Point(480, 6),
                Size = new Size(460, 23),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = SystemColors.HotTrack,
                TextAlign = ContentAlignment.MiddleRight
            };

            pnlSummary.Controls.AddRange(new Control[] { lblTongMatHang, lblTongSoLuong, lblTongGiaTri });

            this.Controls.AddRange(new Control[] { grpInput, pnlToolbar, dgvSanPham, pnlSummary });
            this.Load += (s, e) => LoadData();
        }

        private void SetupGridColumns()
        {
            dgvSanPham.AutoGenerateColumns = false;
            dgvSanPham.Columns.Clear();

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSP",
                HeaderText = "Mã SP",
                Width = 90,
                FillWeight = 75
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenSP",
                HeaderText = "Tên sản phẩm",
                Width = 240,
                FillWeight = 200
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LoaiSP",
                HeaderText = "Loại sản phẩm",
                Width = 120,
                FillWeight = 100
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuong",
                HeaderText = "Số lượng",
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" },
                Width = 90,
                FillWeight = 75
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonGia",
                HeaderText = "Đơn giá (VNĐ)",
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" },
                Width = 130,
                FillWeight = 110
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành tiền (VNĐ)",
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" },
                Width = 150,
                FillWeight = 125
            });

            dgvSanPham.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayNhap",
                HeaderText = "Ngày nhập",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                Width = 110,
                FillWeight = 90
            });
        }

        public void LoadData(List<SanPham> customList = null)
        {
            try
            {
                var list = customList ?? _bus.GetAll();
                _dsHienThi = new BindingList<SanPham>(list);
                dgvSanPham.DataSource = _dsHienThi;

                UpdateSummary();

                if (_dsHienThi.Count > 0)
                {
                    dgvSanPham.Rows[0].Selected = true;
                    DisplayRow(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSummary()
        {
            lblTongMatHang.Text = $"Mặt hàng: {_dsHienThi.Count}";
            int totalQty = 0;
            decimal totalVal = 0;
            foreach (var sp in _dsHienThi)
            {
                totalQty += sp.SoLuong;
                totalVal += sp.ThanhTien;
            }
            lblTongSoLuong.Text = $"Tổng tồn kho: {totalQty:N0} chiếc";
            lblTongGiaTri.Text = $"TỔNG GIÁ TRỊ HÀNG TỒN: {totalVal:N0} VNĐ";
        }

        private void DgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSanPham.Rows.Count)
            {
                DisplayRow(e.RowIndex);
            }
        }

        private void DisplayRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSanPham.Rows.Count) return;
            var sp = dgvSanPham.Rows[rowIndex].DataBoundItem as SanPham;
            if (sp != null)
            {
                txtMaSP.Text = sp.MaSP;
                txtTenSP.Text = sp.TenSP;
                cboLoaiSP.SelectedItem = sp.LoaiSP;
                numSoLuong.Value = Math.Max(numSoLuong.Minimum, Math.Min(numSoLuong.Maximum, sp.SoLuong));
                txtDonGia.Text = sp.DonGia.ToString("N0");
                dtpNgayNhap.Value = sp.NgayNhap;
                txtGhiChu.Text = sp.GhiChu;
                txtMaSP.ReadOnly = true;
            }
        }

        private void ClearForm()
        {
            txtMaSP.ReadOnly = false;
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtDonGia.Clear();
            txtGhiChu.Clear();
            numSoLuong.Value = 1;
            dtpNgayNhap.Value = DateTime.Today;
            if (cboLoaiSP.Items.Count > 0) cboLoaiSP.SelectedIndex = 0;
            txtMaSP.Focus();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string rawPrice = txtDonGia.Text.Replace(",", "").Replace(".", "").Trim();
                if (!decimal.TryParse(rawPrice, out decimal donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }

                var sp = new SanPham(
                    txtMaSP.Text.Trim(),
                    txtTenSP.Text.Trim(),
                    cboLoaiSP.SelectedItem?.ToString() ?? "Khác",
                    (int)numSoLuong.Value,
                    donGia,
                    dtpNgayNhap.Value,
                    txtGhiChu.Text.Trim()
                );

                _bus.Add(sp);
                LoadData();
                MessageBox.Show($"Thêm thành công sản phẩm: {sp.TenSP}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSP.Text))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string rawPrice = txtDonGia.Text.Replace(",", "").Replace(".", "").Trim();
                if (!decimal.TryParse(rawPrice, out decimal donGia))
                {
                    MessageBox.Show("Đơn giá không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGia.Focus();
                    return;
                }

                var sp = new SanPham(
                    txtMaSP.Text.Trim(),
                    txtTenSP.Text.Trim(),
                    cboLoaiSP.SelectedItem?.ToString() ?? "Khác",
                    (int)numSoLuong.Value,
                    donGia,
                    dtpNgayNhap.Value,
                    txtGhiChu.Text.Trim()
                );

                _bus.Update(sp);
                LoadData();
                MessageBox.Show($"Cập nhật thành công sản phẩm: {sp.MaSP}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSanPham.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần xóa trên bảng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var sp = dgvSanPham.CurrentRow.DataBoundItem as SanPham;
                if (sp == null) return;

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm '{sp.TenSP}' (Mã: {sp.MaSP})?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _bus.Delete(sp.MaSP);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Đã xóa sản phẩm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            var results = _bus.Search(keyword);
            LoadData(results);
        }

        private void CboSapXep_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboSapXep.SelectedIndex)
            {
                case 0: LoadData(_bus.Sort("masp", true)); break;
                case 1: LoadData(_bus.Sort("tensp", true)); break;
                case 2: LoadData(_bus.Sort("tensp", false)); break;
                case 3: LoadData(_bus.Sort("dongia", true)); break;
                case 4: LoadData(_bus.Sort("dongia", false)); break;
                case 5: LoadData(_bus.Sort("soluong", false)); break;
                case 6: LoadData(_bus.Sort("thanhtien", false)); break;
            }
        }
    }
}
