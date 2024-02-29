using QLRHM.Models;
using QLRHM7.Interfaces;
using QLRHM7.Repository;

namespace QLRHM7.DTOs
{
    public class UnitOfWork:IUnitOfWork
    {
        private DatnqlrhmContext context;
        public UnitOfWork(DatnqlrhmContext context)
        {
            this.context = context;
        }

        public IBenhNhanRepository benhNhanRepository =>  new BenhNhanRepository(context);

        public async Task<bool> SaveAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}
