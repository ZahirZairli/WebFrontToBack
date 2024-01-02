namespace WebFrontToBack.Models;

public class Service
{
    public Service()
    {
        ServiceImages = new List<ServiceImage>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category{ get; set; }
    public virtual List<ServiceImage> ServiceImages { get; set; }
}
