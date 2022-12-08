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
    public class SanPhamsController : Controller
    {
        private readonly KynaShopContext _context;

        public SanPhamsController(KynaShopContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var kynaShopContext = _context.SanPhams.Include(s => s.MaKhuyenMaiNavigation).Include(s => s.MaNhaSanXuatNavigation);
            return View(await kynaShopContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaNhaSanXuatNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            List<KhuyenMai> listKM = new List<KhuyenMai>();
            KhuyenMai km = new KhuyenMai();
            listKM.Add(km);
            listKM.AddRange(_context.KhuyenMais.ToList());
            ViewData["MaKhuyenMai"] = new SelectList(listKM, "MaKhuyenMai", "MaKhuyenMai");
            //ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai");
            ViewData["MaNhaSanXuat"] = new SelectList(_context.NhaSanXuats, "MaNhaSanXuat", "MaNhaSanXuat");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,GiaGoc,GiaBan,SoLuongTrongKho,MaNhaSanXuat,Mota,TrangThai,MaKhuyenMai,ThongSo")] SanPham sanPham)
        {
            if(sanPham.MaKhuyenMai == -1)
            {
                sanPham.MaKhuyenMai = null;
            }    
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<KhuyenMai> listKM = new List<KhuyenMai>();
            KhuyenMai km = new KhuyenMai();
            km.MaKhuyenMai = -1;
            listKM.Add(km);
            listKM.AddRange(_context.KhuyenMais.ToList());
            ViewData["MaKhuyenMai"] = new SelectList(listKM, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            //ViewData["MaKhuyenMai"] = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "MaKhuyenMai", sanPham.MaKhuyenMai);
            ViewData["MaNhaSanXuat"] = new SelectList(_context.NhaSanXuats, "MaNhaSanXuat", "MaNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            List<KhuyenMai> listKM = new List<KhuyenMai>();
            KhuyenMai km = new KhuyenMai();
            km.MaKhuyenMai = -1;
            listKM.Add(km);
            listKM.AddRange(_context.KhuyenMais.ToList());
            ViewData["MaKhuyenMai"] = new SelectList(listKM, "MaKhuyenMai", "MaKhuyenMai");
            ViewData["MaNhaSanXuat"] = new SelectList(_context.NhaSanXuats, "MaNhaSanXuat", "MaNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,TenSanPham,GiaGoc,GiaBan,SoLuongTrongKho,MaNhaSanXuat,Mota,TrangThai,MaKhuyenMai,ThongSo")] SanPham sanPham)
        {
            if (id != sanPham.MaSanPham)
            {
                return NotFound();
            }
            if(sanPham.MaKhuyenMai == -1)
            {
                sanPham.MaKhuyenMai = null;
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSanPham))
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
            List<KhuyenMai> listKM = new List<KhuyenMai>();
            KhuyenMai km = new KhuyenMai();
            km.MaKhuyenMai = -1;
            listKM.Add(km);
            listKM.AddRange(_context.KhuyenMais.ToList());
            ViewData["MaKhuyenMai"] = new SelectList(listKM, "MaKhuyenMai", "MaKhuyenMai");
            ViewData["MaNhaSanXuat"] = new SelectList(_context.NhaSanXuats, "MaNhaSanXuat", "MaNhaSanXuat", sanPham.MaNhaSanXuat);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SanPhams == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaKhuyenMaiNavigation)
                .Include(s => s.MaNhaSanXuatNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SanPhams == null)
            {
                return Problem("Entity set 'KynaShopContext.SanPhams'  is null.");
            }
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
          return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}
