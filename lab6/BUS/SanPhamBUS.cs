using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Lab06_ThreeLayerMemory.DAL;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.BUS
{
    public class SanPhamBUS
    {
        private readonly SanPhamDAL _dal;

        public SanPhamBUS()
        {
            _dal = new SanPhamDAL();
        }

        public SanPhamBUS(SanPhamDAL dal)
        {
            _dal = dal ?? new SanPhamDAL();
        }

        public List<SanPham> GetAll()
        {
            return _dal.GetAll();
        }

        public BindingList<SanPham> GetBindingList()
        {
            return new BindingList<SanPham>(_dal.GetAll());
        }

        public SanPham GetById(string maSP)
        {
            return _dal.GetById(maSP);
        }

        public void Add(SanPham sp)
        {
            if (sp == null) throw new ArgumentNullException(nameof(sp));

            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(sp.TenSP))
                throw new ArgumentException("Tên sản phẩm không được để trống.");

            if (sp.SoLuong < 0)
                throw new ArgumentOutOfRangeException(nameof(sp.SoLuong), "Số lượng sản phẩm phải >= 0.");

            if (sp.DonGia < 0)
                throw new ArgumentOutOfRangeException(nameof(sp.DonGia), "Đơn giá sản phẩm phải >= 0.");

            if (_dal.GetAll().Any(x => string.Equals(x.MaSP, sp.MaSP.Trim(), StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Mã sản phẩm '{sp.MaSP}' đã tồn tại.");

            _dal.Add(sp);
        }

        public void Update(SanPham sp)
        {
            if (sp == null) throw new ArgumentNullException(nameof(sp));

            if (string.IsNullOrWhiteSpace(sp.MaSP))
                throw new ArgumentException("Mã sản phẩm không được để trống.");

            if (string.IsNullOrWhiteSpace(sp.TenSP))
                throw new ArgumentException("Tên sản phẩm không được để trống.");

            if (sp.SoLuong < 0)
                throw new ArgumentOutOfRangeException(nameof(sp.SoLuong), "Số lượng sản phẩm phải >= 0.");

            if (sp.DonGia < 0)
                throw new ArgumentOutOfRangeException(nameof(sp.DonGia), "Đơn giá sản phẩm phải >= 0.");

            bool ok = _dal.Update(sp);
            if (!ok)
                throw new InvalidOperationException($"Không tìm thấy sản phẩm có mã '{sp.MaSP}' để cập nhật.");
        }

        public void Delete(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
                throw new ArgumentException("Vui lòng chọn sản phẩm cần xóa.");

            bool ok = _dal.Delete(maSP);
            if (!ok)
                throw new InvalidOperationException($"Không tìm thấy sản phẩm có mã '{maSP}' để xóa.");
        }

        public List<SanPham> Search(string keyword)
        {
            return _dal.Search(keyword);
        }

        public List<SanPham> Sort(string criteria, bool ascending)
        {
            var list = _dal.GetAll();
            switch (criteria.ToLower())
            {
                case "tensp":
                    return ascending ? list.OrderBy(x => x.TenSP).ToList() : list.OrderByDescending(x => x.TenSP).ToList();
                case "dongia":
                    return ascending ? list.OrderBy(x => x.DonGia).ToList() : list.OrderByDescending(x => x.DonGia).ToList();
                case "soluong":
                    return ascending ? list.OrderBy(x => x.SoLuong).ToList() : list.OrderByDescending(x => x.SoLuong).ToList();
                case "thanhtien":
                    return ascending ? list.OrderBy(x => x.ThanhTien).ToList() : list.OrderByDescending(x => x.ThanhTien).ToList();
                default:
                    return ascending ? list.OrderBy(x => x.MaSP).ToList() : list.OrderByDescending(x => x.MaSP).ToList();
            }
        }

        public decimal TinhTongGiaTriTonKho()
        {
            return _dal.TinhTongGiaTriTonKho();
        }

        public int TinhTongSoLuongTonKho()
        {
            return _dal.TinhTongSoLuongTonKho();
        }

        public Dictionary<string, List<SanPham>> GetCategoryGroups()
        {
            var result = new Dictionary<string, List<SanPham>>();
            var all = _dal.GetAll();
            foreach (var sp in all)
            {
                string cat = string.IsNullOrWhiteSpace(sp.LoaiSP) ? "Khác" : sp.LoaiSP;
                if (!result.ContainsKey(cat))
                {
                    result[cat] = new List<SanPham>();
                }
                result[cat].Add(sp);
            }
            return result;
        }
    }
}
