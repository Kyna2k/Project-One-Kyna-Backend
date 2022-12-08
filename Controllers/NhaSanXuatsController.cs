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
    public class NhaSanXuatsController : Controller
    {
        private readonly KynaShopContext _context;

        public NhaSanXuatsController(KynaShopContext context)
        {
            _context = context;
        }

        // GET: NhaSanXuats
        public async Task<IActionResult> Index()
        {
              return View(await _context.NhaSanXuats.ToListAsync());
        }

        // GET: NhaSanXuats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhaSanXuats == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.MaNhaSanXuat == id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhaSanXuats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhaSanXuat,TenNhaSanXuat,Hinh")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaSanXuat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhaSanXuats == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNhaSanXuat,TenNhaSanXuat,Hinh")] NhaSanXuat nhaSanXuat)
        {
            if (id != nhaSanXuat.MaNhaSanXuat)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhaSanXuat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhaSanXuatExists(nhaSanXuat.MaNhaSanXuat))
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
            return View(nhaSanXuat);
        }

        // GET: NhaSanXuats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhaSanXuats == null)
            {
                return NotFound();
            }

            var nhaSanXuat = await _context.NhaSanXuats
                .FirstOrDefaultAsync(m => m.MaNhaSanXuat == id);
            if (nhaSanXuat == null)
            {
                return NotFound();
            }

            return View(nhaSanXuat);
        }

        // POST: NhaSanXuats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhaSanXuats == null)
            {
                return Problem("Entity set 'KynaShopContext.NhaSanXuats'  is null.");
            }
            var nhaSanXuat = await _context.NhaSanXuats.FindAsync(id);
            if (nhaSanXuat != null)
            {
                _context.NhaSanXuats.Remove(nhaSanXuat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhaSanXuatExists(int id)
        {
          return _context.NhaSanXuats.Any(e => e.MaNhaSanXuat == id);
        }
    }
}
