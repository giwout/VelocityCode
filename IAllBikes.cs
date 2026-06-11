using CarMagazine2025_42.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarMagazine2025_42.Data.Interfaces
{
    public interface IAllBikes
    {
        // Используем Task для асинхронной работы с базой данных
        Task<IEnumerable<Bike>> GetAllBikesAsync();

        Task<IEnumerable<Bike>> GetFavBikesAsync();

        Task<Bike?> GetObjectBikeAsync(int bikeId);
    }
}