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
    internal class GetShikimoriFactory : AnimeFactory
    {
        internal override async Task GetAnime(AnimeBoxItemControl anime)
        {
            GetShikimori getShikimori = new GetShikimori(anime);
            await getShikimori.Display();
        }
    }
}
