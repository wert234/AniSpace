using AniSpace.Infructuctre.UserControls.AnimeBoxItemControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Models.Factory
{
    internal abstract class AnimeFactory
    {
        internal abstract Task GetAnime(AnimeBoxItemControl anime);
        internal abstract Task SearchAnime(AnimeBoxItemControl anime);
        internal abstract Task GetListAnime(string page, string limit, string season, string ganers = null, string rating = null);
    }
}
