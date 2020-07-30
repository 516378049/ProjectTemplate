using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EF.EF_ExAttr
{
    public class foodsEx:foods
    {
        public List<ratings> ratings { get; set; }
    }
}
