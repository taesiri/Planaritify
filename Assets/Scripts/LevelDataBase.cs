using System;
using Boo.Lang;
using UnityEngine;

[CreateAssetMenu]
[Serializable]
public class LevelDataBase : ScriptableObject
{
    [SerializeField] public GraphDescription[] Graphs;

    public void AddItem(GraphDescription sampleGraph)
    {
        var temp = new List<GraphDescription>();
        if (Graphs != null)
        {
            temp = new List<GraphDescription>(Graphs);
        }

        temp.Add(sampleGraph);
        Graphs = temp.ToArray();
    }
}

[Serializable]
public class GraphDescription
{
    [SerializeField] public EdgeData[] EdgeList;
    [SerializeField] public Vector3[] Embedding;

    public static GraphDescription SampleGraph()
    {
        return new GraphDescription
        {
            EdgeList = new[] {new EdgeData(1, 2), new EdgeData(1, 3)},
            Embedding = new[] {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0)}
        };
    }
}