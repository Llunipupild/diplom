using Core.Repository;
using SaveWords.Model;

namespace SaveWords.Repositroy
{
    public class SaveNewWordRepository : PlayerPrefsRepository<NewWordsModel>
    {
        public SaveNewWordRepository() : base("newWords")
        {
        }
    }
}