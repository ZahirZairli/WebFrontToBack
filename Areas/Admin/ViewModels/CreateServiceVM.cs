using WebFrontToBack.Models;

namespace WebFrontToBack.Areas.Admin.ViewModels;

public class CreateServiceVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public List<IFormFile> Photos { get; set; }
    public List<Category>? Categories { get; set; }
}
