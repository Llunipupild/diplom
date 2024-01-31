using System;

namespace Descriptors.Enumer
{
    public enum WordType
    {
        ANIMALS,
        VEGETABLES_AND_FRUITS,
        POPULAR_100_WORDS,
        ADJECTIVE_TIME,
        ADJECTIVE_HUMAN,
        SCIENCE_AND_ART,
        MATHEMATIC,
        OTHER
    }
    
    public static class WordTypeExtensions
    {
        public static WordType ValueOf(string name)
        {
            return (WordType) Enum.Parse(typeof(WordType), name, true);
        }

        public static string GetName(this WordType value)
        {
            string? result = Enum.GetName(typeof(WordType), value);
            return result != null ? result.ToLower() : "";
        }
    }
}