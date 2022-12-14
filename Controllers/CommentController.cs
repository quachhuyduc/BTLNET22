using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Data;


namespace ZenBlog.Controllers;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;

    public CommentController (ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.Comments.ToListAsync();
        return View(model);
    }
           public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Comment cmt){
          if(ModelState.IsValid){
            _context.Add(cmt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          return View(cmt);
    }
     
     private bool CommentExists(String id)
    {
        return _context.Comments.Any(e => e.CommentId == id);
    }

   
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var cmt = await _context.Comments.FirstOrDefaultAsync(m => m.CommentId == id);
        if(cmt == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(cmt);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(string id)
     {
        var cmt = await _context.Comments.FindAsync(id);
        _context.Comments.Remove(cmt);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

     }
    

}