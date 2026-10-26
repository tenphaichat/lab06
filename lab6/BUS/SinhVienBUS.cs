using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Lab06_ThreeLayerMemory.DAL;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.BUS
{
    public class SinhVienBUS
    {
        private readonly SinhVienDAL _dal;

        public SinhVienBUS()
        {
            _dal = new SinhVienDAL();
        }

        public SinhVienBUS(SinhVienDAL dal)
        {
            _dal = dal ?? new SinhVienDAL();
        }

        public List<SinhVien> GetAll()
        {
            return _dal.GetAll();
        }

        public BindingList<SinhVien> GetBindingList()
        {
            return new BindingList<SinhVien>(_dal.GetAll());
        }

        public SinhVien GetById(string maSV)
        {
            return _dal.GetById(maSV);
        }

        public void Add(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            if (string.IsNullOrWhiteSpace(sv.MaSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(sv.HoTen))
                throw new ArgumentException("Họ và tên không được để trống.");

            if (sv.DiemTB < 0 || sv.DiemTB > 10)
                throw new ArgumentOutOfRangeException(nameof(sv.DiemTB), "Điểm trung bình phải nằm trong khoảng [0, 10].");

            if (_dal.GetAll().Any(x => string.Equals(x.MaSV, sv.MaSV.Trim(), StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Mã sinh viên '{sv.MaSV}' đã tồn tại trong hệ thống.");

            _dal.Add(sv);
        }

        public void Update(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));

            if (string.IsNullOrWhiteSpace(sv.MaSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(sv.HoTen))
                throw new ArgumentException("Họ và tên không được để trống.");

            if (sv.DiemTB < 0 || sv.DiemTB > 10)
                throw new ArgumentOutOfRangeException(nameof(sv.DiemTB), "Điểm trung bình phải nằm trong khoảng [0, 10].");

            bool ok = _dal.Update(sv);
            if (!ok)
                throw new InvalidOperationException($"Không tìm thấy sinh viên có mã '{sv.MaSV}' để cập nhật.");
        }

        public void Delete(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException("Vui lòng chọn mã sinh viên cần xóa.");

            bool ok = _dal.Delete(maSV);
            if (!ok)
                throw new InvalidOperationException($"Không tìm thấy sinh viên có mã '{maSV}' để xóa.");
        }

        public List<SinhVien> Search(string keyword)
        {
            return _dal.Search(keyword);
        }

        public Dictionary<string, List<SinhVien>> GetFacultyGroups()
        {
            var result = new Dictionary<string, List<SinhVien>>();
            var all = _dal.GetAll();
            foreach (var sv in all)
            {
                string khoa = string.IsNullOrWhiteSpace(sv.Khoa) ? "Chưa phân khoa" : sv.Khoa;
                if (!result.ContainsKey(khoa))
                {
                    result[khoa] = new List<SinhVien>();
                }
                result[khoa].Add(sv);
            }
            return result;
        }

        public void ExportToCSV(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn lưu tệp không hợp lệ.");
            _dal.ExportToCSV(filePath);
        }

        public List<SinhVien> ImportFromCSV(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Đường dẫn tệp CSV không hợp lệ.");
            return _dal.ImportFromCSV(filePath);
        }
    }
}
