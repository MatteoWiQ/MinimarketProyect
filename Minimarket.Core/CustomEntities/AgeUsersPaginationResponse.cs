using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class AgeUsersPaginationResponse : PaginationQueryFilter
    {
        public string? UserType { get; set; }
        public int? Age { get; set; }
    }
}
