using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Data;


namespace ZenBlog.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
     
    public UserController (ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.Users.ToListAsync();
        return View(model);
    }
       public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(User us){
          if(ModelState.IsValid){
            _context.Add(us);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          return View(us);
    }
     private bool UserExists(String id)
    {
        return _context.Users.Any(e => e.UserId == id);
    }
    public async Task<IActionResult> Edit (String id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var user = await _context.Users.FindAsync(id);
        if(user == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("UserId,UserName,Passwword,UserCommnet,DayJoin")] User us)
    {
          if(id != us.UserId)
          {
            //return NotFound();
            return View("NotFound");
          }
          if (ModelState.IsValid)
          {
            try 
            {
                _context.Update(us);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!UserExists(us.UserId))
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
          return View(us);
    }
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var us = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
        if(us == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(us);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(string id)
     {
        var us = await _context.Users.FindAsync(id);
        _context.Users.Remove(us);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

     }
    

}