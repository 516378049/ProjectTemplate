using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Common
{
    [Serializable]
    public class UploadResult
    {
        public string state { get; set; }
        public string url { get; set; }
        public string msg { get; set; }
    }
}
