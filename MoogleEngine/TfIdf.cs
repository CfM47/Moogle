using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoogleEngine;
public class TfIdfDirectory 
{
    //esta clase es una estructura que guarda objetos relacionados con una base de datos de .txt
    //necesarios para el funcionamiento el moogle engine
    #region Constructors
    public TfIdfDirectory(string directoryPath)
    {
        //usar este constructor cuando se ha modificado la base de datos de .txt o si no se ha guardado en .json este objeto
        DirectoryPath = directoryPath;
        string[] documentPaths = Directory.GetFiles(DirectoryPath);
        DocumentNames = new string[documentPaths.Length];
        //esto obtiene los nombres de todos los documentos
        for (int i = 0; i < documentPaths.Length; i++)
        {
            FileInfo fileInfo = new FileInfo(documentPaths[i]);
            DocumentNames[i] = fileInfo.Name;

        }
        //crea el diccionario y todos los vectores documentos
        WordsIndexes = WordsIndexer.GetWordIndexes(documentPaths);
        DirectoryVector = DataBaseVector(DocumentNames);
        
        //guarda this en un .json
        SaveDatabaseStructure(directoryPath);
    }
    [JsonConstructor]
    public TfIdfDirectory(string directoryPath, 
                          string [] documentNames, Dictionary<string, Dictionary<string, 
                          List<int>>> wordsIndexes, List<List<double>> directoryVector)
    {
        this.DirectoryPath = directoryPath;
        this.DocumentNames = documentNames;
        this.WordsIndexes = wordsIndexes;
        this.DirectoryVector = directoryVector;
    }
    #endregion
    #region Methods
    #region Vectorial Model
    public List<SearchItem> MatchToDataBase(List<QueryDimension> queryVector, string[] queryWords)
    {
        //este metodo devuelve los 5 documentos mas relevantes

        List <SimilarityResult> Results = new List<SimilarityResult>();
        for(int i = 0; i < DirectoryVector.Count; i++)
        {
            double value = CosineSimilarity(queryVector, DirectoryVector[i]);
            SimilarityResult result = new SimilarityResult(value, i);
            Results.Add(result);
        }
        //ordena en orden decreciente los resultados
        Results.Sort();
        Results.Reverse();

        List<SearchItem> Items = new List<SearchItem>();

        //llena la lista con 5 documentos relevantes
        for (int i = 0; i < 5; i++)
        {
            if (Results[i].Value != 0)
            {
                string title = DocumentNames[Results[i].Index];
                string snippet = GetSnippet(queryWords, title);
                float score = (float)Results[i].Value;

                Items.Add(new SearchItem(title, snippet, score));
            }
        }
        return Items;
    }
    private List <double> DocumentVector(string document)
    {
        //crea los vectores, con valores de tf-idf en cada dimension, de cada documento 
        List <double> result = new List<double>();

        foreach (string word in WordsIndexes.Keys)
            result.Add(DocumentTfIdf(word, document));

        return result;
    }
    private List<List<double>> DataBaseVector(string[] documents)
    {
        //crea los vectores documento de todos los documentos de la base de datos
        List<List<double>> result = new List<List<double>>();
        for (int i = 0; i < documents.Length; i++)
        {
            List<double> documentVector = DocumentVector(documents[i]);
            result.Add(documentVector);
        }
        return result;
    }
    public List<QueryDimension> GenerateQueryVector(string query)
    {
        //este metodo devuelve el vector query
        List<QueryDimension> result = new List<QueryDimension>();

        //este bloquecito convierte la query en un arreglo de palabras simplificadas, para
        //que se pueda comprobar si estan en el diccionario de la base de datos
        char[] separators = {' ',',',';','.',':','"','\r','\n'};
        string[] splittedquery = query.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] words = WordOperator.GetWords(splittedquery);
        WordOperator.ConvertToSimpleVersion(words);

        //esto añade todas las dimensiones de el vector query
        foreach(string word in WordsIndexes.Keys)
        {
            if(words.Contains(word))        // si se encuentra la palabra en la query, calcula su valor
                result.Add(QueryTfIdf(word,query));
            else                            //... si no se encuentra ni te molestes en calcular, dale valor 0
                result.Add(new QueryDimension(0d, word, WordOperator.WordOperations.NoOperation));
        }
        return result;
    }
    public double CosineSimilarity(List<QueryDimension> vector1, List<double> vector2)
    {
        //este metodo calcula cuan similares son dos vectores, mientras mas lo sean, su valor va a ser mas cercano a 1
        double vectorProduct = 0;
        double absVector1 = 0;
        double absVector2 = 0;
        if(vector1.Count != vector2.Count) return 0;
        for(int i = 0; i < vector1.Count; i++)
        { 
            //esta if-else statement decide que hacer en depedencia de el operador de busqueda que tenga la palabra
            if(vector1[i].Operation == WordOperator.WordOperations.MustNotBe && vector2[i] != 0)
                return 0;
            else if(vector1[i].Operation == WordOperator.WordOperations.MustBe && vector2[i] == 0)
                return 0;
            else if (vector1[i].Operation == WordOperator.WordOperations.IsRelevant)
                vector1[i].TfIdfValue *=  vector1[i].Relevance + 1;

            absVector1 += Math.Pow(vector1[i].TfIdfValue, 2);
            absVector2 += Math.Pow(vector2[i], 2);
            vectorProduct += vector1[i].TfIdfValue * vector2[i];
        }
        return (absVector1* absVector2 == 0) ? 0 : vectorProduct / (Math.Sqrt(absVector1*absVector2));            
    }
    private string GetSnippet(string[] query, string documentName)
    {
        //este halla el snippet, devuelve un trozo del documento en el que aparece la palabra mas relevante que aparece en la query
        //quiero ver si lo hago para que los extremos sean palabras completas

        string result = "";
        string wordResult = "";     // palabra mas relevante (la de menos idf en la base de datos)
        foreach (string word in query)
        {
            //si la palabra es mas relevante y si aparece en el documento calcula su snippet
            if (WordsIndexes[word].ContainsKey(documentName) && (Idf(word, DocumentNames) < Idf(wordResult, DocumentNames) || wordResult ==""))
            {
                //estas dos lineas hallan la mediana de las posiciones de en las que aparece word
                int mediumPos = WordsIndexes[word][documentName].Count / 2;
                int position = WordsIndexes[word][documentName][mediumPos];

                //estas dos lineas buscan el texto en donde aparece la palabra
                string DocumentPath = DirectoryPath + "\\" + documentName;  
                string text = File.ReadAllText(DocumentPath);
                
                //40 letras pa aca 40 letras pa allá...
                for (int i = position - 40; i < position + 40; i++)
                {
                    if (i >= 0 && i < text.Length)
                        result += text[i];
                }
                wordResult = word;
            }
        }
        return result;
    }
    #endregion 
    #region tfidf calculation
    private int TfInDocument(string term, string document)
    {
        //calcula la frecuencia un termino en un documento especifico del directorio
        if (!WordsIndexes.ContainsKey(term)) 
            return 0;
        else if (!WordsIndexes[term].ContainsKey(document)) 
            return 0;
        else return WordsIndexes[term][document].Count;
    }
    private QueryDimension TfInText(string term, string text)
    {
        //este metodo calcula el valor de una dimension del vector query

        int value = 0;
        
        //esto convierte la query en dos arreglos de strings, uno con las palabras
        //y los operadores y otro sin estas en su version simple
        char[] separators = {' ',',',';','.',':','"','\r','\n'};
        string [] query = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] words = WordOperator.GetWords(query); 
        WordOperator.ConvertToSimpleVersion(words);

        //esto coge la operacion
        int i = Array.IndexOf(words, term);
        WordOperator.WordOperations operation = WordOperator.GetWordOperation(query[i]);

        //esto da el valor tf
        value = words.Where(word => word == term).Count();

        //esto es para obtener la relevancia del termino en caso de que su operador sea *
        if(operation == WordOperator.WordOperations.IsRelevant)
        {
            int relevance = WordOperator.GetRelevance(query[i]);
            return new QueryDimension(value, term, relevance);

        }

        return new QueryDimension(value, term, operation);
        
    }
    private double Idf(string term, string[] documents)
    {
        //calcula la frecuencia inversa de un termino en el directorio
        double idf = 0;
        if (WordsIndexes.ContainsKey(term))
        {
            double numberOfDocuments = (double)documents.Length;
            double appereancesInDocuments = (double)WordsIndexes[term].Count;
            idf = Math.Log10(numberOfDocuments / appereancesInDocuments);
        }
        return idf;
    }
    private double DocumentTfIdf(string term, string document)
    {
        //calcula el valor de tf-idf de una palabra para un documento
        return Idf(term, DocumentNames) * TfInDocument(term, document);
    }
    private QueryDimension QueryTfIdf(string term, string query)
    {
        //calcula el valor de tf-idf de una palabra para la query
        double idf = Idf(term, DocumentNames);
        QueryDimension Dimension = TfInText(term, query);
        Dimension.TfIdfValue *= idf;
        return Dimension;
    }
    #endregion
    private void SaveDatabaseStructure(string path)
    {
        //guarda este objeto en un .json para usarlo mas tarde
        string savePath = Directory.GetParent(path).FullName + "\\DatabaseInfo.json";
        string jsonString = JsonSerializer.Serialize(this);
        File.WriteAllText(savePath, jsonString);
    }
    #endregion
    #region Propierties

    public List<List<double>> DirectoryVector { get; } //la lista de los vectores documentos (i believe in listas supremacy)
    public string DirectoryPath { get;} //la direccion de la base de datos
    public string[] DocumentNames { get;} //los nombres de todos los documentos de la base de datos, sin su extension
    public Dictionary<string, Dictionary<string, List<int>>> WordsIndexes { get;}  //este diccionario esta grande, pero facilita mucho todo
    #endregion
}