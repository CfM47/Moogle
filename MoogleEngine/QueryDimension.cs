using System;

namespace MoogleEngine;
public class QueryDimension : IComparable
{
	//Esta clase contiene los valores de cada dimension de un vector Query
	public QueryDimension(double tfIdfValue, string word,  WordOperator.WordOperations operation)
	{
		//constructor por defecto;
		TfIdfValue = tfIdfValue;
		Word = word;
		Operation = operation;
	}
    public QueryDimension(double tfIdfValue, string word, int relevance)
    {
		//usar este constructor solo si el operador de esa palabra es  "*"
        TfIdfValue = tfIdfValue;
		Word = word;
        Operation = WordOperator.WordOperations.IsRelevant;
		Relevance = relevance;
    }
    public double TfIdfValue { get; set; }
	public string Word { get;}
	public WordOperator.WordOperations Operation { get; }
	public int Relevance { get; }

	public int CompareTo(object? obj)
	{
		//Este objeto solo se compara con objetos del mismo tipo, y compara
		//los valores tf-idf de ellos
		var b = obj as QueryDimension;
		return TfIdfValue.CompareTo(b.TfIdfValue);
	}
}
