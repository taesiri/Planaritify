using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGHelper{

	public static bool EdgesAreAdjacent(GraphEdge edge1, GraphEdge edge2)
	{
		
//		Debug.Log("Edge 1: " + edge1.GetVertexIds + " -- Edge 2: " + edge2.GetVertexIds);
		var p1 = edge1.GetVertexIds;
		var p2 = edge2.GetVertexIds;

		if (p1.x == p2.x) return true;
		if (p1.y == p2.y) return true;
		if (p1.x == p2.y) return true;
		if (p1.y == p2.x) return true;

		return false;
	}
	
	public static bool DoesIntersect( Vector2 A, Vector2 B, Vector2 C, Vector2 D) {

		var det1 = CalcDeterminant (A.x - C.x,  B.x - C.x,
			A.y- C.y ,  B.y - C.y);
		
		var det2 = CalcDeterminant (A.x - D.x,  B.x - D.x,
			A.y- D.y ,  B.y - D.y);

		var det3 = CalcDeterminant (C.x - A.x,  D.x - A.x,
			C.y - A.y , D.y - A.y);

		var det4 = CalcDeterminant (C.x - B.x,  D.x - B.x,
			C.y - B.y , D.y - B.y);


		var c1 = Mathf.Sign(det1) * Mathf.Sign(det2);
		var c2 = Mathf.Sign(det3) * Mathf.Sign(det4);


		return (c1 < 0 && c2 < 0);

	}

	public static bool DoesIntersect( Vector3 A, Vector3 B, Vector3 C, Vector3 D) {

		var det1 = CalcDeterminant (A.x - C.x,  B.x - C.x,
			A.y- C.y ,  B.y - C.y);

		var det2 = CalcDeterminant (A.x - D.x,  B.x - D.x,
			A.y- D.y ,  B.y - D.y);

		var det3 = CalcDeterminant (C.x - A.x,  D.x - A.x,
			C.y - A.y , D.y - A.y);

		var det4 = CalcDeterminant (C.x - B.x,  D.x - B.x,
			C.y - B.y , D.y - B.y);


		var c1 = Mathf.Sign(det1) * Mathf.Sign(det2);
		var c2 = Mathf.Sign(det3) * Mathf.Sign(det4);


		return (c1 < 0 && c2 < 0);

	}

	public static float CalcDeterminant(float A, float B, float C, float D) {
		return A * B - C * D;
	}


	public static bool CCW(Vector3 A, Vector3 B, Vector3 C) {
		return ((C.y - A.y) * (B.x - A.x) > (B.y - A.y) * (C.x - A.x));
			
	}

	public static bool  DoesIntersect2( Vector3 A, Vector3 B, Vector3 C, Vector3 D) {
		return CCW(A,C,D) != CCW(B,C,D) && CCW(A,B,C) != CCW(A,B,D);
	}
}