using buildingBlock.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Application.Exceptions
{
    internal class ProductOutOfStock : NotFoundException
    {
        public ProductOutOfStock() : base("Ordered Item out of stock.")
        {
        }
    }
}
