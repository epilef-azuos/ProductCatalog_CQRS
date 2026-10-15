namespace ProductCatalog.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        Task CommitAsync();
    }
}
