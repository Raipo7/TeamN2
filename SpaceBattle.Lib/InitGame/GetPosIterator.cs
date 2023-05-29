using System.Collections;

namespace SpaceBattle.Lib;

public class GetPosIterator : IEnumerator
{
    private Vector pos;
    private Vector vel;
    private int maxStep;
    private int nCurr;
    private Queue<int> nPos;
    private int i;
    public GetPosIterator(Vector startCoords, Queue<int> nPos)
    {
        this.pos = startCoords;
        this.vel = new Vector(1, 0);
        this.maxStep = 1; //максимальный шаг по одному направлению спирали
        this.nPos = nPos; //сортированная очередь N-числовых позиций
        this.nCurr = 0; //текущая числовая позиция
        this.i = 0; //этап движения по одному направлению спирали
    }
    public object Current => new Vector(pos[0], pos[1]);

    public bool MoveNext()
    {
        var n = nPos.Dequeue(); //получаем числовую позицию
        if (n == nCurr)
        {
            return true;
        }
        while (true)
        {
            pos += vel;
            nCurr++;
            i++;
            if (i == maxStep)
            {
                int save = vel[0];
                vel[0] = -vel[1];
                vel[1] = save;
                i = 0;
                if (vel[0] != 0 || nCurr == 2)
                {
                    maxStep++;
                }
            }
            if (n == nCurr)
            {
                return true;
            }
        }
    }
    public void Reset()
    {
        throw new NotImplementedException();
    }
}
