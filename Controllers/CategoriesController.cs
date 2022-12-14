using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Models;
using Microsoft.EntityFrameworkCore;
using ZenBlog.Data;
using ZenBlog.Models.Process;

namespace ZenBlog.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;
     private ExcelProcess _excelProcess = new ExcelProcess();
    public CategoriesController (ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var model = await _context.Categoriess.ToListAsync();
        return View(model);
    }
       public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Categories cate){
          if(ModelState.IsValid){
            _context.Add(cate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
          }
          return View(cate);
    }
     private bool CategoriesExists(String id)
    {
        return _context.Categoriess.Any(e => e.CategoriesId== id);
    }
    public async Task<IActionResult> Edit (String id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var categories = await _context.Categoriess.FindAsync(id);
        if(categories == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(categories);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CateId,CateName")] Categories cate)
    {
          if(id != cate.CategoriesId)
          {
            //return NotFound();
            return View("NotFound");
          }
          if (ModelState.IsValid)
          {
            try 
            {
                _context.Update(cate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!CategoriesExists(cate.CategoriesId))
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
          return View(cate);
    }
    public async Task<IActionResult> Delete(string id)
    {
        if(id == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        var cate = await _context.Categoriess.FirstOrDefaultAsync(m => m.CategoriesId == id);
        if(cate == null)
        {
            //return NotFound();
            return View("NotFound");
        }
        return View(cate);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
     public async Task<IActionResult> DeleteConfirmed(string id)
     {
        var cate = await _context.Categoriess.FindAsync(id);
        _context.Categoriess.Remove(cate);
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
                            var cate = new Categories();
                            //set values for attributes
                            cate.CategoriesId = dt.Rows[i][0].ToString();
                            cate.CategoriesName = dt.Rows[i][1].ToString();
                           
                            //add object to Context
                            if (!CategoriesExists(cate.CategoriesId))
                            {
                                _context.Categoriess.Add(cate);    
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