using Microsoft.EntityFrameworkCore;
using Minimarket.Core.Data.Entities;
using Minimarket.Core.Interfaces;
using Minimarket.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimarket.Infrastructure.Repositories
{
    public class SaleRepository : BaseRepository<Sale>, ISaleRepository
    {
        private readonly MinimarketContext _context;
        public SaleRepository(MinimarketContext context) : base(context)
        {
            _context = context;
        }
    }

}
