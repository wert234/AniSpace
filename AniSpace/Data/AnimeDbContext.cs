﻿using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Data
{
    public class AnimeDbContext : DbContext
    {
        public AnimeDbContext()
            :base(){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AniSpace;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public DbSet<AnimeBase> AnimeBoxItemControls { get; set; }

    }
}
