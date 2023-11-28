namespace website.Services;

public interface IFileSystem
{
    IEnumerable<string> GetFiles(string path);

    string GetFileName(string path);

    string GetFileNameWithoutExtension(string path);

    bool Exists(string path);

    string GetExtension(string path);

    TextReader OpenText(string path);

    IEnumerable<string> GetDirectories(string path);

    string PathCombine(params string[] paths);

    Task<string> ReadAllTextAsync(string path);

    Stream OpenRead(string path);
    
    Stream Open(string path);
}
