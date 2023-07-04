using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpaceLib.AnimeDataBase.Models
{
    public class Anime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string? OriginalName { get; set; }
        public string Preview { get; set; }
        public string Genres { get; set; }
        public string Restriction { get; set; }
        public string Release { get; set; }
        public string? Rating { get; set; }
        public string Description { get; set; }

        [ForeignKey("Version")]
        public int VersionId { get; set; }
        public Version Version { get; set; }
    }
}
