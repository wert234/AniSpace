using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Data
{
    internal class AnimeDbContext : DbContext
    {
        public AnimeDbContext()
            :base(){}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AnimeDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public DbSet<AnimeDbItem> AnimeBoxItemControls { get; set; }

    }
}
