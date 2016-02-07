using UnityEngine;
using System.Collections;

public class dayNightCycle : MonoBehaviour {

	public Cycle day;
	public Cycle night;

	public Light sun;
	public Light Moon;

	public float cycleSpeed;

	static float cycle;

	public Transform earth;


	void Update ()
	{

		earth.Rotate (new Vector3(Time.deltaTime * cycleSpeed,0f,0f));

		cycle = Mathf.PingPong (Time.time * cycleSpeed, 360f);

		sun.intensity = Mathf.Lerp (day.sunIntensity, night.sunIntensity, cycle/360f);
	}









}



[System.Serializable]
public class Cycle
{

	public float sunIntensity;
	public Vector3 earthRotation;
	public Color ambiantLight;

}