using System;

namespace MoogleEngine;
public static class WordOperator
{
    //esta clase estatica contiene varias funciones que uso para trabajar con palabras y eso
    public static string ConvertToSimpleVersion(string word)
    {
        //este metodo devuelve un string simpleversion que es igual al string word,
        //pero en minusculas y sin tildes, una version simple de este.
        //(creo que este metodo se puede mejorar)
        string simpleVersion = ""; 
        for (int n = 0; n < word.Length; n++)
        {
            if (char.IsUpper(word[n])) 
            {   
                char lower = char.ToLower(word[n]);
                if (IsVowel(lower))
                {
                    simpleVersion += RemoveAccent(lower);
                }
                else simpleVersion += lower;
            }
            else
            {
                if (IsVowel(word[n]))
                {
                    simpleVersion += RemoveAccent(word[n]);
                }
                else simpleVersion += word[n];
            }
        }
        return simpleVersion;
    }
    public static void ConvertToSimpleVersion(string[] words)
    {
        //este metodo es lo mismo que el anterior, solo que lo aplica a cada string de un arreglo de strings 
        for (int i = 0; i< words.Length; i++)
        {
            words[i] = ConvertToSimpleVersion(words[i]);
        }
    }
    public static char RemoveAccent(char c)
    {
        //Toma una vocal (minuscula) como parametro y devuelve la misma vocal sin tilde
        string a = "àáâãäå";
        string e = "èéêë";
        string i = "ìíîï";
        string o = "òóôõö";
        //string u = "ùúûü";
        if (IsVowel(c))
        {
            if (a.Contains(c)) return 'a';
            else if (e.Contains(c)) return 'e';
            else if (i.Contains(c)) return 'i';
            else if (o.Contains(c)) return 'o';
            else return 'u';
        }
        else return c;
    }
    public static bool IsVowel(char c) 
    {
        //dice si un caracter es una vocal
        string vowels = "àáâãäåèéêëìíîïòóôõöùúûü";
        if (vowels.Contains(c)) return true;
        else return false;
    }
    public static string[] GetWords(string[] text)
    {
        //este metodo toma como parametro un arreglo de palabras, que pueden tener operadores
        //o no, y devuelve un arreglo de estas palabras sin los operadores
        string[] result = new string[text.Length];
        for(int i = 0; i < result.Length; i++)
        {
            if (text[i] != "") 
            { 
                switch (text[i][0])
                {
                    case '!':
                        result[i] = text[i].Substring(1);
                        break;
                    case '^':
                        result[i] = text[i].Substring(1);
                        break;
                    case '*':
                        for(int j = 0; j < text[i].Length; j++)
                        {
                            if (text[i][j] != '*')
                            {
                                result[i] = text[i].Substring(j);
                                break;
                            }
                        }
                        break;       
                    default:
                        result[i] = text[i];
                        break;
                }
            }
        }
        return result;
    }
    public static string[] GetWords(string text)
    {
        //este metodo convierte un string en un arreglo de palabras sin operadores de busqueda
        char[] separators = { ' ', ',', ';', '.', ':', '"', '\r', '\n' };
        string[] splittedQuery = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        string[] words = GetWords(splittedQuery);
        words = GetWords(words);
        ConvertToSimpleVersion(words);
        return words;
    }
    public static WordOperations GetWordOperation(string queryWord)
    {
        //este toma una palabra, que puede contener un operador de busqueda o no, y devuelve este operador
        switch (queryWord[0])
        {
            case '!':
                return WordOperations.MustNotBe;
            case '^':
                return WordOperations.MustBe;
            case '*':
                return WordOperations.IsRelevant;
            default:
                return WordOperations.NoOperation;
        }
    }
    public static int GetRelevance(string word)
    {
        //Este metodo toma una palabra, que puede tener como operador a * o no, y en
        //dependencia de cuantos operadores de reevancia contenga, devuelve su relevancia
        int result = 0;
        for(int i = 0; i < word.Length; i++)
        {
            if (word[i] == '*')
                result++;
            else break;
        }
        return result;
    }
    public static string Suggestion(string word, List<string> PosibleWords)
    {
        //este metodo toma una palabra, y un vocabulario de palabras, y busca la
        //palabra que mas se parece (la de menos distancia de levenshtein) y la devuelve como sugerencia
        string result = "";
        foreach(string posibleWord in PosibleWords)
        {
            if(LevenshteinDistance(word, result) > LevenshteinDistance(word, posibleWord))
                result = posibleWord;
        }
        return result;

    }
    public static int LevenshteinDistance(string s, string t /*, out double porcentaje*/)
    {
        //este metodo devuelve la distancia de Levenshtein entre dos palabras

        //porcentaje = 0;

        // d es una tabla con m+1 renglones y n+1 columnas
        int costo = 0;
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        // Verifica que exista algo que comparar
        if (n == 0) return m;
        if (m == 0) return n;

        // Llena la primera columna y la primera fila.
        for (int i = 0; i <= m; d[i, 0] = i++) ;
        for (int j = 0; j <= n; d[0, j] = j++) ;


        /// recorre la matriz llenando cada unos de los pesos.
        /// i columnas, j renglones
        for (int i = 1; i <= m; i++)
        {
            // recorre para j
            for (int j = 1; j <= n; j++)
            {
                /// si son iguales en posiciones equidistantes el peso es 0
                /// de lo contrario el peso suma a uno.
                costo = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1,  //Eliminacion
                              d[i, j - 1] + 1),                             //Insercion 
                              d[i - 1, j - 1] + costo);                     //Sustitucion
            }
        }

        /// Calculamos el porcentaje de cambios en la palabra.
        //if (s.Length > t.Length)
        //    porcentaje = ((double)d[m, n] / (double)s.Length);
        //else
        //    porcentaje = ((double)d[m, n] / (double)t.Length);
        return d[m, n];
    }
    public enum WordOperations { NoOperation = 0, MustNotBe, MustBe, IsRelevant, IsCloseTo}
}