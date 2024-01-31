using Core.Repository;
using Decks.Model;

namespace Decks.Repository
{
    public class DecksRepository : PlayerPrefsRepository<DecksModel>
    {
        public DecksRepository() : base("decksRepository")
        {
        }
    }
}