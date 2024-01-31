using System.Collections.Generic;

namespace SaveWords.Model
{
    public class NewWordsModel
    {
        private List<NewWords> _newWords;
        
        public NewWordsModel()
        {
            _newWords = new List<NewWords>();
        }

        public void AddNewWord(NewWords newWords)
        {
            if (_newWords.Contains(newWords)) {
                return;
            }
            
            _newWords.Add(newWords);
        }

        public List<NewWords> NewWordsList
        {
            get => _newWords;
        }
    }
}