namespace Lithium.Website;

public class WebsiteConfiguration
{
    public string Name { get; set; }
    public string StartPage { get; set; }
    public string DataPath { get; set; }
    public string ImagesPath { get; set; }
    public string Instagram { get; set; }
    public AdministratorConfiguration Administrator { get; set; }
    public Part Privacy { get; set; }
    public string InfoPart{ get; set; }
    public string BaseUrl { get; set; }
}

public class Part
{
    public string LinkText { get; set; }
    public string Link { get; set; }
}

public class AdministratorConfiguration
{
    public string Email { get; set; }
}