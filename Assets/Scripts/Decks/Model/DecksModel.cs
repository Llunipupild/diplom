using System.Collections.Generic;

namespace Decks.Model
{
    public class DecksModel
    {
        private List<string> _decks1;
        private List<string> _decks2;
        private List<string> _decks3;

        public DecksModel()
        {
            _decks1 = new List<string>();
            _decks2 = new List<string>();
            _decks3 = new List<string>();
        }

        public void AddWordInDecks1(string word)
        {
            if (_decks2.Contains(word)) {
                RemoveWordFromDecks2(word);
            }
            if (_decks3.Contains(word)) {
                RemoveWordFromDecks3(word);
            }
            if (_decks1.Contains(word)) {
                return;
            }
            
            _decks1.Add(word);
        }
        
        public void AddWordInDecks2(string word)
        {
            if (_decks1.Contains(word)) {
                RemoveWordFromDecks1(word);
            }
            if (_decks3.Contains(word)) {
                RemoveWordFromDecks3(word);
            }
            if (_decks2.Contains(word)) {
                return;
            }
            
            _decks2.Add(word);
        }
        
        public void AddWordInDecks3(string word)
        {
            if (_decks1.Contains(word)) {
                RemoveWordFromDecks1(word);
            }
            if (_decks2.Contains(word)) {
                RemoveWordFromDecks2(word);
            }
            if (_decks3.Contains(word)) {
                return;
            }
            
            _decks3.Add(word);
        }

        public void RemoveWordFromDecks1(string word)
        {
            if (!_decks1.Contains(word)) {
                return;
            }

            _decks1.Remove(word);
        }
        
        public void RemoveWordFromDecks2(string word)
        {
            if (!_decks2.Contains(word)) {
                return;
            }

            _decks2.Remove(word);
        }
        
        public void RemoveWordFromDecks3(string word)
        {
            if (!_decks3.Contains(word)) {
                return;
            }

            _decks3.Remove(word);
        }

        public bool ExistOnDecks(string word)
        {
            return Decks1.Contains(word) || Decks2.Contains(word) || Decks3.Contains(word);
        }
        
        public List<string> Decks1
        {
            get => _decks1;
        }

        public List<string> Decks2
        {
            get => _decks2;
        }
        public List<string> Decks3
        {
            get => _decks3;
        }
    }
}