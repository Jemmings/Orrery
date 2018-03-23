using UnityEngine;
using System.Collections;

public class MoonBody : AbstractCelestialBody
{
	private Vector3 relativeDistance;
	private bool clockwise;
	private GameObject thisMoon;
	private GameObject parentPlanet;

	public override Transform Retrieve
	{
		get{ return thisMoon.transform;}
	}

	public MoonBody(GameObject parentPlanet, float zPosOffset) : base(Random.Range(3,10))
	{
		this.parentPlanet = parentPlanet;
		this.clockwise = (Random.Range(0,2) == 0);
		thisMoon = CreateBody(Random.Range(0.3f,1.5f),zPosOffset);//Size is kept low, as the zPosOffset is a constant incremement of 3

		//Set the intended distance between moon/planet to keep it constant
		relativeDistance = thisMoon.transform.position - parentPlanet.transform.position;
	}

	public override void Rotate()
	{
		thisMoon.transform.position = (parentPlanet.transform.position + relativeDistance);

		float xnew, znew;
		float angle = AngleCalc();
		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);

		//Translate planet location back to origin (not actually needed if the origin never moves). 
		Vector3  newMoonPos = new Vector3(thisMoon.transform.position.x - parentPlanet.transform.position.x, thisMoon.transform.position.y, thisMoon.transform.position.z - parentPlanet.transform.position.z);//Original

		//Clockwise
		if(clockwise)
		{
			xnew = newMoonPos.x * c + newMoonPos.z * s;
			znew = -newMoonPos.x * s + newMoonPos.z * c;
		}
		else
		{
			xnew = newMoonPos.x * c - newMoonPos.z * s;
			znew = newMoonPos.x * s + newMoonPos.z * c;
		}

		//Translate planetLoc back to actual position
		newMoonPos.x = xnew + parentPlanet.transform.position.x;
		newMoonPos.z = znew + parentPlanet.transform.position.z;

		thisMoon.transform.position = newMoonPos;
		relativeDistance = thisMoon.transform.position - parentPlanet.transform.position;
	}
}
