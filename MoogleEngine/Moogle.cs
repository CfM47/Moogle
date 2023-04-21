namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query , TfIdfDirectory Tf)  //aqui tuve que pasar como parametro el objeto que contiene las cosas que solo se hacen al principio
    {        
        string suggestion = query;

        string[] queryWords = WordOperator.GetWords(query);
        foreach(string word in queryWords)
        {
            //verifica si hay palabras de la query que no aparecen y las cambia por la palabra posible
            if (!Tf.WordsIndexes.ContainsKey(word))
            {
                var PosibleWords = Tf.WordsIndexes.Keys.ToList();
                string sugestedWord = WordOperator.Suggestion(word, PosibleWords);
                suggestion = suggestion.Replace(word, sugestedWord);
            }
        }
        
        //obtienen los resultados
        List<QueryDimension> suggestionVector = Tf.GenerateQueryVector(suggestion);
        string[] suggestionWords = WordOperator.GetWords(suggestion);
        List<SearchItem> items = Tf.MatchToDataBase(suggestionVector, suggestionWords);

        if (suggestion == query) //verifica si fue necesario sugerir algo
            suggestion = "";

        return new SearchResult(items, suggestion);
    }
}
