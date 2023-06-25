using System;

namespace MoogleEngine;
public class QueryDimension : IComparable
{
	//Esta clase contiene los valores de cada dimension de un vector Query
	public QueryDimension(double tfIdfValue, string word, WordOperation operation)
	{
		//constructor por defecto;
		TfIdfValue = tfIdfValue;
		Word = word;
		Operation = operation;
	}
    public double TfIdfValue { get; set; }
	public string Word { get;}
	public WordOperation Operation { get; }
	public int CompareTo(object? obj)
	{
		//Este objeto solo se compara con objetos del mismo tipo, y compara
		//los valores tf-idf de ellos
		var b = obj as QueryDimension;
		return TfIdfValue.CompareTo(b.TfIdfValue);
	}
}
