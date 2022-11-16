namespace ShikimoriTests
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
            StudioName = "Shikimori";
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public async Task SearchAsync_AnimeReture()
        {
            #region Arrange  

            string expected = "8.66";

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
        public async Task GetAsyncAnime_AnimeReture()
        {
            #region Arrange  

            string expected = "8.66";

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