using System;
using System.Collections.Generic;
using System.Linq;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.DAL
{
    public class SanPhamDAL
    {
        private readonly List<SanPham> _data = new List<SanPham>();

        public SanPhamDAL()
        {
            SeedData();
        }

        private void SeedData()
        {
            _data.Clear();
            _data.Add(new SanPham("SP01", "Laptop Asus ROG Strix G16", "Laptop", 5, 32000000m, new DateTime(2026, 1, 15), "Intel i7 13650HX, RTX 4060"));
            _data.Add(new SanPham("SP02", "Laptop Dell XPS 13 Plus", "Laptop", 4, 28500000m, new DateTime(2026, 2, 10), "Intel i7 1360P, 16GB, 512GB"));
            _data.Add(new SanPham("SP03", "MacBook Air M2 13.6 inch", "Laptop", 6, 24000000m, new DateTime(2026, 2, 20), "Apple M2, 8GB, 256GB SSD"));
            _data.Add(new SanPham("SP04", "Bàn phím cơ AKKO 3087 V2", "Bàn phím", 15, 1250000m, new DateTime(2026, 3, 5), "Switch Akko Orange, Type-C"));
            _data.Add(new SanPham("SP05", "Bàn phím không dây Logitech MX Keys", "Bàn phím", 8, 2800000m, new DateTime(2026, 3, 12), "Kết nối 3 thiết bị Bluetooth"));
            _data.Add(new SanPham("SP06", "Chuột Razer DeathAdder V2", "Chuột", 20, 850000m, new DateTime(2026, 3, 18), "Cảm biến quang học 20K DPI"));
            _data.Add(new SanPham("SP07", "Chuột Logitech MX Master 3S", "Chuột", 12, 2200000m, new DateTime(2026, 3, 22), "Cuộn MagSpeed êm ái"));
            _data.Add(new SanPham("SP08", "Màn hình Dell UltraSharp U2422H", "Màn hình", 10, 6200000m, new DateTime(2026, 4, 2), "24 inch FHD IPS 100% sRGB"));
            _data.Add(new SanPham("SP09", "Màn hình Gaming LG 27GP850", "Màn hình", 7, 7900000m, new DateTime(2026, 4, 15), "27 inch 2K Nano IPS 165Hz"));
            _data.Add(new SanPham("SP10", "Tai nghe Sony WH-1000XM4", "Âm thanh", 6, 5500000m, new DateTime(2026, 5, 1), "Chống ồn chủ động cao cấp"));
            _data.Add(new SanPham("SP11", "Loa Bluetooth Marshall Emberton", "Âm thanh", 9, 3800000m, new DateTime(2026, 5, 10), "Chuẩn chống nước IPX7, 20 giờ pin"));
        }

        public List<SanPham> GetAll()
        {
            return new List<SanPham>(_data);
        }

        public SanPham GetById(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP)) return null;
            return _data.FirstOrDefault(x => string.Equals(x.MaSP, maSP.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(SanPham sp)
        {
            if (sp == null) throw new ArgumentNullException(nameof(sp));
            _data.Add(sp);
        }

        public bool Update(SanPham sp)
        {
            if (sp == null) throw new ArgumentNullException(nameof(sp));
            var existing = GetById(sp.MaSP);
            if (existing == null) return false;

            existing.TenSP = sp.TenSP;
            existing.LoaiSP = sp.LoaiSP;
            existing.SoLuong = sp.SoLuong;
            existing.DonGia = sp.DonGia;
            existing.NgayNhap = sp.NgayNhap;
            existing.GhiChu = sp.GhiChu;
            return true;
        }

        public bool Delete(string maSP)
        {
            var sp = GetById(maSP);
            if (sp != null)
            {
                return _data.Remove(sp);
            }
            return false;
        }

        public List<SanPham> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            string k = keyword.Trim().ToLower();
            return _data.Where(x => x.MaSP.ToLower().Contains(k) ||
                                    x.TenSP.ToLower().Contains(k) ||
                                    x.LoaiSP.ToLower().Contains(k)).ToList();
        }

        public decimal TinhTongGiaTriTonKho()
        {
            return _data.Sum(x => x.ThanhTien);
        }

        public int TinhTongSoLuongTonKho()
        {
            return _data.Sum(x => x.SoLuong);
        }
    }
}
