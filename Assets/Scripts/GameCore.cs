using System;
using UnityEngine;

public class GameCore : MonoBehaviour
{
	RaycastHit2D hit;
	Vector2[] touches = new Vector2[5];
	GraphVertex[] Vertices = new GraphVertex[5];

	public GameState GameState;

	public void Update()
	{
		if (GameState == GameState.Playing)
		{
			HandleTouchInputAndMovement();
		}
		else if (GameState == GameState.SelectLevelScreen)
		{
			HandleTouchDown();
		}
	}

	public void HandleTouchInputAndMovement()
	{
		foreach (Touch t in Input.touches)
		{
			try
			{
				touches[t.fingerId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(t.fingerId).position);
				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Began)
				{
					hit = Physics2D.Raycast(touches[t.fingerId], Vector2.zero);
					if (hit)
					{
						if (hit.collider.gameObject)
						{
							var vertex = hit.collider.gameObject.GetComponent<GraphVertex>();
							vertex.TouchDown();
							Vertices[t.fingerId] = vertex;
						}
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Moved)
				{
					if (Vertices[t.fingerId])
					{
						Vector3 worldPos = Camera.main.ScreenToWorldPoint((Input.GetTouch(t.fingerId).position));
						Vertices[t.fingerId].TouchMove(worldPos);
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Ended)
				{
					if (Vertices[t.fingerId])
					{
						Vertices[t.fingerId].TouchUp();
						Vertices[t.fingerId] = null;
					}
				}
			}
			catch (Exception exp)
			{
				Debug.LogWarning(exp.Message);
			}
		}
	}

	public void HandleTouchDown()
	{
		foreach (Touch t in Input.touches)
		{
//			try
//			{
				touches[t.fingerId] = Camera.main.ScreenToWorldPoint(Input.GetTouch(t.fingerId).position);
				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Began)
				{
					hit = Physics2D.Raycast(touches[t.fingerId], Vector2.zero);
					if (hit)
					{
						if (hit.collider.gameObject)
						{
							var vertex = hit.collider.gameObject.GetComponent<GraphVertex>();
							vertex.TouchDown();
							Vertices[t.fingerId] = vertex;
						}
					}
				}

				if (Input.GetTouch(t.fingerId).phase == TouchPhase.Ended)
				{
					if (Vertices[t.fingerId])
					{
						Vertices[t.fingerId].TouchUp();
						Vertices[t.fingerId] = null;
					}
				}
//			}
//			catch (Exception exp)
//			{
//				Debug.LogWarning(exp.Message);
//			}
		}
	}
}