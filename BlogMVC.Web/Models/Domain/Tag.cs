namespace BlogMVC.Web.Models.Domain;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;

    public ICollection<BlogPost> BlogPosts { get; set; }
}