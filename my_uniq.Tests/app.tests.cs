using System.Text;       
using my_uniq;         
namespace my_uniq.Tests; 

public class AppTests
{
    [Test]
    public void Run_RemovesAdjacentDuplicates()
    {
        var input = new StringReader("a\na\nb\nb\nc\n");

        var output = new StringWriter();

        var error = new StringWriter();

        int exitCode = App.Run(Array.Empty<string>(), input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));

  
        Assert.That(output.ToString(),
            Is.EqualTo("a\nb\nc\n".Replace("\n", Environment.NewLine)));

        Assert.That(error.ToString(), Is.Empty);
    }

    [Test]
    public void Run_WithIgnoreCase_RemovesCaseInsensitiveDuplicates()
    {
        var input = new StringReader("a\nA\nb\nB\nc\n");
        var output = new StringWriter();
        var error = new StringWriter();


        int exitCode = App.Run(new[] { "-i" }, input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));


        Assert.That(output.ToString(),
            Is.EqualTo("a\nb\nc\n".Replace("\n", Environment.NewLine)));

        Assert.That(error.ToString(), Is.Empty);
    }

    [Test]
    public void Run_WithCount_WritesCounts()
    {
        var input = new StringReader("a\na\na\nb\nb\nc\n");
        var output = new StringWriter();
        var error = new StringWriter();

        int exitCode = App.Run(new[] { "-c" }, input, output, error);

        Assert.That(exitCode, Is.EqualTo(0));

        Assert.That(output.ToString(),
            Is.EqualTo("3 a\n2 b\n1 c\n".Replace("\n", Environment.NewLine)));

        Assert.That(error.ToString(), Is.Empty);
    }

    [Test]
    public void Run_UnknownOption_Returns2AndWritesError()
    {
        var input = new StringReader("");
        var output = new StringWriter();
        var error = new StringWriter();

        int exitCode = App.Run(new[] { "-x" }, input, output, error);

        Assert.That(exitCode, Is.EqualTo(2));

        Assert.That(output.ToString(), Is.Empty);

        Assert.That(error.ToString(), Does.Contain("Unknown option"));
    }

    [Test]
    public void Run_FileNotFound_Returns1()
    {
        var input = new StringReader("");
        var output = new StringWriter();
        var error = new StringWriter();


        int exitCode = App.Run(new[] { "no_such_file.txt" }, input, output, error);

        Assert.That(exitCode, Is.EqualTo(1));


        Assert.That(output.ToString(), Is.Empty);

        Assert.That(error.ToString(), Does.Contain("File not found"));
    }
}