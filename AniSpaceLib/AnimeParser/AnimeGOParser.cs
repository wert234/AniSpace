using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

using AniSpaceLib.AnimeDataBase.Models;
using Azure;
using System.Diagnostics.Metrics;

namespace AniSpaceLib.AnimeParser
{
    public class AnimeGOParser
    {
        private string _Url = @"https://animego.org/anime?sort=a.createdAt&direction=desc&type=animes&page=";
        private HtmlDocument _Document = new HtmlDocument();
        private List<string> _AnimeHtml = new List<string>();

        public async Task<List<Anime>> Parse(int Page)
        {
            var listAnime = new List<Anime>();

            using (var client = new HttpClient())
            {
                var response =  await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, new Uri(_Url + Page.ToString())));
                _Document.LoadHtml(await response.Content.ReadAsStringAsync());
            }

            if (_Document.DocumentNode.SelectNodes("//div[@class='col-12']") is null) return null;

            foreach (var animeHtml in _Document.DocumentNode.SelectNodes("//div[@class='col-12']"))
                _AnimeHtml.Add(animeHtml.InnerHtml);

            foreach (var animeHtml in _AnimeHtml)
                listAnime.Add(HtmlToAnime(animeHtml));

            _AnimeHtml.Clear();
            return listAnime;
        }

        private Anime HtmlToAnime(string animeHtml)
        {
            var anime = new Anime();
            _Document.LoadHtml(animeHtml);
            if(_Document?.DocumentNode?.SelectSingleNode("//div[@class='text-gray-dark-6 small mb-2']")?.InnerText != null)
                anime.OriginalName = _Document.DocumentNode.SelectSingleNode("//div[@class='text-gray-dark-6 small mb-2']").InnerText;
            anime.Preview = _Document.DocumentNode.SelectSingleNode("//div[@class='anime-list-lazy lazy']").Attributes["data-original"].Value;
            anime.Name = _Document.DocumentNode.SelectSingleNode("//div[@class='h5 font-weight-normal mb-1']").InnerText;
            if (_Document?.DocumentNode?.SelectSingleNode("//span[@class='anime-genre d-none d-sm-inline']")?.InnerText != null)
                anime.Genres = _Document.DocumentNode.SelectSingleNode("//span[@class='anime-genre d-none d-sm-inline']").InnerText;
            anime.Release = _Document.DocumentNode.SelectSingleNode("//span[@class='anime-year mb-2']").InnerText;
            if (_Document?.DocumentNode?.SelectSingleNode("//div[@class='p-rate-flag__text']")?.InnerText != null)
                anime.Rating = _Document.DocumentNode.SelectSingleNode("//div[@class='p-rate-flag__text']").InnerText;
            anime.Restriction = "Не указан";
            anime.VersionId = 1;
            anime.Description = _Document.DocumentNode.SelectSingleNode("//div[@class='description d-none d-sm-block']").InnerText;

            return anime;
        }
    }
}
