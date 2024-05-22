using BlogMVC.Web.Data;
using BlogMVC.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Web.Repositories;

public class TagRepository : ITagRepository
{
    private readonly BlogDbContext _blogDbContext;

    public TagRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
    
    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
       return await _blogDbContext.Tags.ToListAsync();
    }

    public Task<Tag?> GetAsync(Guid id)
    {
        return _blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag> AddAsync(Tag tag)
    {
        await _blogDbContext.Tags.AddAsync(tag);
        await _blogDbContext.SaveChangesAsync();

        return tag;
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == tag.Id);

        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            await _blogDbContext.SaveChangesAsync();
            return existingTag;
        }

        return null;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var existingTag = await _blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (existingTag != null)
        {
            _blogDbContext.Tags.Remove(existingTag);
            await _blogDbContext.SaveChangesAsync();
            return existingTag;
        }

        return null;
    }
}