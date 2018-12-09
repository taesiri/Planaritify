using System;

[Serializable]
public class EdgeData
{
    public int Source;
    public int Sink;

    public EdgeData(int source, int sink)
    {
        Source = source;
        Sink = sink;
    }

    public EdgeData()
    {
        Source = -1;
        Sink = -1;
    }
}