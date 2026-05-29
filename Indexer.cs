using System.Collections.Generic;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly Dictionary<string, Dictionary<int, List<int>>> index = new();
    private readonly Dictionary<int, string> documents = new();

    public void Add(int id, string documentText)
    {
        Remove(id);

        documentText ??= string.Empty;
        documents[id] = documentText;

        AddWordsFromDocument(id, documentText);
    }

    public List<int> GetIds(string word)
    {
        if (word == null)
            return new List<int>();

        if (!index.TryGetValue(word, out var documentsById))
            return new List<int>();

        return new List<int>(documentsById.Keys);
    }

    public List<int> GetPositions(int id, string word)
    {
        if (word == null)
            return new List<int>();

        if (!index.TryGetValue(word, out var documentsById))
            return new List<int>();

        if (!documentsById.TryGetValue(id, out var positions))
            return new List<int>();

        return new List<int>(positions);
    }

    public void Remove(int id)
    {
        if (!documents.TryGetValue(id, out var documentText))
            return;

        RemoveWordsFromDocument(id, documentText);
        documents.Remove(id);
    }

    private void AddWordsFromDocument(int id, string documentText)
    {
        foreach (var wordInfo in GetWords(documentText))
        {
            if (!index.TryGetValue(wordInfo.Word, out var documentsById))
            {
                documentsById = new Dictionary<int, List<int>>();
                index[wordInfo.Word] = documentsById;
            }

            if (!documentsById.TryGetValue(id, out var positions))
            {
                positions = new List<int>();
                documentsById[id] = positions;
            }

            positions.Add(wordInfo.Position);
        }
    }

    private void RemoveWordsFromDocument(int id, string documentText)
    {
        foreach (var wordInfo in GetWords(documentText))
        {
            if (!index.TryGetValue(wordInfo.Word, out var documentsById))
                continue;

            documentsById.Remove(id);

            if (documentsById.Count == 0)
                index.Remove(wordInfo.Word);
        }
    }

    private static IEnumerable<WordInfo> GetWords(string documentText)
    {
        var wordStart = -1;

        for (var i = 0; i <= documentText.Length; i++)
        {
            var isEndOfText = i == documentText.Length;
            var isSeparator = isEndOfText || IsSeparator(documentText[i]);

            if (isSeparator)
            {
                if (wordStart != -1)
                {
                    var word = documentText.Substring(wordStart, i - wordStart);
                    yield return new WordInfo(word, wordStart);
                    wordStart = -1;
                }
            }
            else if (wordStart == -1)
            {
                wordStart = i;
            }
        }
    }

    private static bool IsSeparator(char character)
    {
        return character is ' ' or '.' or ',' or '!' or '?' or ':' or '-' or '\r' or '\n';
    }

    private readonly record struct WordInfo(string Word, int Position);
}
