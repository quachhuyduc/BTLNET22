using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Data;


namespace ZenBlog.Controllers;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public AdminController (ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.Admins.ToListAsync();
        return View(model);
    }
       public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Admin ad){
          if(ModelState.IsValid){
            _context.Add(ad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          return View(ad);
    }
     private bool AdminExists(String id)
    {
        return _context.Admins.Any(e => e.AdminId == id);
    }
    public async Task<IActionResult> Edit (String id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var admin = await _context.Admins.FindAsync(id);
        if(admin == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(admin);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("UserName,Password")] Admin ad)
    {
          if(id != ad.AdminId)
          {
            //return NotFound();
            return View("NotFound");
          }
          if (ModelState.IsValid)
          {
            try 
            {
                _context.Update(ad);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!AdminExists(ad.AdminId))
                {
                    //return NotFound();
                    return View("NotFound");
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
          }
          return View(ad);
    }
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var ad = await _context.Admins.FirstOrDefaultAsync(m => m.AdminId == id);
        if(ad == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(ad);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(string id)
     {
        var ad = await _context.Admins.FindAsync(id);
        _context.Admins.Remove(ad);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

     }
     
}