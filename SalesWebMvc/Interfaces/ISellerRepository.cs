using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public interface ISellerRepository
    {
        Task<List<Seller>> FindAllAsync();

        Task<Boolean> HasSellerAsync(int Id);
        Task InsertAsync(Seller seller);
        Task<Seller> FindByIdAsync(int id);
        Task RemoveAsync(Seller seller);
        Task UpdateAsync(Seller seller);
    }
}
