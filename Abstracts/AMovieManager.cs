using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public abstract class AMovieManager : MonoBehaviour {

	protected PlayerManager playerManager;
	protected GameObject cam;
	protected CameraManager cameraManager;
	protected PlayerCanvasManager playerCanvasManager;
	protected virtual void Awake()
	{
		playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
		cam = GameObject.Find("Camera");
		cameraManager = GameObject.Find("Camera").GetComponent<CameraManager>();
		playerCanvasManager = GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvasManager>();
		StartCoroutine(StartMovie());
	}

	protected void StopInputs()
    {
        playerManager.isMenuAwake = true;
    }
    protected void StartInputs()
    {
        playerManager.isMenuAwake = false;
    }

	protected abstract IEnumerator StartMovie();

}
