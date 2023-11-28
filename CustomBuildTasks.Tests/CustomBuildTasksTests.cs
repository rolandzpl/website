using static CustomBuildTasks.SubstituteVariablesTask;

namespace CustomBuildTasks;

public class CustomBuildTasksTests
{
    [Test]
    public void SubstituteVariables_GivenFile_xxxxxxxx()
    {
        SubstituteVariables("Dictionary.txt", "Sample.txt");
        Assert.That(File.ReadAllText("Sample.txt"), Is.EqualTo(File.ReadAllText("Expected.txt")));
    }
}