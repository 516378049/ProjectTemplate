using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAPI.Core.Models
{
    public class DynamicResult : ModelBase
    {
        public string RetCode { get; set; }
        public string RetMsg { get; set; }
    }
}
