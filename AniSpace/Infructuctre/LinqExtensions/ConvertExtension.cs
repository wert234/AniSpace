using System.Text.RegularExpressions;

namespace AniSpace.Infructuctre.LinqExtensions
{
    public static class ConvertExtension
    {
        //remove from a specific sign to the end of the line
        public static string ConvertToSearchName(this string Input, char sign)
        {
            Input = Regex.Replace(Input, @"Part\s[1-9]", " ");
            Input = Regex.Replace(Input, @"Movie\s[1-9]", " ");
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == sign)
                    return Input.Remove(i); 
            }
            Input = Regex.Replace(Input, "[!\"#$%&'()*+,-./:;<=>?@\\[\\]^_`{|}~|||∬]", " ");
            return Regex.Replace(Input, " ", "+");
        }
    }
}
