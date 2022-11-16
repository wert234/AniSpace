using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models.FactoryDomins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models.Factory
{
    internal class AniMangFactory : AnimeFactory
    {
        internal override async Task GetAnime(AnimeBoxItemControl anime)
        {
            AniMang aniMang = new AniMang(anime);
            await aniMang.HowToGetAsync();
        }

        internal override async Task GetListAnime(string page, string limit, string season, string ganers = null, string rating = null)
        {
            AniMang aniMang = new AniMang(page, limit, season, ganers, rating);
            await aniMang.GetListAsync();
        }

        internal override async Task SearchAnime(AnimeBoxItemControl anime)
        {
            AniMang aniMang = new AniMang(anime);
            await aniMang.SearchAsync();
        }
    }
}
