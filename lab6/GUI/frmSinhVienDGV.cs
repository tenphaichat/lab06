using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Lab06_ThreeLayerMemory.BUS;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmSinhVienDGV : Form
    {
        // Bài 5 & 10: Form chỉ gọi tầng BUS, không truy cập trực tiếp DAL
        private readonly SinhVienBUS _bus;
        private BindingList<SinhVien> _dsHienThi;

        private TextBox txtMaSV;
        private TextBox txtHoTen;
        private DateTimePicker dtpNgaySinh;
        private ComboBox cboGioiTinh;
        private ComboBox cboKhoa;
        private TextBox txtDiemTB;
        private TextBox txtSoThich;

        private Button btnThem;
        private Button btnSua;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnXuatCSV;
        private Button btnNhapCSV;

        private DataGridView dgvSinhVien;
        private Label lblTongSo;

        public frmSinhVienDGV()
        {
            _bus = new SinhVienBUS();
            InitializeComponent();
        }

        public frmSinhVienDGV(SinhVienBUS bus)
        {
            _bus = bus ?? new SinhVienBUS();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 1, 2, 5, 9: Quản lý Sinh viên trên DataGridView (Mô hình 3 lớp)";
            this.Size = new Size(1000, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // GroupBox Nhập liệu
            var grpInput = new GroupBox
            {
                Text = "Thông tin chi tiết sinh viên",
                Location = new Point(20, 15),
                Size = new Size(940, 175)
            };

            // Hàng 1: Mã SV, Họ tên, Điểm TB
            var lblMaSV = new Label { Text = "Mã sinh viên:", Location = new Point(25, 30), Size = new Size(100, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtMaSV = new TextBox { Location = new Point(130, 30), Size = new Size(170, 25) };

            var lblHoTen = new Label { Text = "Họ và tên:", Location = new Point(330, 30), Size = new Size(90, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtHoTen = new TextBox { Location = new Point(425, 30), Size = new Size(220, 25) };

            var lblDiemTB = new Label { Text = "Điểm TB:", Location = new Point(675, 30), Size = new Size(80, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtDiemTB = new TextBox { Location = new Point(760, 30), Size = new Size(150, 25) };

            // Hàng 2: Ngày sinh, Giới tính, Khoa
            var lblNgaySinh = new Label { Text = "Ngày sinh:", Location = new Point(25, 70), Size = new Size(100, 25), TextAlign = ContentAlignment.MiddleLeft };
            dtpNgaySinh = new DateTimePicker { Location = new Point(130, 70), Size = new Size(170, 25), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" };

            var lblGioiTinh = new Label { Text = "Giới tính:", Location = new Point(330, 70), Size = new Size(90, 25), TextAlign = ContentAlignment.MiddleLeft };
            cboGioiTinh = new ComboBox { Location = new Point(425, 70), Size = new Size(220, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
            cboGioiTinh.SelectedIndex = 0;

            var lblKhoa = new Label { Text = "Khoa đào tạo:", Location = new Point(675, 70), Size = new Size(80, 25), TextAlign = ContentAlignment.MiddleLeft };
            cboKhoa = new ComboBox { Location = new Point(760, 70), Size = new Size(150, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cboKhoa.Items.AddRange(new object[] { "Công nghệ thông tin", "Kinh tế", "Ngoại ngữ", "Cơ khí", "Điện tử" });
            cboKhoa.SelectedIndex = 0;

            // Hàng 3: Sở thích
            var lblSoThich = new Label { Text = "Sở thích:", Location = new Point(25, 110), Size = new Size(100, 25), TextAlign = ContentAlignment.MiddleLeft };
            txtSoThich = new TextBox { Location = new Point(130, 110), Size = new Size(515, 25) };

            grpInput.Controls.AddRange(new Control[] {
                lblMaSV, txtMaSV,
                lblHoTen, txtHoTen,
                lblDiemTB, txtDiemTB,
                lblNgaySinh, dtpNgaySinh,
                lblGioiTinh, cboGioiTinh,
                lblKhoa, cboKhoa,
                lblSoThich, txtSoThich
            });

            // GroupBox Nút chức năng
            var pnlButtons = new Panel
            {
                Location = new Point(20, 195),
                Size = new Size(940, 45)
            };

            btnThem = new Button { Text = "Thêm mới", Location = new Point(0, 5), Size = new Size(100, 32) };
            btnSua = new Button { Text = "Cập nhật", Location = new Point(110, 5), Size = new Size(100, 32) };
            btnXoa = new Button { Text = "Xóa bỏ", Location = new Point(220, 5), Size = new Size(100, 32) };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(330, 5), Size = new Size(100, 32) };
            btnXuatCSV = new Button { Text = "Xuất CSV (Bài 9)", Location = new Point(690, 5), Size = new Size(120, 32) };
            btnNhapCSV = new Button { Text = "Nhập CSV (Bài 9)", Location = new Point(820, 5), Size = new Size(120, 32) };

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += (s, e) => ClearForm();
            btnXuatCSV.Click += BtnXuatCSV_Click;
            btnNhapCSV.Click += BtnNhapCSV_Click;

            pnlButtons.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnLamMoi, btnXuatCSV, btnNhapCSV });

            // DataGridView
            dgvSinhVien = new DataGridView
            {
                Location = new Point(20, 250),
                Size = new Size(940, 345),
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
            dgvSinhVien.CellClick += DgvSinhVien_CellClick;

            lblTongSo = new Label
            {
                Text = "Tổng số sinh viên: 0",
                Location = new Point(20, 605),
                Size = new Size(400, 25),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            this.Controls.AddRange(new Control[] { grpInput, pnlButtons, dgvSinhVien, lblTongSo });
            this.Load += FrmSinhVienDGV_Load;
        }

        private void FrmSinhVienDGV_Load(object sender, EventArgs e)
        {
            SetupDataGridViewColumns();
            LoadData();
        }

        // Bài 1: Cấu hình DataGridView và BindingList<SinhVien>
        private void SetupDataGridViewColumns()
        {
            dgvSinhVien.AutoGenerateColumns = false;
            dgvSinhVien.Columns.Clear();

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaSV",
                HeaderText = "Mã SV",
                Width = 100,
                FillWeight = 80
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoTen",
                HeaderText = "Họ và tên",
                Width = 200,
                FillWeight = 160
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgaySinh",
                HeaderText = "Ngày sinh",
                DefaultCellStyle = { Format = "dd/MM/yyyy" },
                Width = 110,
                FillWeight = 90
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GioiTinh",
                HeaderText = "Giới tính",
                Width = 90,
                FillWeight = 70
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Khoa",
                HeaderText = "Khoa đào tạo",
                Width = 180,
                FillWeight = 150
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DiemTB",
                HeaderText = "Điểm TB",
                DefaultCellStyle = { Format = "N2" },
                Width = 90,
                FillWeight = 80
            });

            dgvSinhVien.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "XepLoai",
                HeaderText = "Xếp loại",
                Width = 110,
                FillWeight = 90
            });
        }

        public void LoadData()
        {
            try
            {
                // Bài 5: Form gọi qua BUS
                var list = _bus.GetAll();
                _dsHienThi = new BindingList<SinhVien>(list);
                dgvSinhVien.DataSource = _dsHienThi;
                lblTongSo.Text = $"Tổng số sinh viên: {_dsHienThi.Count}";

                if (_dsHienThi.Count > 0)
                {
                    dgvSinhVien.Rows[0].Selected = true;
                    DisplayRow(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvSinhVien.Rows.Count)
            {
                DisplayRow(e.RowIndex);
            }
        }

        private void DisplayRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvSinhVien.Rows.Count) return;
            var sv = dgvSinhVien.Rows[rowIndex].DataBoundItem as SinhVien;
            if (sv != null)
            {
                txtMaSV.Text = sv.MaSV;
                txtHoTen.Text = sv.HoTen;
                dtpNgaySinh.Value = sv.NgaySinh;
                txtDiemTB.Text = sv.DiemTB.ToString("0.00");
                cboGioiTinh.SelectedItem = sv.GioiTinh;
                cboKhoa.SelectedItem = sv.Khoa;
                txtSoThich.Text = sv.SoThich;
                txtMaSV.ReadOnly = true; // Khóa Mã khi đang chọn dòng để sửa
            }
        }

        private void ClearForm()
        {
            txtMaSV.ReadOnly = false;
            txtMaSV.Clear();
            txtHoTen.Clear();
            txtDiemTB.Clear();
            txtSoThich.Clear();
            dtpNgaySinh.Value = new DateTime(2006, 1, 1);
            if (cboGioiTinh.Items.Count > 0) cboGioiTinh.SelectedIndex = 0;
            if (cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
            txtMaSV.Focus();
        }

        // Bài 2: CRUD trên BindingList qua BUS
        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!double.TryParse(txtDiemTB.Text.Trim(), out double diem))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiemTB.Focus();
                    return;
                }

                var sv = new SinhVien(
                    txtMaSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    dtpNgaySinh.Value,
                    diem,
                    cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                    cboKhoa.SelectedItem?.ToString() ?? "Công nghệ thông tin",
                    txtSoThich.Text.Trim()
                );

                // Gọi BUS xử lý kiểm tra trùng mã và thêm
                _bus.Add(sv);
                LoadData();
                MessageBox.Show($"Thêm thành công sinh viên: {sv.HoTen}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaSV.Text))
                {
                    MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ bảng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!double.TryParse(txtDiemTB.Text.Trim(), out double diem))
                {
                    MessageBox.Show("Điểm trung bình không hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDiemTB.Focus();
                    return;
                }

                var sv = new SinhVien(
                    txtMaSV.Text.Trim(),
                    txtHoTen.Text.Trim(),
                    dtpNgaySinh.Value,
                    diem,
                    cboGioiTinh.SelectedItem?.ToString() ?? "Nam",
                    cboKhoa.SelectedItem?.ToString() ?? "Công nghệ thông tin",
                    txtSoThich.Text.Trim()
                );

                _bus.Update(sv);
                LoadData();
                MessageBox.Show($"Cập nhật thành công sinh viên: {sv.MaSV}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSinhVien.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn dòng cần xóa trên DataGridView.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var sv = dgvSinhVien.CurrentRow.DataBoundItem as SinhVien;
                if (sv == null) return;

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên '{sv.HoTen}' (Mã: {sv.MaSV})?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _bus.Delete(sv.MaSV);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Đã xóa sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Bài 9: Tầng DAL xuất và nhập tệp CSV
        private void BtnXuatCSV_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                    sfd.FileName = "DanhSachSinhVien.csv";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        _bus.ExportToCSV(sfd.FileName);
                        MessageBox.Show($"Xuất dữ liệu thành công ra tệp:\n{sfd.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNhapCSV_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        _bus.ImportFromCSV(ofd.FileName);
                        LoadData();
                        MessageBox.Show($"Nhập dữ liệu thành công từ tệp:\n{ofd.FileName}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhập file CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
