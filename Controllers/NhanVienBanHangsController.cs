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
    public class NhanVienBanHangsController : Controller
    {
        private readonly KynaShopContext _context;

        public NhanVienBanHangsController(KynaShopContext context)
        {
            _context = context;
        }

        // GET: NhanVienBanHangs
        public async Task<IActionResult> Index()
        {
              return View(await _context.NhanVienBanHangs.ToListAsync());
        }

        // GET: NhanVienBanHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NhanVienBanHangs == null)
            {
                return NotFound();
            }

            var nhanVienBanHang = await _context.NhanVienBanHangs
                .FirstOrDefaultAsync(m => m.MaNhanVien == id);
            if (nhanVienBanHang == null)
            {
                return NotFound();
            }

            return View(nhanVienBanHang);
        }

        // GET: NhanVienBanHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NhanVienBanHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNhanVien,Username,Password,Avatar,NgaySinh,DiaChi,SoDienThoai")] NhanVienBanHang nhanVienBanHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhanVienBanHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVienBanHang);
        }

        // GET: NhanVienBanHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NhanVienBanHangs == null)
            {
                return NotFound();
            }

            var nhanVienBanHang = await _context.NhanVienBanHangs.FindAsync(id);
            if (nhanVienBanHang == null)
            {
                return NotFound();
            }
            return View(nhanVienBanHang);
        }

        // POST: NhanVienBanHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNhanVien,Username,Password,Avatar,NgaySinh,DiaChi,SoDienThoai")] NhanVienBanHang nhanVienBanHang)
        {
            if (id != nhanVienBanHang.MaNhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVienBanHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NhanVienBanHangExists(nhanVienBanHang.MaNhanVien))
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
            return View(nhanVienBanHang);
        }

        // GET: NhanVienBanHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NhanVienBanHangs == null)
            {
                return NotFound();
            }

            var nhanVienBanHang = await _context.NhanVienBanHangs
                .FirstOrDefaultAsync(m => m.MaNhanVien == id);
            if (nhanVienBanHang == null)
            {
                return NotFound();
            }

            return View(nhanVienBanHang);
        }

        // POST: NhanVienBanHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NhanVienBanHangs == null)
            {
                return Problem("Entity set 'KynaShopContext.NhanVienBanHangs'  is null.");
            }
            var nhanVienBanHang = await _context.NhanVienBanHangs.FindAsync(id);
            if (nhanVienBanHang != null)
            {
                _context.NhanVienBanHangs.Remove(nhanVienBanHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienBanHangExists(int id)
        {
          return _context.NhanVienBanHangs.Any(e => e.MaNhanVien == id);
        }
    }
}
