using System;
using Newtonsoft.Json;

namespace Ekeel.Interop.Go;

public class Descriptor
{
    [JsonProperty("namespace")]
    public string? Namespace { get; set; }

    [JsonProperty("class")]
    public string? Class { get; set; }

    [JsonProperty("entry_points")]
    public IEnumerable<EntryPoint>? EntryPoints { get; set; }

    [JsonProperty("references")]
    public IEnumerable<string>? References { get; set; }

    public static Descriptor LoadDescriptor(string descriptorFile)
    {
        return JsonConvert.DeserializeObject<Descriptor>(File.ReadAllText(descriptorFile));
    }
}