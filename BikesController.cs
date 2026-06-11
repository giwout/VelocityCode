using CarMagazine2025_42.Data.Context;
using CarMagazine2025_42.Data.Models;
using CarMagazine2025_42.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarMagazine2025_42.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BikesController : Controller
    {
        private readonly BikeDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BikesController(BikeDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }

        public IActionResult BikeList(int page = 1)
        {
            int pageSize = 10; // Или другое значение для админки
            var totalItems = _db.Bikes.Count();
            var bikes = _db.Bikes.OrderBy(b => b.BikeId)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            var model = new IndexBikesModel
            {
                Bikes = bikes,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            };

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult BikeInfo(int bikeId)
        {
            var bike = _db.Bikes
                          .Include(b => b.Category) // Важно: подгружаем категорию
                          .FirstOrDefault(b => b.BikeId == bikeId);

            if (bike == null)
            {
                // Для диплома лучше перенаправить в каталог, чем показывать 404
                return RedirectToAction("Index", "Home");
            }

            return View(bike);
        }

        [HttpGet]
        public IActionResult AddBike() => View(new AddBikePagingModel
        {
            Bike = new Bike(),
            Categories = _db.Categories.ToList()
        });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBike(Bike bike, IFormFile? uploadedFile)
        {
            if (!ModelState.IsValid)
            {
                var model = new AddBikePagingModel { Bike = bike, Categories = _db.Categories.ToList() };
                return View(model);
            }

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string fullPath = Path.Combine(_env.WebRootPath, "img", fileName);

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                bike.Img = "/img/" + fileName;
            }

            _db.Bikes.Add(bike);
            await _db.SaveChangesAsync();
            return RedirectToAction("BikeList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var bike = _db.Bikes.FirstOrDefault(x => x.BikeId == id);
            if (bike != null)
            {
                _db.Bikes.Remove(bike);
                _db.SaveChanges();
            }
            return RedirectToAction("BikeList");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bike = _db.Bikes.FirstOrDefault(b => b.BikeId == id);
            if (bike == null) return NotFound();

            var model = new AddBikePagingModel
            {
                Bike = bike,
                Categories = _db.Categories.ToList()
            };
            return View("AddBike", model); // Используем ту же вьюху, что и для создания
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Bike bike, IFormFile? uploadedFile)
        {
            if (!ModelState.IsValid)
            {
                var model = new AddBikePagingModel { Bike = bike, Categories = _db.Categories.ToList() };
                return View("AddBike", model);
            }

            if (uploadedFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                string fullPath = Path.Combine(_env.WebRootPath, "img", fileName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                bike.Img = "/img/" + fileName;
            }

            _db.Bikes.Update(bike);
            await _db.SaveChangesAsync();
            return RedirectToAction("BikeList");
        }
    }
}