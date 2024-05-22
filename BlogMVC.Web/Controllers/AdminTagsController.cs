using BlogMVC.Web.Data;
using BlogMVC.Web.Models.Domain;
using BlogMVC.Web.Models.ViewModels;
using BlogMVC.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Web.Controllers;

public class AdminTagsController : Controller
{
    private readonly ITagRepository _tagRepository;

    public AdminTagsController(ITagRepository tagRepository)
    {
        
        _tagRepository = tagRepository;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddTagRequest addTagRequest)
    {
        var tag = new Tag()
        {
            Name = addTagRequest.Name,
            DisplayName = addTagRequest.DisplayName
        };

        await _tagRepository.AddAsync(tag);
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        //use dbcontext to read tags
        var tags = await _tagRepository.GetAllAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _tagRepository.GetAsync(id);
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
    public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
    {
        var tag = new Tag()
        {
            Id = editTagRequest.Id,
            Name = editTagRequest.Name,
            DisplayName = editTagRequest.DisplayName
        };

        var updatedTag = await _tagRepository.UpdateAsync(tag);
        if (updatedTag != null)
        {
            //Show success message
        }
        else
        {
            //show failure notification
        }

        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
    {
        var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);
        if (deletedTag != null)
        {
            //show success notification
            return RedirectToAction("List");
        }

        //show error notification
        return RedirectToAction("Edit", new { id = editTagRequest.Id });
    }
}