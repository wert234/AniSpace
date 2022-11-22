using AniSpace.Infructuctre.UserControls.AnimeGaner;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Linq;

namespace AniSpace.Infructuctre.LinqExtensions
{
    public static class ConvertExtension
    {
        //remove from a specific sign to the end of the line
        public static string ConvertToSearchName(this string Input, char sign)
        {
            Input = Regex.Replace(Input, @"Part\s[1-9]", " ");
            Input = Regex.Replace(Input, @"Movie\s[1-9]", " ");
            Input = Regex.Replace(Input, @"Часть\s[1-9]", " ");
            Input = Regex.Replace(Input, @"Фильм\s[1-9]", " ");
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == sign)
                    return Input.Remove(i); 
            }
            Input = Regex.Replace(Input, "[!\"#$%&'()*+,-./:;<=>?@\\[\\]^_`{|}~|||∬]", " ");
            return Regex.Replace(Input, " ", "+");
        }

        public static string ConvertToString(this ComboBoxItem Input, string Default)
        {
            if (Input is null)
            {
                Input = new ComboBoxItem();
                Input.Content = Default;
            }
            return Input.Content.ToString();
        }

        public static string GanerToString(this AnimeGaner Input, string Default)
        {
            if (Input is null || Input.GanerSelected is false) return Default;
            return Input.GanerName;
        }

        public static string GanerToShikiGaner(this string Input, string Dafalt)
        {
            string ShikimoriGaners = "";
            Dictionary<string, string> ganers = new Dictionary<string, string>
            {
                {"Комедия", "4-Comedy" },
                {"Романтика", "22-Romance" },
                {"Школа", "23-School" },
                {"Боевые искусства", "17-Martial-Arts" },
                {"Детектив", "7-Mystery" },
                {"Драма", "8-Drama" },
                {"Меха", "18-Mecha" },
                {"Музыка", "19-Music" },
                {"Повседневность", "36-Slice-of-Life" },
                {"Приключения", "2-Adventure" },
                {"Спорт", "30-Sports" },
                {"Ужасы", "14-Horror" },
                {"Фантастика", "24-Sci-Fi" },
                {"Фэнтези", "10-Fantasy" },
                {"Экшен", "1-Action" },
                {"Этти", "9-Ecchi" },
            };
            if (Input is null) return Dafalt;
            foreach (var item in Input.Split(", "))
            {
                ShikimoriGaners = ShikimoriGaners + "," + ganers[item];
            }

            return ShikimoriGaners.Remove(0, 1);
        }

        public static string GanerToAnimeGoGaner(this string Input, string Dafalt)
        {
            string ShikimoriGaners = "";
            Dictionary<string, string> ganers = new Dictionary<string, string>
            {
                {"Комедия", "comedy" },
                {"Романтика", "romance" },
                {"Школа", "school" },
                {"Боевые искусства", "martial-arts" },
                {"Детектив", "mystery" },
                {"Драма", "drama" },
                {"Меха", "mecha" },
                {"Музыка", "music" },
                {"Повседневность", "slice-of-Life" },
                {"Приключения", "adventure" },
                {"Спорт", "sports" },
                {"Ужасы", "horror" },
                {"Фантастика", "sci-Fi" },
                {"Фэнтези", "fantasy" },
                {"Экшен", "action" },
                {"Этти", "ecchi" },
            };
            if (Input is null) return Dafalt;
            foreach (var item in Input.Split(", "))
            {
                ShikimoriGaners = ShikimoriGaners + "-or-" + ganers[item];
            }

            return ShikimoriGaners.Remove(0, 1);
        }
    }
}
