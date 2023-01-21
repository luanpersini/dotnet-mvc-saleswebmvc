using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;


namespace SalesWebMvc.Services
{
    public class SellerService : ISellerService
    {
        private readonly SalesWebMvcContext _context;
        private readonly ISellerRepository _sellerRepository;

        public SellerService(SalesWebMvcContext context, ISellerRepository sellerRepository)
        {
            _context = context;
            _sellerRepository = sellerRepository;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _sellerRepository.FindAllAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            await _sellerRepository.InsertAsync(seller);
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            var seller = await this._sellerRepository.FindByIdAsync(id);

            if (seller == null)
            {
                throw new NotFoundException("Id not found");
            }

            return seller;
        }

        public async Task RemoveAsync(int id)
        {
            var seller = await this._sellerRepository.FindByIdAsync(id);

            if (seller == null)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                await this._sellerRepository.RemoveAsync(seller);
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            await Task.WhenAll(this.CheckIfSellerExistsAsync(obj.Id));

            try
            {
                await _sellerRepository.UpdateAsync(obj);
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        private async Task CheckIfSellerExistsAsync(int id)
        {
            bool hasSeller = await _sellerRepository.HasSellerAsync(id);

            if (!hasSeller)
            {
                throw new NotFoundException("Id not found");
            }

        }
    }
}