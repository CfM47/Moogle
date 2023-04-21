using System;
using System.Runtime.CompilerServices;

public class SimilarityResult : IComparable
{
	//esta clase es un contenedor mongo de dos variables, sirve para lo mismo que una tupla,
	//solo que es mas bonito acceder a las cosas por su nombre que por item1 o item 2.
	public SimilarityResult(double value, int index)
	{
		Value = value;
		Index = index;
	}
	public double Value { get;}
	public int Index { get;}

	public int CompareTo(object? obj)
	{		
		var b = obj as SimilarityResult;
		return Value.CompareTo(b?.Value);
	}
}
