using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab06_ThreeLayerMemory.GUI
{
    public class frmKienTruc3Lop : Form
    {
        public frmKienTruc3Lop()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Bài 10: Tổ chức kiến trúc 3 lớp (DTO - DAL - BUS - GUI)";
            this.Size = new Size(860, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            var lblTieuDe = new Label
            {
                Text = "KIẾN TRÚC 3 LỚP PHÂN TÁCH TRÁCH NHIỆM (3-TIER ARCHITECTURE)",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(25, 20),
                AutoSize = true
            };

            var grpDTO = new GroupBox
            {
                Text = "1. Tầng DTO (Data Transfer Object) - Namespace: Lab06_ThreeLayerMemory.DTO",
                Location = new Point(25, 60),
                Size = new Size(790, 85)
            };
            var lblDTO = new Label
            {
                Text = "- Chứa các lớp thực thể dữ liệu: SinhVien, SanPham, Khoa.\n- Chỉ chứa thuộc tính (Properties), Constructor và phương thức tính toán nội tại (TinhTuoi, XepLoai, ThanhTien).\n- Không chứa logic lưu trữ dữ liệu hay tương tác giao diện người dùng.",
                Location = new Point(20, 25),
                Size = new Size(750, 50)
            };
            grpDTO.Controls.Add(lblDTO);

            var grpDAL = new GroupBox
            {
                Text = "2. Tầng DAL (Data Access Layer) - Namespace: Lab06_ThreeLayerMemory.DAL",
                Location = new Point(25, 155),
                Size = new Size(790, 95)
            };
            var lblDAL = new Label
            {
                Text = "- Chứa các lớp truy cập kho dữ liệu: SinhVienDAL, SanPhamDAL.\n- Quản lý danh sách trong bộ nhớ (List<T>), cung cấp CRUD: GetAll(), Add(), Update(), Delete(), Search().\n- Hỗ trợ xuất và nhập tệp CSV (ExportToCSV, ImportFromCSV) lưu trữ cục bộ (Bài 9).\n- Chuẩn bị sẵn sàng thay thế bằng ADO.NET kết nối SQL Server ở Lab 7 mà không làm ảnh hưởng BUS.",
                Location = new Point(20, 25),
                Size = new Size(750, 60)
            };
            grpDAL.Controls.Add(lblDAL);

            var grpBUS = new GroupBox
            {
                Text = "3. Tầng BUS (Business Logic Layer) - Namespace: Lab06_ThreeLayerMemory.BUS",
                Location = new Point(25, 260),
                Size = new Size(790, 95)
            };
            var lblBUS = new Label
            {
                Text = "- Chứa các lớp xử lý nghiệp vụ: SinhVienBUS, SanPhamBUS.\n- Kiểm tra tính hợp lệ dữ liệu (Validate): kiểm tra trùng mã, kiểm tra trường bắt buộc, khoảng điểm [0, 10], đơn giá >= 0.\n- Thực hiện các nghiệp vụ: Tính tổng giá trị tồn kho, sắp xếp, phân nhóm dữ liệu cho TreeView.\n- Tầng trung gian duy nhất đứng giữa GUI và DAL, ngăn cách hoàn toàn Form với kho dữ liệu.",
                Location = new Point(20, 25),
                Size = new Size(750, 60)
            };
            grpBUS.Controls.Add(lblBUS);

            var grpGUI = new GroupBox
            {
                Text = "4. Tầng GUI (Presentation Layer) - Namespace: Lab06_ThreeLayerMemory.GUI",
                Location = new Point(25, 365),
                Size = new Size(790, 100)
            };
            var lblGUI = new Label
            {
                Text = "- Các Form giao diện WinForms: frmMain, frmSinhVienDGV, frmSinhVienListView, frmKhoaSinhVienTree, frmSanPhamDGV, frmDanhMucSanPhamTree.\n- Form CHỈ khởi tạo và gọi đối tượng BUS (SinhVienBUS, SanPhamBUS).\n- Form TUYỆT ĐỐI KHÔNG chứa mã truy cập trực tiếp danh sách hay logic nghiệp vụ kiểm tra kho dữ liệu.",
                Location = new Point(20, 25),
                Size = new Size(750, 65)
            };
            grpGUI.Controls.Add(lblGUI);

            var btnDong = new Button
            {
                Text = "Đóng",
                Location = new Point(715, 480),
                Size = new Size(100, 35)
            };
            btnDong.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTieuDe, grpDTO, grpDAL, grpBUS, grpGUI, btnDong });
        }
    }
}
