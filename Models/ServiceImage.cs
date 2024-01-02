namespace WebFrontToBack.Models;

public class ServiceImage
{
    public int Id { get; set; }
    public string Path { get; set; }
    public bool IsActive { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
}
