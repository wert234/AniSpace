using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models.FactoryDomins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AniSpace.Models.Factory
{
    internal class ShikimoriFactory : AnimeFactory
    {
        internal override async Task GetAnime(AnimeBoxItemControl anime)
        {
            Shikimori shikimori = new Shikimori(anime);
            await shikimori.HowToGet();
        }

        internal override async Task GetListAnime(string page, string limit, string season, string rating = null)
        {
            Shikimori shikimori = new Shikimori(page, limit, season, rating);
            await shikimori.GetList();
        }

        internal override async Task SearchAnime(AnimeBoxItemControl anime)
        {
            Shikimori shikimori = new Shikimori(anime);
            await shikimori.Search();
        }
    }
}
