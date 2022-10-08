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
    }
}
