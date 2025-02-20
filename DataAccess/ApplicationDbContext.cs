﻿using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}
        public DbSet<UserModel> Users { get; set; }
        public DbSet<NewProjectModel> NewProjects { get; set; }
        public DbSet<CandleModel> Candles { get; set; }
        public DbSet<MarketDepthModel> MarketDepth { get; set; }
        public DbSet<RecentTradesModel> RecentTrades { get; set; }
    }
}
