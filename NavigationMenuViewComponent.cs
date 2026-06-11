using CarMagazine2025_42.Data.Context;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CarMagazine2025_42.Views.Shared.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly BikeDbContext db;
        public NavigationMenuViewComponent(BikeDbContext db) => this.db = db;

        public IViewComponentResult Invoke()
        {
            // Считываем ID категории из параметров маршрута (URL)
            // Это безопасный и правильный способ
            var categoryId = RouteData?.Values["categoryId"];
            ViewBag.SelectedCategory = categoryId != null ? Convert.ToInt32(categoryId) : 0;

            return View(db.Categories.OrderBy(c => c.CategoryName));
        }
    }
}