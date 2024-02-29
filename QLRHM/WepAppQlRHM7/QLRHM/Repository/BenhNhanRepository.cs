using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using QLRHM.Models;
using QLRHM7.Interfaces;

namespace QLRHM7.Repository
{
    public class BenhNhanRepository : IBenhNhanRepository
    {
        private readonly DatnqlrhmContext context;
        public BenhNhanRepository(DatnqlrhmContext context)
        {
            this.context = context;       
    }
        public async Task<IEnumerable<BenhNhan>> GetListBenhNhan()
        {
            return await context.BenhNhans.Where(x => x.Active == 1).OrderByDescending(x => x.Idbn).Take(100).ToListAsync();

        }

        public async Task<IEnumerable<BenhNhan>> GetListBenhNhanAn()
        {
            return await context.BenhNhans.Where(x => x.Active == 1).OrderByDescending(x => x.Idbn).Take(100).ToListAsync();
        }

        public async Task<IEnumerable<BenhNhan>> GetListBenhNhanPK()
        {
            return await context.BenhNhans.Where(x => x.Active == 0).Take(100).OrderByDescending(x => x.Idbn).ToListAsync();
        }

        public async Task<IEnumerable<BenhNhan>> Search(string searchString)
        {
            var bn = await GetListBenhNhan();
            if (!string.IsNullOrEmpty(searchString))
            {
                bn = await context.BenhNhans
                    .Where(x => x.Active == 1 &&
                        (x.MaBenhNhan.Contains(searchString) ||
                         x.TenBn.Contains(searchString) ||
                         x.Sdt.Contains(searchString) ||
                         x.Cccd.Contains(searchString) ||
                         x.Email.Contains(searchString)))
                    .ToListAsync();
            }
            return bn;
        }


        public async Task<int> TongBn()
        {
            return await context.BenhNhans.CountAsync();
        }

    }
}
