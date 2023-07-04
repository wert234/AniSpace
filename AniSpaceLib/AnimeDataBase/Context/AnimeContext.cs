using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniSpaceLib.AnimeDataBase.Models;
using Microsoft.EntityFrameworkCore;
using Version = AniSpaceLib.AnimeDataBase.Models.Version;

namespace AniSpaceLib.AnimeDataBase.Context
{
    internal class AnimeContext : DbContext
    {
        private readonly string _DataBasePath;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                            => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AnimeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anime>()
                 .HasOne(a => a.Version)
                 .WithMany(v => v.Animes)
                 .HasForeignKey(a => a.VersionId);
        }

        public AnimeContext() : base() { }
        public AnimeContext(string dataBasePath) : base()  => _DataBasePath = dataBasePath;

        public DbSet<Anime> Animes { get; set; }
        public DbSet<Version> Versions { get; set; }
    }
}
