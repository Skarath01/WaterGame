using UnityEngine;
using System.Collections;

public class terrainChanger : MonoBehaviour{
	public Terrain mainTerrain;

	int resolutionX;
	int resolutionY;
	float[,] heigths;

	// Use this for initialization
	void Start () {
		resolutionX = mainTerrain.terrainData.heightmapWidth;
		resolutionY = mainTerrain.terrainData.heightmapHeight;

		heigths = mainTerrain.terrainData.GetHeights (0, 0, resolutionX, resolutionY);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit))
			{
				ModifyTerrain (hit.point, 0.001f, 20);
			}
		}
	}

	void ModifyTerrain(Vector3 position, float amount, int diameter)
	{
		int terrainPosX = (int)((position.x / mainTerrain.terrainData.size.x) * resolutionX);
		int terrainPosY = (int)((position.z / mainTerrain.terrainData.size.z) * resolutionY);

		float[,] heightChange = new float[diameter, diameter];

		int radius = (int)(diameter / 2);
			
		for (int x = 0; x < diameter; x++)
		{
			for(int y = 0; y < diameter; y++)
			{
				int x2 = x - radius;
				int y2 = y - radius;

				if (terrainPosY + y2 < 0 || terrainPosY + y2 >= resolutionY || terrainPosX + x2 < 0 || terrainPosX + x2 >= resolutionX)
					continue;
				
				float distance = Mathf.Sqrt((x2 * x2) + (y2 * y2));

				if (distance > radius)
				{
					heightChange [y, x] = heigths [terrainPosY + y2, terrainPosX + x2];
				}
				else
				{
					heightChange [y, x] = heigths [terrainPosY + y2, terrainPosX + x2] + (amount - (amount * (distance / radius)));
					heigths [terrainPosY + y2, terrainPosX + x2] = heightChange[y, x];
				}
			}
		}

		mainTerrain.terrainData.SetHeights (terrainPosX - radius, terrainPosY - radius, heightChange);
	}

	void OnCameraMove (Vector3 newCameraPosition)
	{
		
	}
}