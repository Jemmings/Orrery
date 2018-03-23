using UnityEngine;
using System.Collections;

public interface ICelestialBody
{
	Transform Retrieve{ get; }//Returns the body's transfrom(needed for the UI).
	void Rotate();//Provide a simple, uniform method of rotating the planet/moon body
}
