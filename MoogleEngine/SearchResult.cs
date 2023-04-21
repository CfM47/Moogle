namespace MoogleEngine;

public class SearchResult
{
    //modifiqué este objeto para que funcionara con una lista de SearchItems, me gusta mas que un arreglo de estos
    private List<SearchItem> items;

    public SearchResult(List<SearchItem> items, string suggestion="")
    {
        if (items == null) {
            throw new ArgumentNullException("items");
        }

        this.items = items;
        this.Suggestion = suggestion;
    }

    public SearchResult() : this(new List<SearchItem>()) {

    }

    public string Suggestion { get; private set; }

    public IEnumerable<SearchItem> Items() {
        return this.items;
    }

    public int Count { get { return this.items.Count; } }
}
