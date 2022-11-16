namespace AniMangTests
{
    public class Tests
    {
        private static AnimeBoxItemControl _Anime;
        private static string StudioName;

        [SetUp]
        public void Setup()
        {
            AnimeControler._AnimeListBoxItems = new ObservableCollection<UserControl>();
            _Anime = new AnimeBoxItemControl();
            _Anime.AnimeName = "Вайолет";
            _Anime.AnimeAge = "2018";
            StudioName = "AniMang";
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public async Task SearchAsync_AnimeReture()
        {
            #region Arrange  

            string expected = "9.26";

            #endregion

            #region Act
         
            await AnimeControler.SearchAsync(_Anime.AnimeName, _Anime.AnimeAge, StudioName, "Драма");

            #endregion

            #region Assert

            Assert.That(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeRaiting, Is.EqualTo(expected));

            #endregion
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public void Create_CreateAnimeReture()
        {
            #region Arrange  

            var anime = new AnimeBoxItemControl();
            anime.AnimeName = "Вайолет";
            anime.AnimeAge = "2018";
            anime.AnimeTegs = "Драма, Фэнтези";
            anime.AnimeRaiting = "9.26";
            anime.AnimeImage = (ImageSource)new ImageSourceConverter().ConvertFrom("https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg");
            anime.AnimeOrigName = "Violet Evergarden";

            #endregion

            #region Act

            AnimeControler.Create("Вайолет", "Violet Evergarden", "9.26", "https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg", "2018", "Драма, Фэнтези");

            #endregion

            #region Assert

            Assert.That(anime.AnimeRaiting, Is.EqualTo(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeRaiting));
            Assert.That(anime.AnimeName, Is.EqualTo(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeName));
            Assert.That(anime.AnimeOrigName, Is.EqualTo(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeOrigName));
            Assert.That(anime.AnimeImage.Width, Is.EqualTo(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeImage.Width));
            Assert.That(anime.AnimeTegs, Is.EqualTo(((AnimeBoxItemControl)AnimeControler._AnimeListBoxItems[0]).AnimeTegs));
          
            #endregion
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public async Task GetAsyncAnime_AnimeReture()
        {
            #region Arrange  

            string expected = "9.26";

            #endregion

            #region Act

            await AnimeControler.GetAsync(StudioName, _Anime);

            #endregion

            #region Assert

            Assert.That(_Anime.AnimeRaiting, Is.EqualTo(expected));

            #endregion
        }
    }
}