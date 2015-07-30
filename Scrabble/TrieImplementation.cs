using System;

namespace Scrabble
{
    public class TrieImplementation
    {
        public TrieImplementation()
        {

        }

        public void constructTrie(Trie trie, WordList wordList)
        {
            foreach (var item in wordList.DictionaryOfScrabbleWords)
            {
                trie.Add(item.Value.ToString());
            }
        }
    }
}
