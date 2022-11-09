using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace AniSpace.Infructuctre.LinqExtensions
{
    internal static class CompareExtensions
    {
        public static int CompareName(this string Input, string Comparable)
            => Input.Except(Comparable).ToList().Count;

        public static List<HtmlNode> CompareMatches(this List<HtmlNode> Input, string Comparable)
        {
            List<HtmlNode> Output = new List<HtmlNode>(1);
            Output.Add(Input.First());
            foreach (HtmlNode item in Input.Skip(1))
            {
                if(item.InnerText.ConvertToSearchName(' ').CompareName(Comparable) < Output.First().InnerText.CompareName(Comparable))
                    Output[Output.Count - 1] = item;
            }
            return Output;
        }
    }
}
