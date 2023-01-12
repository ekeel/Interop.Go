using System;
using System.Reflection;
using Ekeel.Interop.Stdlib;
using HandlebarsDotNet;

namespace Ekeel.Interop.Go;

public class Generator
{
    public static string GenerateProject()
    {
        var rawTemplate = EmbeddedResource.Get<string>(Assembly.GetExecutingAssembly(), "Ekeel.Interop.GoLang.Resources.Templates.CSProj.tmpl");
        var template = Handlebars.Compile(rawTemplate);
        var renderedTemplate = template(null);

        return renderedTemplate;
    }

    public static string GenerateStaticClass(Descriptor descriptor)
    {
        var rawTemplate = EmbeddedResource.Get<string>(Assembly.GetExecutingAssembly(), "Ekeel.Interop.GoLang.Resources.Templates.StaticClass.tmpl");
        var template = Handlebars.Compile(rawTemplate);
        var renderedTemplate = template(descriptor);

        return renderedTemplate;
    }
}