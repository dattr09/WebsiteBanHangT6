using WebsiteBanHang.Models;
using Microsoft.EntityFrameworkCore;

namespace WebsiteBanHang.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Returning all categories
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            // Returning category by ID
#pragma warning disable CS8603 // Possible null reference return.
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddAsync(Category category)
        {
            // Adding a new category
            _ = _context.Categories.Add(category);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            // Updating an existing category
            _ = _context.Categories.Update(category);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Deleting category by ID
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _ = _context.Categories.Remove(category);
                _ = await _context.SaveChangesAsync();
            }
        }
    }
}
