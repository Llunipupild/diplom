using System;
using Decks.Enum;
using UI.Cards.Controller;
using UnityEngine;

namespace Decks.Controller
{
    public class Deck : MonoBehaviour
    {
        [SerializeField]
        private DeckNumber deckNumberNumber;

        public Action<DeckNumber> onSelectedDeck;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<CardController>() != null) {
                onSelectedDeck?.Invoke(deckNumberNumber);
            }
        }
    }
}