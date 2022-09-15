using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
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
        public AnimeDbContext(DbContextOptions options)
            :base(options){}

        public DbSet<AnimeBoxItemControl> AnimeBoxItemControls { get; set; }
    }
}
