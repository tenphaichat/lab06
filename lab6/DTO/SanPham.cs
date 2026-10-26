using System;

namespace Lab06_ThreeLayerMemory.DTO
{
    public class SanPham
    {
        private string _maSP = string.Empty;
        private string _tenSP = string.Empty;
        private string _loaiSP = "Laptop";
        private int _soLuong = 1;
        private decimal _donGia = 0;
        private DateTime _ngayNhap = DateTime.Today;
        private string _ghiChu = string.Empty;

        public string MaSP
        {
            get => _maSP;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mã sản phẩm không được để trống.");
                _maSP = value.Trim().ToUpper();
            }
        }

        public string TenSP
        {
            get => _tenSP;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên sản phẩm không được để trống.");
                _tenSP = value.Trim();
            }
        }

        public string LoaiSP
        {
            get => _loaiSP;
            set => _loaiSP = string.IsNullOrWhiteSpace(value) ? "Khác" : value.Trim();
        }

        public int SoLuong
        {
            get => _soLuong;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Số lượng phải >= 0.");
                _soLuong = value;
            }
        }

        public decimal DonGia
        {
            get => _donGia;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Đơn giá phải >= 0.");
                _donGia = value;
            }
        }

        public decimal ThanhTien => SoLuong * DonGia;

        public DateTime NgayNhap
        {
            get => _ngayNhap;
            set => _ngayNhap = value;
        }

        public string GhiChu
        {
            get => _ghiChu;
            set => _ghiChu = value;
        }

        public SanPham() { }

        public SanPham(string maSP, string tenSP, string loaiSP, int soLuong, decimal donGia, DateTime? ngayNhap = null, string ghiChu = "")
        {
            MaSP = maSP;
            TenSP = tenSP;
            LoaiSP = loaiSP;
            SoLuong = soLuong;
            DonGia = donGia;
            NgayNhap = ngayNhap ?? DateTime.Today;
            GhiChu = ghiChu;
        }

        public override string ToString()
        {
            return $"{TenSP} ({DonGia:N0} đ)";
        }
    }
}
