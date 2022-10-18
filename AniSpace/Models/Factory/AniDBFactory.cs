using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models.FactoryDomins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models.Factory
{
    internal class AniDBFactory : AnimeFactory
    {
        internal override async Task GetAnime(AnimeBoxItemControl anime)
        {
            AniDB aniDB = new AniDB(anime);
            
            await aniDB.Display();
        }

        internal override Task GetListAnime(string page, string limit, string season, string rating = null)
        {
            throw new NotImplementedException();
        }

        internal override async Task SearchAnime(AnimeBoxItemControl anime)
        {
            AniDB searchAniDB = new AniDB(anime);
            await searchAniDB.Search();
        }
    }
}
