using Minimarket.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Core.CustomEntities
{
    public class SummarizeUserTypePagination : PaginationQueryFilter
    {
        public string? UserType { get; set; }
        public int? UsersCount { get; set; }
    }
}
