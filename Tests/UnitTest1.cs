using Ekeel.Interop.Go;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test()
    {
        Handler handler = new Handler("D:\\tmp\\TestDescriptor.json");
        handler.CompileLibrary();
        var tst = handler.GetType();
        tst.GetMethod("HelloWorld").Invoke(null, new[] { "TST" });
        var a = tst.GetMembers();

        var t = tst;
    }
}
