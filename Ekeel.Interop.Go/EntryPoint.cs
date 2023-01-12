using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekeel.Interop.Go;

public class EntryPoint
    {
        [JsonProperty("dll")]
        public string? DLL { get; set; }

        [JsonProperty("signature")]
        public string? Signature { get; set; }
    }
