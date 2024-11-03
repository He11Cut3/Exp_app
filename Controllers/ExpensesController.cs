using Expenses_App.Infrastructure;
using Expenses_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Expenses_App.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly DataContext _context;

        public ExpensesController(DataContext context)
        {
            _context = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .ToListAsync();
            return View(expenses);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ExpenseCategories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _context.Expenses.Include(e => e.Category).FirstOrDefaultAsync(e => e.ExpenseId == id);
            if (expense == null)
            {
                return NotFound();
            }

            var categories = await _context.ExpenseCategories.ToListAsync();

            if (categories == null || !categories.Any())
            {
                ModelState.AddModelError("", "Не удалось загрузить категории. Пожалуйста, добавьте категории перед редактированием расходов.");
                return View(expense);
            }

            ViewBag.Categories = categories;
            return View(expense);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,Date,Amount,Comment,CategoryId")] Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseId == id);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MonthlyExpenses()
        {
            var monthlyExpenses = await _context.Expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            return View(monthlyExpenses);
        }
        public async Task<IActionResult> DownloadReport()
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .ToListAsync();

            var expensesByMonth = expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month);

            using (var package = new ExcelPackage())
            {
                foreach (var monthGroup in expensesByMonth)
                {
                    var sheetName = new DateTime(monthGroup.Key.Year, monthGroup.Key.Month, 1)
                        .ToString("MMMM yyyy", System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));

                    var worksheet = package.Workbook.Worksheets.Add(sheetName);


                    worksheet.Cells[1, 1].Value = "Категория";
                    worksheet.Cells[1, 2].Value = "Сумма";
                    worksheet.Cells[1, 3].Value = "Комментарий";
                    worksheet.Cells[1, 4].Value = "Дата";

                    int row = 2; 
                    foreach (var expense in monthGroup)
                    {
                        worksheet.Cells[row, 1].Value = expense.Category?.Name;
                        worksheet.Cells[row, 2].Value = expense.Amount;
                        worksheet.Cells[row, 3].Value = expense.Comment;
                        worksheet.Cells[row, 4].Value = expense.Date.ToString("yyyy-MM-dd");
                        row++;
                    }

                    
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);
                stream.Position = 0;

                var fileName = $"Отчёт_расходы_{DateTime.Now:yyyyMMdd}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }




    }
}
