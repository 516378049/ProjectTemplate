using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.orderMeal
{
    public class CartOrder
    {
        public CartOrder(int id, string deskNumber, int menuId, int count)
        {
            this.id = id;
            this.deskNumber = deskNumber;
            this.menuId = menuId;
            this.count = count;
        }
        public int id { get; set; }
        public string deskNumber { get; set; }
        public int menuId { get; set; }
        public int count { get; set; }
    }
}
