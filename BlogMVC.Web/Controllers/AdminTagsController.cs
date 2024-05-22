using BlogMVC.Web.Data;
using BlogMVC.Web.Models.Domain;
using BlogMVC.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogMVC.Web.Controllers;

public class AdminTagsController : Controller
{
    private readonly BlogDbContext _blogDbContext;

    public AdminTagsController(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag()
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };
        
        _blogDbContext.Tags.Add(tag);
        _blogDbContext.SaveChanges();
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult List()
    {
        //use dbcontext to read tags
        var tags = _blogDbContext.Tags.ToList();
        return View(tags);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
       var tag = _blogDbContext.Tags.FirstOrDefault(t => t.Id == id);
       if (tag is not null)
       {
           var editTagRequest = new EditTagRequest()
           {
               Id = tag.Id,
               Name = tag.Name,
               DisplayName = tag.DisplayName
           };

           return View(editTagRequest);
       }
       return View(null);
    }

    [HttpPost]
    public IActionResult Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag()
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };

        var existingTag = _blogDbContext.Tags.FirstOrDefault(t => t.Id == editTagRequest.Id);
        if (existingTag is not null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            _blogDbContext.SaveChanges();
            
            //show success notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }
        //show failure notification
        return RedirectToAction("Edit", new {id = editTagRequest.Id});
    }

    [HttpPost]
    public IActionResult Delete(EditTagRequest editTagRequest)
    {
        var existingTag = _blogDbContext.Tags.FirstOrDefault(t => t.Id == editTagRequest.Id);
        if (existingTag != null)
        {
            _blogDbContext.Tags.Remove(existingTag);
            _blogDbContext.SaveChanges();
            
            //show success notification
            return RedirectToAction("List");
        }
            //show error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}