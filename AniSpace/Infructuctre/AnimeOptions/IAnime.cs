using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace
{
    internal interface IAnime
    {
        public int Id { get; set; }
        public string AnimeName { get; set; }
        public string AnimeRating { get; set; }
        public string AnimeAge{ get; set; }
        public string AnimeImage { get; set; }
        public string AnimeTegs { get; set; }
    }
}
