namespace QLRHM7.Interfaces
{
    public interface IUnitOfWork
    {
        IBenhNhanRepository benhNhanRepository { get; }
        Task<bool> SaveAsync();
    }
}
