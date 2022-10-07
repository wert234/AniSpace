using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using AniSpace.Models.FactoryDomins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models.Factory
{
    internal class GetAniMangFactory : AnimeFactory
    {
        internal override void GetAnime(AnimeBoxItemControl anime)
        {
            GetAniMang getAniMang = new GetAniMang(anime);
            getAniMang.Display();
        }
    }
}
