using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Data;
using ZenBlog.Models.Process;

namespace ZenBlog.Controllers;

public class BlogController : Controller
{
    private readonly ApplicationDbContext _context;
     private ExcelProcess _excelProcess = new ExcelProcess();
    public BlogController (ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.Blogs.ToListAsync();
        return View(model);
    }
       public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Blog bg){
          if(ModelState.IsValid){
            _context.Add(bg);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          return View(bg);
    }
     private bool BlogExists(String id)
    {
        return _context.Blogs.Any(e => e.BlogId == id);
    }
    public async Task<IActionResult> Edit (String id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var blog = await _context.Blogs.FindAsync(id);
        if(blog == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(blog);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("BlogId,Title,Content,PostDay,Author")] Blog bg)
    {
          if(id != bg.BlogId)
          {
            //return NotFound();
            return View("NotFound");
          }
          if (ModelState.IsValid)
          {
            try 
            {
                _context.Update(bg);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!BlogExists(bg.BlogId))
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
          return View(bg);
    }
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var bg = await _context.Blogs.FirstOrDefaultAsync(m => m.BlogId == id);
        if(bg == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(bg);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(string id)
     {
        var bg = await _context.Blogs.FindAsync(id);
        _context.Blogs.Remove(bg);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));

     }
     public async Task<IActionResult> Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if(fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload");
                }
                else
                {
                    //rename file when upload to server
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //save file to server
                        await file.CopyToAsync(stream);
                        //read data from file and write to database
                        var dt = _excelProcess.ExcelToDataTable(fileLocation);
                        //using for loop to read data from dt
                        for(int i = 0; i < dt.Rows.Count; i++)
                        {
                            //create a new Person object
                            var bg = new Blog();
                            //set values for attributes
                            bg.BlogId = dt.Rows[i][0].ToString();
                            bg.Title = dt.Rows[i][1].ToString();
                            bg.Content = dt.Rows[i][2].ToString();
                            bg.PostDay = dt.Rows[i][3].ToString();
                            bg.Author = dt.Rows[i][4].ToString();
                            //add object to Context
                            if (!BlogExists(bg.BlogId))
                            {
                                _context.Blogs.Add(bg);    
                            }

                        }
                        //save to database
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    
                }
            }
            return View();
        }

}