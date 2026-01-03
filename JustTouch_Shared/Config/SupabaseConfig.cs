using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTouch_Shared.Config
{
    public class SupabaseConfig
    {
        public string Url { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string StorageHost { get; set; } = string.Empty;
        public string AuthHost { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
