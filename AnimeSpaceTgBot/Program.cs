using System.ComponentModel.DataAnnotations;
using System.Threading;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using AniSpaceLib.AnimeClient;
using AniSpaceLib.AnimeDataBase;
using AniSpaceLib.AnimeDataBase.Models;
using AniSpaceLib.AnimeParser;



AnimeClient animeClient = new AnimeClient("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AnimeDataBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

//var ListAnimeGO = new List<Anime>();

//var Parser = new AnimeGOParser();

//for (int i = 1; i <= 118; i++)
//    ListAnimeGO.AddRange(await Parser.Parse(i));

//animeClient.AddAnime(ListAnimeGO);

animeClient.AddAnime(new Anime() { Description = "dd", Genres = "dd", Name = "ddd", OriginalName = "dd", Preview = "dd", Rating = "", Release = "", Restriction = "dd", VersionId = 1 });

var d = animeClient.GetAnime().Result;
Console.WriteLine();
//var client = new TelegramBotClient("6345633945:AAH_2oCddhoeFrfxmVTeHtm7MhwyVXDyMeA");
//client.StartReceiving(Update, Error);

//Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
//{
//    throw new NotImplementedException();
//}

//Console.ReadLine();

//async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
//{
//   var message = update.Message;
//    if (message.Text != null)
//    {
//        if(message.Text == "Топ 3")
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                await botClient.SendPhotoAsync(
//                                   chatId: update.Message.Chat.Id,
//                                   photo: InputFile.FromStream(System.IO.File.Open(@"C:\Users\Vladlen\Pictures\аниме.jpg", FileMode.Open)),
//                                   caption: $"<b>Судзума закрывающия двери - {i}</b>. <i>переходи и</i>: <a href=\"https://pixabay.com\">смотри онлайн</a>",
//                                   parseMode: ParseMode.Html);
//            }
//            return;
//        }
//   }
//}

