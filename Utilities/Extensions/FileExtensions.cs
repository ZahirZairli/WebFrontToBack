using Microsoft.AspNetCore.Hosting;

namespace WebFrontToBack.Utilities.Extensions;

public static class FileExtensions
{
    public static bool CheckContentType(this IFormFile file,string contentType)
    {
        return file.ContentType.Contains(contentType);
    }
    public static bool CheckContentSize(this IFormFile file,double kb)
    {
        return file.Length / 1024 < kb;
    }
    public async static Task<string> SaveAsync(this IFormFile file,string root)
    {
        string fileName = Guid.NewGuid().ToString() + file.FileName;
        string resultroot = Path.Combine(root, fileName); //Belelikle biz rootun ucundeki assets in icindeki img nin yolun goturduk   
        using (FileStream fileStream = new FileStream(resultroot, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        return fileName;
    }
}
