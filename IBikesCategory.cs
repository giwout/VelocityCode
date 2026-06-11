using CarMagazine2025_42.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarMagazine2025_42.Data.Interfaces
{
    public interface IBikesCategory
    {
        // Используем Task для асинхронного получения всех категорий
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
    }
}