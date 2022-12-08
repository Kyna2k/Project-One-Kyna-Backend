using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KynaShop.Models;

namespace KynaShop.Controllers
{
    public class HinhsController : Controller
    {
        private readonly KynaShopContext _context;

        public HinhsController(KynaShopContext context)
        {
            _context = context;
        }

        // GET: Hinhs
        public async Task<IActionResult> Index()
        {
            var kynaShopContext = _context.Hinhs.Include(h => h.MaSanPhamNavigation);
            return View(await kynaShopContext.ToListAsync());
        }

        // GET: Hinhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hinhs == null)
            {
                return NotFound();
            }

            var hinh = await _context.Hinhs
                .Include(h => h.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.Mahinh == id);
            if (hinh == null)
            {
                return NotFound();
            }

            return View(hinh);
        }

        // GET: Hinhs/Create
        public IActionResult Create()
        {
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham");
            return View();
        }

        // POST: Hinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Mahinh,MaSanPham,Url")] Hinh hinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hinh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", hinh.MaSanPham);
            return View(hinh);
        }

        // GET: Hinhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hinhs == null)
            {
                return NotFound();
            }

            var hinh = await _context.Hinhs.FindAsync(id);
            if (hinh == null)
            {
                return NotFound();
            }
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", hinh.MaSanPham);
            return View(hinh);
        }

        // POST: Hinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Mahinh,MaSanPham,Url")] Hinh hinh)
        {
            if (id != hinh.Mahinh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HinhExists(hinh.Mahinh))
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
            ViewData["MaSanPham"] = new SelectList(_context.SanPhams, "MaSanPham", "MaSanPham", hinh.MaSanPham);
            return View(hinh);
        }

        // GET: Hinhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hinhs == null)
            {
                return NotFound();
            }

            var hinh = await _context.Hinhs
                .Include(h => h.MaSanPhamNavigation)
                .FirstOrDefaultAsync(m => m.Mahinh == id);
            if (hinh == null)
            {
                return NotFound();
            }

            return View(hinh);
        }

        // POST: Hinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hinhs == null)
            {
                return Problem("Entity set 'KynaShopContext.Hinhs'  is null.");
            }
            var hinh = await _context.Hinhs.FindAsync(id);
            if (hinh != null)
            {
                _context.Hinhs.Remove(hinh);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HinhExists(int id)
        {
          return _context.Hinhs.Any(e => e.Mahinh == id);
        }
    }
}
