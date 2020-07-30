using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EF.EF_ExAttr
{
    public class sellersEx :sellers
    {
        public List<supports> supports { get; set; }
    }
}
