using System.Collections;

namespace SpaceBattle.Lib;

public class GetFuelIterator : IEnumerator
{
    private IEnumerator enumerator;
    public GetFuelIterator(IEnumerable<int> nPos)
    {
        this.enumerator = nPos.GetEnumerator();
    }
    public object Current => enumerator.Current;

    public bool MoveNext() => enumerator.MoveNext();
    public void Reset()
    {
        throw new NotImplementedException();
    }
}
