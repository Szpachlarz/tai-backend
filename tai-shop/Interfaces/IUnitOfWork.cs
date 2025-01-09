namespace tai_shop.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();
    }
}
