using AniSpaceLib.AnimeDataBase.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AniSpaceLib.AnimeDataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace AniSpaceLib.AnimeClient
{
    public class AnimeClient
    {
        private readonly AnimeContext _DB;
        public AnimeClient(string dataBasePath)
        {
            _DB = new AnimeContext();
        }

        public void CreateVersion(string VersionName)
        {
            _DB.Versions.Add(new AnimeDataBase.Models.Version() { Animes = new List<Anime>(), Name = VersionName });
            _DB.SaveChanges();
        }

        public async Task AddAnime(Anime anime)
        {
             var d = await  _DB.Versions.FirstAsync(v => v.Id == anime.VersionId);
            d.Animes.Add(anime);
            _DB.SaveChanges();
        }

        public async Task AddAnime(List<Anime> animeList)
        {
            _DB.Animes.AddRange(animeList);
            _DB.SaveChanges();
        }

        public async Task<ICollection<Anime>> GetAnime()
        {
            return (await _DB.Versions.FirstAsync(v => v.Name == "AnimeGO")).Animes;
        }

    }
}
