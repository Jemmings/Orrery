using UnityEngine;
using System.Collections;

public class MenuPlanet : MonoBehaviour {

	ICelestialBody planet;

	void Start()
	{
		planet = new PlanetBody(this.gameObject,Random.Range(6,11),0,(Random.Range(0,2) == 0),1);
	}

	void Update()
	{
		planet.Rotate();
	}

}
