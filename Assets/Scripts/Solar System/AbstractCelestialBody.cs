using UnityEngine;
using System.Collections;

public abstract class AbstractCelestialBody : ICelestialBody
{
	private float speed{ get; set; }

	public AbstractCelestialBody(float speed)
	{
		this.speed = speed;
	}

	protected GameObject CreateBody(float size, float zPosOffset)
	{
		GameObject newBody = MonoBehaviour.Instantiate(Resources.Load("CelestialBody") as GameObject);
		newBody.GetComponent<MeshRenderer>().material.color = RandomColor();
		newBody.transform.localScale = new Vector3(size,size,size);
		newBody.transform.position = newBody.transform.position + new Vector3(0,0,zPosOffset);
		return newBody;
	}

	protected GameObject CreateBody(float size, float zPosOffset, bool hasRing)
	{
		GameObject newBody = MonoBehaviour.Instantiate(Resources.Load("CelestialBodyRing") as GameObject);
		newBody.GetComponent<MeshRenderer>().material.color = RandomColor();
		newBody.GetComponentsInChildren<MeshRenderer>()[1].material.color = RandomColor();//Ring Colour
		newBody.transform.localScale = new Vector3(size,size,size);
		newBody.transform.position = newBody.transform.position + new Vector3(0,0,zPosOffset);
		return newBody;
	}

	protected float AngleCalc()
	{
		return Time.deltaTime * speed/10;
	}

	protected Color RandomColor()
	{
		return new Color(Random.Range(0f,1.0f),Random.Range(0f,1.0f),Random.Range(0f,1.0f));
	}

	public abstract Transform Retrieve{ get; }
	public abstract void Rotate();
}
