using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AniSpace.Models
{
    internal class AnimeDbItem
    {
        public int Id { get; set; }
        public string AnimeName { get; set; }
        public string AnimeRating { get; set; }
        public string AnimeImage { get; set; }
    }
}
