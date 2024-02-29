using QLRHM.Models;

namespace QLRHM7.Interfaces
{
    public interface IBenhNhanRepository
    {
        Task<IEnumerable<BenhNhan>> GetListBenhNhan();
        Task<IEnumerable<BenhNhan>> GetListBenhNhanAn();
        Task<IEnumerable<BenhNhan>> GetListBenhNhanPK();
        Task<int> TongBn();
        Task<IEnumerable<BenhNhan>> Search(string searchString);

    }
}
