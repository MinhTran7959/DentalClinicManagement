using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLRHM7.DTOs
{
    public class BenhNhanDto
    {
        public int Idbn { get; set; }

        public string MaBenhNhan { get; set; } = null!;

        public string TenBn { get; set; } = null!;
     
        public DateTime NgaySinh { get; set; }
      
        public string? AnhBn { get; set; }
       
        public IFormFile? FrontImage { get; set; }
        public string GioiTinh { get; set; } = null!;
        public string? DiaChi { get; set; }

        public string? Sdt { get; set; }
        public int? Phong { get; set; }
        public int? Active { get; set; }
    }
}
