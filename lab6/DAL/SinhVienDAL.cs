using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Lab06_ThreeLayerMemory.DTO;

namespace Lab06_ThreeLayerMemory.DAL
{
    public class SinhVienDAL
    {
        private readonly List<SinhVien> _data = new List<SinhVien>();

        public SinhVienDAL()
        {
            SeedData();
        }

        private void SeedData()
        {
            _data.Clear();

            // Khoa Công nghệ thông tin (>= 3 sinh viên)
            _data.Add(new SinhVien("24002545", "Phan Văn Thắng", new DateTime(2006, 3, 26), 8.5, "Nam", "Công nghệ thông tin", "Lập trình .NET, AI", "avatar_thang.png"));
            _data.Add(new SinhVien("SV01", "Nguyễn Văn An", new DateTime(2005, 5, 12), 8.2, "Nam", "Công nghệ thông tin", "An toàn thông tin", ""));
            _data.Add(new SinhVien("SV02", "Trần Quốc Bình", new DateTime(2005, 8, 20), 7.0, "Nam", "Công nghệ thông tin", "Thiết kế Web", "avatar_binh.png"));
            _data.Add(new SinhVien("SV03", "Lê Thị Mai Chi", new DateTime(2006, 1, 15), 9.2, "Nữ", "Công nghệ thông tin", "Khoa học dữ liệu", ""));

            // Khoa Kinh tế (>= 3 sinh viên)
            _data.Add(new SinhVien("SV04", "Phạm Hải Đăng", new DateTime(2005, 11, 4), 8.0, "Nam", "Kinh tế", "Phân tích tài chính", ""));
            _data.Add(new SinhVien("SV05", "Hoàng Hương Giang", new DateTime(2006, 7, 19), 7.8, "Nữ", "Kinh tế", "Kinh doanh quốc tế", ""));
            _data.Add(new SinhVien("SV06", "Đặng Minh Hoàng", new DateTime(2005, 3, 30), 6.5, "Nam", "Kinh tế", "Marketing", ""));

            // Khoa Ngoại ngữ (>= 3 sinh viên)
            _data.Add(new SinhVien("SV07", "Đỗ Thùy Linh", new DateTime(2006, 9, 8), 8.8, "Nữ", "Ngoại ngữ", "Tiếng Anh thương mại", ""));
            _data.Add(new SinhVien("SV08", "Vũ Quang Minh", new DateTime(2005, 12, 22), 5.5, "Nam", "Ngoại ngữ", "Biên phiên dịch", ""));
            _data.Add(new SinhVien("SV09", "Ngô Khánh Huyền", new DateTime(2006, 4, 10), 9.0, "Nữ", "Ngoại ngữ", "Tiếng Trung giao tiếp", ""));
        }

        public List<SinhVien> GetAll()
        {
            return new List<SinhVien>(_data);
        }

        public SinhVien GetById(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV)) return null;
            return _data.FirstOrDefault(x => string.Equals(x.MaSV, maSV.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public void Add(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));
            _data.Add(sv);
        }

        public bool Update(SinhVien sv)
        {
            if (sv == null) throw new ArgumentNullException(nameof(sv));
            var existing = GetById(sv.MaSV);
            if (existing == null) return false;

            existing.HoTen = sv.HoTen;
            existing.NgaySinh = sv.NgaySinh;
            existing.DiemTB = sv.DiemTB;
            existing.GioiTinh = sv.GioiTinh;
            existing.Khoa = sv.Khoa;
            existing.SoThich = sv.SoThich;
            existing.DuongDanAnh = sv.DuongDanAnh;
            return true;
        }

        public bool Delete(string maSV)
        {
            var sv = GetById(maSV);
            if (sv != null)
            {
                return _data.Remove(sv);
            }
            return false;
        }

        public List<SinhVien> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAll();
            string k = keyword.Trim().ToLower();
            return _data.Where(x => x.MaSV.ToLower().Contains(k) ||
                                    x.HoTen.ToLower().Contains(k) ||
                                    x.Khoa.ToLower().Contains(k)).ToList();
        }

        // Bài 9: Tầng DAL xuất và nhập tệp CSV
        public void ExportToCSV(string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("MaSV,HoTen,NgaySinh,DiemTB,GioiTinh,Khoa,SoThich,DuongDanAnh");
            foreach (var sv in _data)
            {
                string line = $"\"{sv.MaSV}\",\"{sv.HoTen}\",\"{sv.NgaySinh:yyyy-MM-dd}\",{sv.DiemTB.ToString(CultureInfo.InvariantCulture)},\"{sv.GioiTinh}\",\"{sv.Khoa}\",\"{sv.SoThich}\",\"{sv.DuongDanAnh}\"";
                sb.AppendLine(line);
            }
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public List<SinhVien> ImportFromCSV(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Không tìm thấy tệp CSV.", filePath);

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length <= 1) return GetAll();

            var importedList = new List<SinhVien>();
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Tách cột CSV hỗ trợ dấu ngoặc kép
                string[] parts = ParseCsvLine(line);
                if (parts.Length >= 4)
                {
                    string ma = parts[0].Trim();
                    string ten = parts[1].Trim();
                    DateTime ns = DateTime.TryParse(parts[2].Trim(), out var d) ? d : new DateTime(2006, 1, 1);
                    double diem = double.TryParse(parts[3].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var dtb) ? dtb : 5.0;
                    string gt = parts.Length > 4 ? parts[4].Trim() : "Nam";
                    string khoa = parts.Length > 5 ? parts[5].Trim() : "Công nghệ thông tin";
                    string st = parts.Length > 6 ? parts[6].Trim() : "";
                    string anh = parts.Length > 7 ? parts[7].Trim() : "";

                    if (!string.IsNullOrWhiteSpace(ma) && !string.IsNullOrWhiteSpace(ten))
                    {
                        importedList.Add(new SinhVien(ma, ten, ns, diem, gt, khoa, st, anh));
                    }
                }
            }

            if (importedList.Count > 0)
            {
                _data.Clear();
                _data.AddRange(importedList);
            }
            return GetAll();
        }

        private static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var sb = new StringBuilder();
            foreach (char c in line)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            result.Add(sb.ToString());
            return result.ToArray();
        }
    }
}
