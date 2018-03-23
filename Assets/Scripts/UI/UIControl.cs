using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour {


	private CameraControl camControl;
	private SolarSystemManager solarSystemManager;

	public Dropdown viewMenu;
	public Text titleText;
	public Text paragraphText;

	private bool orbitPaused = true;
	private Transform[] planetPositions;


	void Awake()
	{
		camControl = GetComponent<CameraControl>();//The UI and Camera are tightly coupled, which could be changed
		solarSystemManager = GetComponent<SolarSystemManager>();
	}
		
	//Updates the options in the dropdown to include the randomly generated planets,
	//and stores their positions in an array.
	public void UpdatePlanetList(List<ICelestialBody> planets)
	{
		List<string> planetOptions = new List<string>();
		planetPositions = new Transform[planets.Count];

		for(int i = 0; i < planets.Count; i++)
		{
			planetPositions[i] = planets[i].Retrieve;
			int textPos = i + 1;
			planetOptions.Add( "Planet " + textPos.ToString() );
		}

		viewMenu.AddOptions( planetOptions );
	}

	public void ChangeView()
	{

		if(viewMenu.value == 0)
		{
			camControl.ChangeView(0);
		}
		else if(viewMenu.value == 1)
		{
			camControl.ChangeView(1);
		}
		else
		{
			camControl.ChangeView(planetPositions[viewMenu.value-2]);
		}
	}
		
	public void PauseOrbit()
	{
		orbitPaused = !orbitPaused;
		solarSystemManager.pauseRotation = !solarSystemManager.pauseRotation;
	}

	public IEnumerator FadeInfo(string title, string paragraph, bool fadeIn)
	{
		if(fadeIn)
		{
			titleText.text = title;
			paragraphText.text = paragraph;
		}

		float lerpTime = 0.5f;
		float elapsedTime = 0;
		float startingColour = (fadeIn) ? 0: 1;
		float endingColour = (fadeIn) ? 1: 0;

		while (elapsedTime < lerpTime)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > lerpTime)
				elapsedTime = lerpTime;

			//Lerp
			float perc = elapsedTime / lerpTime;//No easing. Always use this either on it's own or in combination with the easings below;
			//perc = Mathf.Sin(perc * Mathf.PI * 0.5f);//Ease in
			//perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);//Ease out

			float colour = Mathf.Lerp(startingColour,endingColour,perc);
			titleText.color = new Color(0,0,0,colour);
			paragraphText.color = new Color(0,0,0,colour);

			yield return null;
		}

		titleText.color = new Color(0,0,0,endingColour);
		paragraphText.color = new Color(0,0,0,endingColour);
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene( 1 );
	}

	public void Exit()
	{
		SceneManager.LoadScene( 0 );
	}
}
