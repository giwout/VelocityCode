using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models;
using CarMagazine2025_42.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarMagazine2025_42.Controllers
{
    public class HomeController : Controller
    {
        private readonly BikeDbContext _db;

        public HomeController(BikeDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string category)
        {
            IEnumerable<Bike> bikes;
            if (string.IsNullOrEmpty(category))
            {
                bikes = _db.Bikes.Include(b => b.Category).OrderBy(b => b.BikeId);
            }
            else
            {
                bikes = _db.Bikes.Include(b => b.Category)
                           .Where(b => b.Category.CategoryName == category)
                           .OrderBy(b => b.BikeId);
            }

            // Передаем модель, в которой есть Count
            var model = new IndexBikesModel { Bikes = bikes.ToList() };

            // В Index.cshtml используй @Model.Bikes.Count() вместо простого вывода
            return View(model);
        }

        public IActionResult About() => View();

        [Authorize(Roles = "Admin")]
        public IActionResult LoadTestData()
        {
            // Очистка БД
            _db.Bikes.RemoveRange(_db.Bikes);
            _db.Categories.RemoveRange(_db.Categories);
            _db.SaveChanges();

            // Создание категорий
            var catM = new Category { CategoryName = "Горные" };
            var catR = new Category { CategoryName = "Шоссейные" };
            var catC = new Category { CategoryName = "Городские" };
            _db.Categories.AddRange(catM, catR, catC);
            _db.SaveChanges();

            // Создание списка велосипедов с путями к локальным файлам
            var bikes = new List<Bike> {
        new Bike { BikeName = "Scott Spark", Price = 250000, Category = catM, Img = "/img/bikes/bike1.jpg", ShortDescription = "Трейловый монстр" },
        new Bike { BikeName = "Giant TCR", Price = 180000, Category = catR, Img = "/img/bikes/bike2.jpg", ShortDescription = "Аэродинамика" },
        new Bike { BikeName = "Trek Marlin", Price = 85000, Category = catM, Img = "/img/bikes/bike3.jpg", ShortDescription = "Надежный хардтейл" },
        new Bike { BikeName = "Specialized Sirrus", Price = 75000, Category = catC, Img = "/img/bikes/bike4.jpg", ShortDescription = "Городской фитнес" },
        new Bike { BikeName = "Cube Reaction", Price = 120000, Category = catM, Img = "/img/bikes/bike5.jpg", ShortDescription = "Немецкий стандарт" },
        new Bike { BikeName = "Bianchi Infinito", Price = 320000, Category = catR, Img = "/img/bikes/bike6.jpg", ShortDescription = "Итальянский шик" },
        new Bike { BikeName = "Marin Nicasio", Price = 95000, Category = catC, Img = "/img/bikes/bike7.jpg", ShortDescription = "Гравийный комфорт" },
        new Bike { BikeName = "Cannondale Habit", Price = 280000, Category = catM, Img = "/img/bikes/bike8.jpg", ShortDescription = "Агрессивный стиль" },
        new Bike { BikeName = "Merida Scultura", Price = 150000, Category = catR, Img = "/img/bikes/bike9.jpg", ShortDescription = "Гоночная классика" },
        new Bike { BikeName = "Electra Townie", Price = 65000, Category = catC, Img = "/img/bikes/bike10.jpg", ShortDescription = "Самый удобный" }
    };

            _db.Bikes.AddRange(bikes);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Search(string searchString)
        {
            // Получаем все велосипеды
            var bikes = _db.Bikes.Include(b => b.Category).AsQueryable();

            // Если строка поиска не пустая, фильтруем
            if (!string.IsNullOrEmpty(searchString))
            {
                bikes = bikes.Where(b => b.BikeName.ToLower().Contains(searchString.ToLower()));
            }

            // Если ничего не нашли, покажем пустой список, а не ошибку
            var model = new IndexBikesModel { Bikes = bikes.ToList() };

            return View("Index", model); // Возвращаем ту же страницу Index, но с отфильтрованным списком
        }
    }
}