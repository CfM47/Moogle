using System;
using System.Collections.Generic;

namespace MoogleEngine;
public static class WordsIndexer
{
	//esta clase solo contiene un metodo para crear el diccionario de una base de datos de documentos
	public static Dictionary<string, Dictionary<string,List<int>> > GetWordIndexes(string[] documentsPaths)
	{
		// este metodo toma un conjunto de Documentos de texto (.txt) y devuelve un diccionario que
		// asocia cada palabra que exista con los documentos en quese encuentra y estos a
		// su vez con la cantidad de veces que la palabra aparece en dicho documento.
		Dictionary < string, Dictionary<string, List<int>>> WordIndexes = new Dictionary<string, Dictionary<string, List<int>>>();
		foreach(string documentPath in documentsPaths)
		{
			string text = File.ReadAllText(documentPath);

			//esta parte funciona casi igual que la funcion string.split() pero la uso para obtener la posicion de la palabra
			char[] separators = {' ',',',';','.',':','"','\r','\n','~' };
			string word = "";
			for(int i = 0; i< text.Length; i++)
			{
				if (!separators.Contains(text[i]))word += text[i];

				if ((separators.Contains(text[i]) && word != "") || i == text.Length - 1)
				{
					FileInfo fileInfo = new FileInfo(documentPath);
					string documentName = fileInfo.Name;
					string simplifiedWord = WordOperator.ConvertToSimpleVersion(word);
					if (!WordIndexes.ContainsKey(simplifiedWord))
					{
						WordIndexes.Add(simplifiedWord, new Dictionary<string, List<int>>());
						WordIndexes[simplifiedWord].Add(documentName, new List<int>());
						WordIndexes[simplifiedWord][documentName].Add(i - simplifiedWord.Length);
					}
					else
					{
						if (WordIndexes[simplifiedWord].ContainsKey(documentName))
							WordIndexes[simplifiedWord][documentName].Add(i - simplifiedWord.Length);
						else
						{
							WordIndexes[simplifiedWord].Add(documentName, new List<int>());
							WordIndexes[simplifiedWord][documentName].Add(i - simplifiedWord.Length);
						}
					}
					word = "";
				}
			}
		}
		return WordIndexes;
	}
}