using AniSpace.Data;
using System.Linq;

namespace DateBaseTests
{
    public class Tests
    {
        private AnimeDbContext _AnimeDb;
        private AnimeBase _Anime;

        [SetUp]
        public void Setup()
        {
            AnimeControler._AnimeListBoxItems = new ObservableCollection<UserControl>();
            _AnimeDb = new AnimeDbContext();
            _Anime = new AnimeBase();
            _Anime.AnimeName = "Вайолет";
            _Anime.AnimeAge = "2018";
            _Anime.AnimeTegs = "Драма, Фэнтези";
            _Anime.AnimeRating = "9.26";
            _Anime.AnimeImage = "https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg";
            _Anime.AnimeOrigName = "Violet Evergarden";
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public async Task SaveAsync_AnimeRetureInDB()
        {
            #region Arrange



            #endregion

            #region Act

            await AnimeDbControler.SaveAsync(_Anime.AnimeRating, _Anime.AnimeName, _Anime.AnimeOrigName, "https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg", _Anime.AnimeAge, _Anime.AnimeTegs);

            #endregion

            #region Assert

            Assert.That(_Anime, Is.EqualTo(_AnimeDb.AnimeBoxItemControls.First()));

            #endregion

        }
    }
}