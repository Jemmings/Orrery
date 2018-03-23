using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystemManager : MonoBehaviour
{
	CameraControl cameraControl;
	UIControl uiControl;
	private GameObject sun;
	private List<ICelestialBody> planets = new List<ICelestialBody>();

	[HideInInspector]
	public bool pauseRotation = true;

	void Awake()
	{
		cameraControl = GetComponent<CameraControl>();
		uiControl = GetComponent<UIControl>();
	}

	void Start()
	{
		sun = CreateSun();
		CreateSolarSystem();
	}


	void Update()
	{
		if(!pauseRotation)
		{
			foreach(ICelestialBody planet in planets)
			{
				planet.Rotate();
			}
		}
	}

	GameObject CreateSun()
	{
		sun = Instantiate(Resources.Load("CelestialBody"), Vector3.zero, Quaternion.identity) as GameObject;
		sun.transform.localScale = new Vector3(30,30,30);
		sun.GetComponent<Renderer>().material.color = Color.yellow;
		return sun;
	}

	void CreateSolarSystem()
	{
		//Planet size and moon number needs to be recorded so that each planet
		//can be offset and no overlapping orbits occur.
		float zPosOffset = 5;//Start offset to move planet away from star
		for(int i = 0; i < Random.Range(3,6); i++)
		{
			int planetSize = Random.Range(5,15);
			int numberOfMoons = Random.Range(0,4);
			bool hasRing = (Random.Range(0,3) == 0);

			zPosOffset += planetSize * 2.5f;
			zPosOffset += (numberOfMoons * 3) + 10;//Same offset used in the PlanetBody's moon creation.
			zPosOffset += (hasRing) ? 7.5f : 2f;

			ICelestialBody planet = new PlanetBody(sun,planetSize,zPosOffset,hasRing,numberOfMoons);
			planets.Add( planet );
		}

		//Set the values for the camera and ui (cam and ui are tightly coupled)
		cameraControl.SetCamRestrictions(zPosOffset);
		uiControl.UpdatePlanetList(planets);
	}

}
