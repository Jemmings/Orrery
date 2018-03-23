using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

	private UIControl uicontrol;
	private FactStorage factStore;

	public Dropdown viewMenu;
	public Toggle pauseToggle;

	private Transform mainCam, focusPlanet;
	private float camHeight;
	private Vector3 topDownView, topDownRotation;
	private Vector3 horizonView, horizonRotation;
	private bool movingCamera = false;
	private bool followingPlanet = false;
	private Vector3 planetFocusOffset = new Vector3(0,5,-35);

	void Awake()
	{
		mainCam = Camera.main.transform;
		uicontrol = GetComponent<UIControl>();
		factStore = GetComponent<FactStorage>();
	}


	void LateUpdate()
	{
		if(followingPlanet && !movingCamera)
		{
			FollowPlanet();
		}
	}

	public void ChangeView(int viewType)
	{
		if(viewType == 0 && !movingCamera)
		{
			viewMenu.interactable = false;
			pauseToggle.interactable = false;
			StartCoroutine( CameraLerp(mainCam,topDownView,topDownRotation, 1f) );
			movingCamera = true;

			if(followingPlanet)
			{
				StartCoroutine( uicontrol.FadeInfo("Title","Paragraph",false) );
			}

			followingPlanet = false;
		}
		else if(viewType == 1 && !movingCamera)
		{
			viewMenu.interactable = false;
			pauseToggle.interactable = false;
			StartCoroutine( CameraLerp(mainCam,horizonView,horizonRotation, 1f) );
			movingCamera = true;

			if(followingPlanet)
			{
				StartCoroutine( uicontrol.FadeInfo("Title","Paragraph",false) );
			}

			followingPlanet = false;
		}
		else
		{
			return;
		}
	}

	public void ChangeView(Transform focusTarget)
	{
		if(followingPlanet)
		{
			StartCoroutine( uicontrol.FadeInfo("Title","Paragraph",false) );
		}

		viewMenu.interactable = false;
		pauseToggle.interactable = false;
		Vector3 focusPos = focusTarget.position;

		if(!pauseToggle.isOn)
		{
			uicontrol.PauseOrbit();
		}

		Vector3 planetPos = focusPos + planetFocusOffset;
		focusPlanet = focusTarget;

		StartCoroutine( CameraLerp( mainCam, planetPos, new Vector3(8,0,0), 1f) );
		movingCamera = true;
		followingPlanet = true;
	}

	public void SetCamRestrictions(float height)
	{
		topDownView = new Vector3(0, height * 2, 0);
		topDownRotation = new Vector3(90,0,0);

		horizonView = new Vector3(0,0,-height * 1.5f);
		horizonRotation = new Vector3(0,0,0);

		//Force update the camera position
		ChangeView(0);
	}

	IEnumerator CameraLerp(Transform camGO, Vector3 pos, Vector3 rot, float lerpTime)
	{
		float elapsedTime = 0;
		Vector3 startingPos = camGO.position;
		Vector3 startingRot = camGO.localEulerAngles;

		while (elapsedTime < lerpTime)
		{
			//increment timer once per frame
			elapsedTime += Time.deltaTime;
			if (elapsedTime > lerpTime)
				elapsedTime = lerpTime;

			//lerp!
			float perc = elapsedTime / lerpTime;//No easing. Always use this either on it's own or in combination with the easings below.
			//perc = Mathf.Sin(perc * Mathf.PI * 0.5f);//Ease in
			//perc = 1f - Mathf.Cos(perc * Mathf.PI * 0.5f);//Ease out

			camGO.position = Vector3.Lerp(startingPos, pos, perc);
			camGO.localEulerAngles = Vector3.Lerp(startingRot,rot,perc);

			yield return null;
		}

		camGO.position = pos;//Makes sure the go reaches it's intended destination.
		camGO.localEulerAngles = rot;

		//Choose a random 'fact' to display
		if(followingPlanet)
		{
			string[] fact = factStore.GetRandomFact();
			StartCoroutine( uicontrol.FadeInfo(fact[0],fact[1],true) );

			if(!pauseToggle.isOn)
			{
				uicontrol.PauseOrbit();
			}
		}

		movingCamera = false;
		viewMenu.interactable = true;
		pauseToggle.interactable = true;
	}
		

	void FollowPlanet()
	{
		mainCam.position = focusPlanet.position + planetFocusOffset;
	}
}
