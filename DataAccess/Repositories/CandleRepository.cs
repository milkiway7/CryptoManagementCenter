using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CandleRepository : BaseRepository, ICandleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CandleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCandleAsync(CandleModel candle)
        {
            _dbContext.Candles.Add(candle);
            await SaveAsync();
        }
    }
}
