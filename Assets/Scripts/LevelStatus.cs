using UnityEngine;

public class LevelStatus : MonoBehaviour
{
	public GraphVertex GraphVertex;
	public SpriteRenderer[] SpriteToColor;
	public Color SolvedColor = Color.green;

	void Start()
	{
		if (GraphVertex)
		{
			var isSolved = PlayerPrefs.GetInt(GraphVertex.LevelId.ToString());
			if (isSolved == 1)
			{
				foreach (var spr in SpriteToColor)
				{
					spr.color = SolvedColor;
				}
			}
		}
	}
}
