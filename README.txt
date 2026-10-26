1. HƯỚNG DẪN BIÊN DỊCH VÀ CHẠY
- Yêu cầu môi trường: Visual Studio 2022 hoặc .NET SDK (>= .NET 8 / .NET 9 hỗ trợ Windows Forms Desktop).
- Mở bằng Visual Studio:
  + Mở file solution: IT3212_Labs.sln
  + Nhấn F5 hoặc Ctrl+F5 để build và khởi chạy ứng dụng Windows Forms.
- Chạy bằng dòng lệnh (.NET CLI):
  + Di chuyển vào thư mục lab6:
      cd lab6
  + Chạy lệnh:
      dotnet run --project lab6/Lab06_ThreeLayerMemory.csproj

2. CẤU TRÚC VÀ TỔ CHỨC MÃ NGUỒN (KIẾN TRÚC 3 LỚP)
Dự án được phân chia độc lập và rõ ràng theo chuẩn 3-Tier Architecture trong bộ nhớ (In-Memory), phong cách giao diện WinForms basic chuẩn hệ điều hành:
- Solution: IT3212_Labs.sln
- Project: lab6/Lab06_ThreeLayerMemory.csproj
- Tầng DTO (Data Transfer Object) - Namespace: Lab06_ThreeLayerMemory.DTO:
  + SinhVien.cs: Lớp thực thể sinh viên (MaSV, HoTen, NgaySinh, DiemTB, GioiTinh, Khoa, SoThich, DuongDanAnh, Tuoi, XepLoai).
  + SanPham.cs: Lớp thực thể sản phẩm (MaSP, TenSP, LoaiSP, SoLuong, DonGia, ThanhTien, NgayNhap, GhiChu).
  + Khoa.cs: Lớp thực thể khoa đào tạo (MaKhoa, TenKhoa).
- Tầng DAL (Data Access Layer) - Namespace: Lab06_ThreeLayerMemory.DAL:
  + SinhVienDAL.cs: Quản lý danh sách List<SinhVien> trong bộ nhớ (seed sẵn 3 khoa, mỗi khoa 3 sinh viên); các hàm GetAll(), Add(), Update(), Delete(), Search(), ExportToCSV(), ImportFromCSV().
  + SanPhamDAL.cs: Quản lý danh sách List<SanPham> trong bộ nhớ; CRUD, tìm kiếm, tính tổng số lượng và tổng giá trị tồn kho.
- Tầng BUS (Business Logic Layer) - Namespace: Lab06_ThreeLayerMemory.BUS:
  + SinhVienBUS.cs: Tầng trung gian kiểm tra tính toàn vẹn và hợp lệ của dữ liệu (Validate rỗng, chặn trùng mã sinh viên, kiểm tra miền điểm [0, 10]), phân nhóm khoa cho TreeView, xuất nhập CSV.
  + SanPhamBUS.cs: Kiểm tra nghiệp vụ sản phẩm (chặn trùng mã SP, số lượng >= 0, đơn giá >= 0), phân nhóm danh mục, tìm kiếm, sắp xếp đa tiêu chí và tính tổng giá trị hàng tồn.
- Tầng GUI (Presentation Layer) - Namespace: Lab06_ThreeLayerMemory.GUI:
  + frmMain.cs: Dashboard trung tâm MDI Container quản lý MenuStrip, ToolStrip, StatusStrip, đồng hồ thời gian thực và quản lý mở các form con.
  + frmSinhVienDGV.cs: Quản lý sinh viên trên DataGridView (Bài 1, 2, 5, 9).
  + frmSinhVienListView.cs: Danh sách sinh viên bằng ListView dạng Details và ImageList xếp loại học lực (Bài 3, 8).
  + frmKhoaSinhVienTree.cs: Cây phân cấp Khoa - Sinh viên bằng TreeView (Bài 4).
  + frmSanPhamDGV.cs: Quản lý sản phẩm trên DataGridView (Tìm kiếm, Sắp xếp, Tổng tồn kho) (Bài 6).
  + frmDanhMucSanPhamTree.cs: TreeView quản lý Danh mục - Sản phẩm kết hợp ContextMenuStrip (Thêm, Đổi tên, Xóa node) (Bài 7).
  + frmKienTruc3Lop.cs: Giới thiệu và minh họa kiến trúc 3 lớp phân tách trách nhiệm (Bài 10).
  + frmGioiThieu.cs: Hộp thoại giới thiệu thông tin tác giả và học phần.
- Thư mục Images/: Chứa ảnh đại diện sinh viên và tài nguyên ứng dụng.

3. DANH SÁCH BÀI TẬP ĐÃ HOÀN THÀNH (10/10 BÀI)
* PHẦN 1: BÀI TẬP CÓ HƯỚNG DẪN (BÀI 1 - 5)
- Bài 1: Hiển thị BindingList trên DataGridView: Tạo BindingList<SinhVien>, liên kết DataGridView, cấu hình tiêu đề tiếng Việt chuẩn, bật FullRowSelect, không sinh cột tự động lộn xộn.
- Bài 2: CRUD trong bộ nhớ: Thao tác Thêm, Sửa, Xóa sinh viên trong BindingList; kiểm tra trùng mã trước khi thêm, đồng bộ giao diện khi chọn dòng trên bảng.
- Bài 3: Danh sách chi tiết bằng ListView: Thiết lập View = View.Details; hiển thị Mã, Họ tên, Khoa, Điểm TB, Xếp loại; lưu đối tượng vào thuộc tính Tag của ListViewItem để xem hồ sơ chi tiết.
- Bài 4: Cây Khoa - Sinh viên bằng TreeView: Tạo node gốc theo Khoa, node con theo sinh viên; gán Tag = sv; khi chọn node con hiển thị hồ sơ chi tiết và ảnh đại diện bên phải.
- Bài 5: Tách mô hình GUI - BUS - DAL giả lập: SinhVienDAL lưu dữ liệu, SinhVienBUS kiểm tra nghiệp vụ; Form GUI chỉ gọi BUS, hoàn toàn không thao tác trực tiếp vào kho dữ liệu.

* PHẦN 2: BÀI TẬP TỰ THỰC HIỆN (BÀI 6 - 10)
- Bài 6: Quản lý sản phẩm bằng DataGridView: CRUD sản phẩm trong bộ nhớ; tìm kiếm theo mã hoặc tên; sắp xếp đa tiêu chí (tên, giá, số lượng, thành tiền); tính tổng giá trị hàng tồn kho (154.800.000 VNĐ).
- Bài 7: TreeView quản lý Danh mục - Sản phẩm: Cây 2 cấp Danh mục -> Sản phẩm; gắn ContextMenuStrip hỗ trợ Thêm danh mục mới, Thêm sản phẩm vào danh mục, Đổi tên node (F2), Xóa node có xác nhận.
- Bài 8: ListView có biểu tượng dùng ImageList: Xây dựng ImageList biểu tượng xếp loại học lực sinh viên (Xuất sắc, Giỏi, Khá, Trung bình, Yếu); hỗ trợ đổi View linh hoạt (Details, LargeIcon, SmallIcon, Tile).
- Bài 9: Xuất và nhập dữ liệu ra tệp CSV: Cài đặt trực tiếp tại tầng DAL giả lập; lưu danh sách sinh viên ra tệp CSV chuẩn UTF-8 và đọc lại dữ liệu khi người dùng yêu cầu.
- Bài 10: Hoàn thiện kiến trúc 3 lớp: Phân chia rõ ràng các thư mục và namespace DTO, DAL, BUS, GUI; Form không chứa logic lưu trữ dữ liệu, sẵn sàng chuyển đổi kết nối SQL Server ở Lab 7.

4. DỮ LIỆU KIỂM THỬ MẪU (TEST CASES)
- Thông tin sinh viên mẫu: Phan Văn Thắng - MSSV: 24002545 - Ngày sinh: 26/03/2006 - Điểm: 8.50 - Khoa: Công nghệ thông tin.
- Test Bài 1: Mở frmSinhVienDGV -> Bảng DataGridView hiển thị danh sách sinh viên với 7 cột tiêu đề rõ ràng, chế độ chọn trọn dòng.
- Test Bài 2: Nhập mã trùng "24002545" -> Bấm Thêm -> Hệ thống cảnh báo "Mã sinh viên '24002545' đã tồn tại trong hệ thống." Nhập mã mới "SV11", Nguyễn Đức Kiên, Điểm 8.0 -> Thêm thành công, bảng cập nhật ngay lập tức.
- Test Bài 3: Mở frmSinhVienListView -> Chế độ View.Details hiển thị mã, họ tên, khoa, điểm, xếp loại; nhấp chọn sinh viên bất kỳ -> Panel bên phải hiển thị đầy đủ thông tin trích xuất từ item.Tag.
- Test Bài 4: Mở frmKhoaSinhVienTree -> Cây mở rộng 3 khoa (CNTT, Kinh tế, Ngoại ngữ); nhấp chọn sinh viên Phan Văn Thắng -> Panel hồ sơ hiển thị đầy đủ thông tin cá nhân và ảnh đại diện rõ nét.
- Test Bài 5: Tách biệt kiến trúc: Kiểm tra mã nguồn Form frmSinhVienDGV không hề chứa biến List<SinhVien> hay thao tác thêm vào list, toàn bộ đều thông qua _bus.Add() / _bus.Update() / _bus.Delete().
- Test Bài 6: Mở frmSanPhamDGV -> Bảng sản phẩm hiển thị các mặt hàng Laptop, Bàn phím, Chuột, Màn hình, Âm thanh; Thanh thống kê hiển thị chính xác "Mặt hàng: 11 | Tổng tồn kho: 100 chiếc | TỔNG GIÁ TRỊ HÀNG TỒN: 154,800,000 VNĐ". Tìm từ khóa "ROG" -> Hiển thị đúng Laptop Asus ROG. Chọn sắp xếp "Đơn giá giảm dần" -> Các sản phẩm giá cao lên đầu.
- Test Bài 7: Mở frmDanhMucSanPhamTree -> Cây hiển thị các danh mục. Nhấp chuột phải vào node danh mục -> ContextMenuStrip hiện lên -> Chọn "Thêm sản phẩm vào danh mục này" -> Nhập tên sản phẩm -> Node sản phẩm mới được tạo ngay dưới danh mục. Chọn "Đổi tên node" -> Đổi tên thành công. Chọn "Xóa node" -> Hỏi xác nhận và xóa an toàn.
- Test Bài 8: Tại frmSinhVienListView -> Chọn chế độ xem "LargeIcon" hoặc "Tile" -> Các icon màu sắc theo xếp loại học lực hiển thị nổi bật bên cạnh tên sinh viên.
- Test Bài 9: Tại frmSinhVienDGV -> Bấm nút "Xuất CSV (Bài 9)" -> Lưu thành công file DanhSachSinhVien.csv với định dạng chuẩn. Bấm "Nhập CSV (Bài 9)" -> Tầng DAL đọc tệp và tải dữ liệu lên giao diện hoàn hảo.
- Test Bài 10: Xem cấu trúc solution: DTO, DAL, BUS, GUI được tách riêng biệt; mở frmKienTruc3Lop -> Xem sơ đồ và giải thích chi tiết cơ chế phân tách trách nhiệm.
