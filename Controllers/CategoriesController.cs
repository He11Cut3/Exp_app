using Expenses_App.Infrastructure;
using Expenses_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses_App.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = _context.ExpenseCategories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseCategory category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.ExpenseCategories
                                         .Include(c => c.Expenses)
                                         .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.Expenses.Any())
            {
                ModelState.AddModelError("", "Невозможно удалить категорию, так как она содержит связанные расходы.");

                return View("Index", await _context.ExpenseCategories.ToListAsync());
            }

            _context.ExpenseCategories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
