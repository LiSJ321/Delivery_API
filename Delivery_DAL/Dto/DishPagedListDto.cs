using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery_DAL.Dto
{
    public class DishPagedListDto
    {
        public List<DishDto> Dishes { get; set; }

        public PageInfoModel Pagination { get; set; }
    }
}
