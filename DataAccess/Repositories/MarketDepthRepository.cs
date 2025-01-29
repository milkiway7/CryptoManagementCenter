using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MarketDepthRepository : BaseRepository, IMarketDepthRepository
    {
        private readonly ApplicationDbContext _context;
        public MarketDepthRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;   
        }

        public async Task AddMarketDepthAsync(MarketDepthModel marketDepth)
        {
            _context.Add(marketDepth);
            await SaveAsync();
        }
    }
}
