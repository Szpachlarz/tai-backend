using tai_shop.Interfaces;

namespace tai_shop.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
