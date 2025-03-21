using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Products
            .Include(p => p.Category) // Include thông tin về category
            .ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public async Task AddAsync(Product product)
        {
            _ = _context.Products.Add(product);
            _ = await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _ = _context.Products.Update(product);
            _ = await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
#pragma warning disable CS8604 // Possible null reference argument.
            _ = _context.Products.Remove(product);
#pragma warning restore CS8604 // Possible null reference argument.
            _ = await _context.SaveChangesAsync();
        }
    }
}
