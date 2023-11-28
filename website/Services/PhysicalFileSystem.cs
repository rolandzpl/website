namespace website.Services;

public class PhysicalFileSystem : IFileSystem
{
    public IEnumerable<string> GetFiles(string rootPath) => Directory.GetFiles(rootPath);

    public string GetFileName(string path) => Path.GetFileName(path);

    public string GetFileNameWithoutExtension(string path) => Path.GetFileNameWithoutExtension(path);

    public bool Exists(string path) => File.Exists(path);

    public string GetExtension(string path) => Path.GetExtension(path);

    public TextReader OpenText(string path) => File.OpenText(path);

    public IEnumerable<string> GetDirectories(string path) => Directory.GetDirectories(path);

    public string PathCombine(params string[] paths) => Path.Combine(paths);

    public Task<string> ReadAllTextAsync(string path) => File.ReadAllTextAsync(path);

    public Stream OpenRead(string path) => File.OpenRead(path);

    public Stream Open(string path) => File.Open(path, FileMode.OpenOrCreate);
}
