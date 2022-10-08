using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models.FactoryDomins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models.Factory
{
    internal class GetAniDBFactory : AnimeFactory
    {
        internal override async Task GetAnime(AnimeBoxItemControl anime)
        {
            GetAniDB getAniDB = new GetAniDB(anime);
            await getAniDB.Display();
        }
    }
}
