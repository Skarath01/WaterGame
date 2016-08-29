using UnityEngine;
using System.Collections;

public class Realtimeterraineditor : MonoBehaviour {

	private void raiseTerrain(Vector3 point){
		int terX = (int)((point.x / Terrain.terrainData.size.x) * xResolution);
		int terZ = (int)((point.z / Terrain.terrainData.size.z) * zResolution);
		float[,] height = Terrain.terrainData.GetHeights (terX - 4, terZ - 4, 9, 9);  //new float[1,1];

		for (int tempY = 0; tempY < 9; tempY++)
			for (int tempX = 0; tempX < 9; tempX++) {
				float dist_to_target = Mathf.Abs ((float)tempY - 4f) + Mathf.Abs ((float)tempX - 4f);
				float maxDist = 8f;
				float proportion = dist_to_target / maxDist;

				height [tempX, tempY] += 0.01f * (1f - proportion);
				height [terX - 4 + tempX, terZ - 4 + tempY] += 0.01f * (1f - proportion);
			}

		Terrain.terrainData.SetHeights (terX - 4, terZ - 4, height);
	}
}