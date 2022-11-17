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
            AnimeDbControler.LoadAnime(new ObservableCollection<AnimeBase>(), _AnimeDb, new ObservableCollection<UserControl>());
            await AnimeDbControler.SaveAsync(_Anime.AnimeRating, _Anime.AnimeName, _Anime.AnimeOrigName, "https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg", _Anime.AnimeAge, _Anime.AnimeTegs);

            #endregion

            #region Assert

            Assert.That(_AnimeDb.AnimeBoxItemControls.Any(x => x.AnimeName == _Anime.AnimeName));
            
            #endregion

        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public async Task DeleteAsync_NullRetureInDB()
        {
            #region Arrange

            if (_AnimeDb.AnimeBoxItemControls.Where(x => x.AnimeName == _Anime.AnimeName).Count() == 0)
            {
                AnimeDbControler.LoadAnime(new ObservableCollection<AnimeBase>(), _AnimeDb, new ObservableCollection<UserControl>());
                await AnimeDbControler.SaveAsync(_Anime.AnimeRating, _Anime.AnimeName, _Anime.AnimeOrigName, "https://animang.one/wp-content/uploads/2018/03/vajolet-evergarden.jpg", _Anime.AnimeAge, _Anime.AnimeTegs);
            }


            #endregion

            #region Act
            AnimeDbControler.LoadAnime(new ObservableCollection<AnimeBase>(), _AnimeDb, new ObservableCollection<UserControl>());
            await AnimeDbControler.DelteByNameAsync(_Anime.AnimeName);

            #endregion

            #region Assert

            Assert.That(_AnimeDb.AnimeBoxItemControls.Where(x => x.AnimeName == _Anime.AnimeName).Count, Is.EqualTo(0));

            #endregion

        }
    }
}