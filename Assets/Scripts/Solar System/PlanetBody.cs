using UnityEngine;
using System.Collections;

public class PlanetBody : AbstractCelestialBody
{
	private bool clockwise;
	private GameObject thisPlanet;
	private GameObject sun;
	private ICelestialBody[] moons;

	public override Transform Retrieve
	{
		get{ return thisPlanet.transform;}
	}

	public PlanetBody(GameObject sun, float size, float zPosOffset, bool hasRing, int numberOfMoons) : base(Random.Range(2,5))
	{
		//Options could possibly be split in to different constructors.
		this.sun = sun;
		this.clockwise = (Random.Range(0,2) == 0);
		thisPlanet = (hasRing) ? CreateBody(size, zPosOffset, hasRing) : CreateBody(size,zPosOffset);

		//Create the moons
		if(numberOfMoons > 0)
		{ 
			moons = new ICelestialBody[numberOfMoons];

			for(int i = 0; i < numberOfMoons; i++)
			{
				moons[i] = new MoonBody(thisPlanet,thisPlanet.transform.position.z + (hasRing ? 10.5f:8) + (3f * i));//Consistent incrememted position
			}
		}
	}

	public GameObject ThisPlanet
	{
		get{return thisPlanet;}
	}

	public override void Rotate()
	{
		float xnew,znew;
		float angle = AngleCalc();
		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);

		Vector3 newPlanetPos = new Vector3(thisPlanet.transform.position.x - sun.transform.position.x, thisPlanet.transform.position.y, thisPlanet.transform.position.z - sun.transform.position.y); 

		//Rotation direction
		if(clockwise)
		{
			xnew = newPlanetPos.x * c + newPlanetPos.z * s;
			znew = -newPlanetPos.x * s + newPlanetPos.z * c;
		}
		else
		{
			xnew = newPlanetPos.x * c - newPlanetPos.z * s;
			znew = newPlanetPos.x * s + newPlanetPos.z * c;
		}

		//Translate planetLoc back to actual position
		newPlanetPos.x = xnew + sun.transform.position.x;
		newPlanetPos.z = znew + sun.transform.position.z;

		thisPlanet.transform.position = newPlanetPos;


		//Rotate the moons after the position of the parent planet has been changed
		if(moons != null)
		{	
			foreach(ICelestialBody moon in moons)
			{
				moon.Rotate();
			}
		}
	}
}
