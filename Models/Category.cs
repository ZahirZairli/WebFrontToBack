namespace WebFrontToBack.Models;

public class Category
{
    public Category()
    {
        Services = new List<Service>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public virtual List<Service> Services { get; set; } //Virtual yazanda Lazy loading istifade olunur yeni isteyen vaxt caqirmaq 
                                                        //istesek onda ayrica kod yazmaliyiq
}
