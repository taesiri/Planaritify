using Boo.Lang;
using UnityEngine;

public class GraphEmbeddingData
{
    public List<Vector3> VertexPosition;
    public List<GraphVertex> GraphVertextComponent;

    public GraphEmbeddingData()
    {
        VertexPosition = new List<Vector3>();
        GraphVertextComponent = new List<GraphVertex>();
    }
    
    public void Add(Vector3 nodePosition, GraphVertex vScript)
    {
        VertexPosition.Add(nodePosition);
        GraphVertextComponent.Add(vScript);
    }

    public GraphVertex GetVertexAt(int index)
    {
        return GraphVertextComponent[index];
    }

    public void AttachPrefabToEachNode(GameObject wigglerPrefab)
    {
        foreach (var gVertex in GraphVertextComponent)
        {
            var w = GameObject.Instantiate(wigglerPrefab, Vector3.zero, Quaternion.identity);
            w.GetComponent<GraphWiggle>().NodeToWiggle = gVertex;
            w.transform.parent = gVertex.transform;
        }
    }
}