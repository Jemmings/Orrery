using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartPrototype()
	{
		SceneManager.LoadScene( 1 );
	}

	public void QuitProgram()
	{
		Application.Quit();
	}
}
