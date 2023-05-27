namespace SpaceBattle.Lib;
using System.Collections;

public class GetPosIterator : IEnumerator
{
    private Vector pos;
    private Vector vel;
    private int maxStep;
    private int nCurr;
    private Queue<int> nPos;
    private bool breakStatus = false;
    private int i;
    public GetPosIterator(Queue<int> nPos) {
        this.pos = new Vector(0, 0);
        this.vel = new Vector(1, 0);
        this.maxStep = 0;
        this.nPos = nPos;
        this.nCurr = 0;
        this.i = 0;
    }
    public object Current => new Vector(pos[0], pos[1]);

    public bool MoveNext()
    {
        var n = nPos.Dequeue();
        while(true)
        {
            if (vel[0] == 1 || nCurr == 2)
            {
                maxStep++;
            }
            while (i <= maxStep) {
                pos += vel;
                nCurr++;
                i++;
                if (i == maxStep) {
                    int save = vel[0];
                    vel[0] = -vel[1];
                    vel[1] = save;
                    i = 0;
                }
                if (n == nCurr) 
                {
                    breakStatus = true;
                    break;
                }
            }
            if (breakStatus) 
            {
                breakStatus = false;
                return true;
            }
        }

    }
    public void Reset()
    {
        throw new NotImplementedException();
    }
}