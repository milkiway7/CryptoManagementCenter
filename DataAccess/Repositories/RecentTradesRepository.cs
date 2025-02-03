using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RecentTradesRepository : BaseRepository, IRecentTradesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RecentTradesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRecentTradesAsync(RecentTradesModel recentTrades)
        {
            _dbContext.RecentTrades.Add(recentTrades);
            await SaveAsync();
        }
    }
}
