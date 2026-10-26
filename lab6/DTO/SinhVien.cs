using System;

namespace Lab06_ThreeLayerMemory.DTO
{
    public class SinhVien
    {
        private string _maSV = string.Empty;
        private string _hoTen = string.Empty;
        private DateTime _ngaySinh = new DateTime(2006, 3, 26);
        private double _diemTB = 8.5;
        private string _gioiTinh = "Nam";
        private string _khoa = "Công nghệ thông tin";
        private string _soThich = "Lập trình";
        private string _duongDanAnh = string.Empty;

        public string MaSV
        {
            get => _maSV;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sinh viên không được để trống.");
                _maSV = value.Trim().ToUpper();
            }
        }

        public string HoTen
        {
            get => _hoTen;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Họ và tên không được để trống.");
                _hoTen = value.Trim();
            }
        }

        public DateTime NgaySinh
        {
            get => _ngaySinh;
            set => _ngaySinh = value;
        }

        public double DiemTB
        {
            get => _diemTB;
            set
            {
                if (value < 0 || value > 10)
                    throw new ArgumentOutOfRangeException(nameof(value), "Điểm trung bình phải từ 0 đến 10.");
                _diemTB = value;
            }
        }

        public string GioiTinh
        {
            get => _gioiTinh;
            set => _gioiTinh = value;
        }

        public string Khoa
        {
            get => _khoa;
            set => _khoa = value;
        }

        public string SoThich
        {
            get => _soThich;
            set => _soThich = value;
        }

        public string DuongDanAnh
        {
            get => _duongDanAnh;
            set => _duongDanAnh = value;
        }

        // Computed properties for easy DataGridView display
        public string XepLoai
        {
            get
            {
                if (DiemTB >= 9.0) return "Xuất sắc";
                if (DiemTB >= 8.0) return "Giỏi";
                if (DiemTB >= 6.5) return "Khá";
                if (DiemTB >= 5.0) return "Trung bình";
                return "Yếu";
            }
        }

        public int Tuoi
        {
            get
            {
                int tuoi = DateTime.Today.Year - NgaySinh.Year;
                if (NgaySinh.Date > DateTime.Today.AddYears(-tuoi)) tuoi--;
                return tuoi < 0 ? 0 : tuoi;
            }
        }

        public SinhVien()
        {
            _maSV = "24002545";
            _hoTen = "Phan Văn Thắng";
            _ngaySinh = new DateTime(2006, 3, 26);
            _diemTB = 8.5;
            _gioiTinh = "Nam";
            _khoa = "Công nghệ thông tin";
            _soThich = "Lập trình";
            _duongDanAnh = "";
        }

        public SinhVien(string maSV, string hoTen, DateTime ngaySinh, double diemTB,
                        string gioiTinh = "Nam", string khoa = "Công nghệ thông tin",
                        string soThich = "", string anh = "")
        {
            MaSV = maSV;
            HoTen = hoTen;
            NgaySinh = ngaySinh;
            DiemTB = diemTB;
            GioiTinh = gioiTinh;
            Khoa = khoa;
            SoThich = soThich;
            DuongDanAnh = anh;
        }

        public override string ToString()
        {
            return $"{MaSV} - {HoTen}";
        }
    }
}
