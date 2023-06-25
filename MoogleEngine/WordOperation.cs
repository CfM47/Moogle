using System;
public abstract class WordOperation{}
public class OpNone : WordOperation
{
    public OpNone() { }
}
public class OpMustNotBe : WordOperation
{
    public OpMustNotBe(){}
}
public class OpMustBe : WordOperation
{
    public OpMustBe(){}
}
public class OpIsRelevant : WordOperation
{
    public int WordRelevance { get; }
    public OpIsRelevant(int relevance)
    {
        WordRelevance = relevance;
    }
}
public class OpIsCloseTo : WordOperation
{
    public string NearbyWord { get; }
    public OpIsCloseTo(string nearbyWord)
    {
        NearbyWord = nearbyWord;
    }
}
