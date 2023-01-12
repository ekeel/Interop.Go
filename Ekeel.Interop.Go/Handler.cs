using System;
using System.Diagnostics;
using System.Runtime.Loader;

namespace Ekeel.Interop.Go;

/// <summary>
/// Class <c>Handler</c> handles the import of a Go library.
/// </summary>
public class Handler
{
    private string _uuid;
    private string _projectDirectory;
    private string _assemblyPath;
    private Descriptor _descriptor;

    public Descriptor Descriptor { get { return _descriptor; } }
    public string UUID { get { return _uuid; } }
    public string AssemblyPath { get { return _assemblyPath; } }
    public string ProjectDirectory { get { return _projectDirectory; } }

    public Handler(string descriptorFile)
    {
        _uuid = Guid.NewGuid().ToString();
        _descriptor = Descriptor.LoadDescriptor(descriptorFile);
    }

    public void CompileLibrary()
    {
        GenerateProject();
        Compile();
    }

    public object GetClassInstance()
    {
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(_assemblyPath);
        var t = $"{_descriptor.Namespace}.{_descriptor.Class}";
        var type = assembly.GetType($"{_descriptor.Namespace}.{_descriptor.Class}");
        var instance = Activator.CreateInstance(type);

        return instance;
    }

    public Type? GetType()
    {
        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(_assemblyPath);
        var t = $"{_descriptor.Namespace}.{_descriptor.Class}";
        var type = assembly.GetType($"{_descriptor.Namespace}.{_descriptor.Class}");

        return type;
    }

    private void GenerateProject()
    {
        var tempDir = Path.GetTempPath();
        var projectDir = Path.Combine(tempDir, UUID);
        var csprojPath = Path.Combine(projectDir, $"{_descriptor.Class}.csproj");
        var classPath = Path.Combine(projectDir, $"{_descriptor.Class}.cs");
        var assemblyPath = Path.Join(projectDir, "bin", "Release", "net6.0", $"{_descriptor.Class}.dll");

        var csprojStr = Generator.GenerateProject();
        var classStr = Generator.GenerateStaticClass(_descriptor);

        if (!Directory.Exists(projectDir))
            Directory.CreateDirectory(projectDir);

        File.WriteAllText(csprojPath, csprojStr);
        File.WriteAllText(classPath, classStr);

        _projectDirectory = projectDir;
        _assemblyPath = assemblyPath;
    }

    private void Compile()
    {
        var compileProc = new Process();
        compileProc.StartInfo.WorkingDirectory = ProjectDirectory;
        compileProc.StartInfo.FileName = "dotnet";
        compileProc.StartInfo.Arguments = "build -c Release";
        compileProc.StartInfo.CreateNoWindow = true;
        compileProc.StartInfo.UseShellExecute = false;
        compileProc.StartInfo.RedirectStandardOutput = true;
        compileProc.StartInfo.RedirectStandardError = true;

        compileProc.ErrorDataReceived += (sender, data) =>
        {
            throw new Exception(data.Data);
        };

        compileProc.Start();
        compileProc.WaitForExit();
    }
}