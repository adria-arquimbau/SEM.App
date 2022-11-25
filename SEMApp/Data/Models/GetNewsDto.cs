namespace SEM.App.Data.Models;

public class GetNewsDto
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public Guid EventId { get; set; }
    public string Title { get; set; }
    public DateTime CreationDate { get; set; }
    public string AuthorName { get; set; }
}
