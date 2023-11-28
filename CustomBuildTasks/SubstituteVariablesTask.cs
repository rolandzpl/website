using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CustomBuildTasks;

public class SubstituteVariablesTask : Task
{
    public string SourceFile { get; set; }

    public string TargetFile { get; set; }

    public string DictionaryFile { get; set; }

    public override bool Execute()
    {
        Log.LogMessage(MessageImportance.High, "Replacing variables in file");
        SubstituteVariables(DictionaryFile, SourceFile, TargetFile);
        return true;
    }

    public static void SubstituteVariables(string dictionaryFile, string sourceFile, string? targetFile = null)
    {
        var dictionary = File.ReadAllLines(dictionaryFile)
            .Select(_ => _.Split('=', 2))
            .ToDictionary(line => line[0], line => line[1]);
        var content = File.ReadAllText(sourceFile);
        foreach (var replacement in dictionary)
        {
            content = content.Replace($"${{{replacement.Key}}}", replacement.Value);
        }
        File.WriteAllText(targetFile ?? sourceFile, content);
    }
}
