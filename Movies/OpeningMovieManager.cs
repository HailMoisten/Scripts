using UnityEngine;
using System.Collections;

public class OpeningMovieManager : AMovieManager {

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
	}

	protected override IEnumerator StartMovie()
	{
		FadeManager.Instance.FadeIn(10.0f);
		StopInputs();
		playerCanvasManager.UIOnOff();
		yield return new WaitForSeconds(1.0f);
		cam.transform.position = new Vector3(364.5f, 56.7f, 350.0f);
		cameraManager.declementDistance();
		cameraManager.declementDistance();
		cameraManager.declementDistance();
		cameraManager.declementDistance();
		cameraManager.declementHeight();
		cameraManager.declementHeight();
		StartInputs();
		yield return new WaitForSeconds(18.0f);
		iTween.RotateTo(cam,
			iTween.Hash("rotation", new Vector3(60, 160, 20),
				"time", 3.0f,
				"easetype", "InCubic"
			));
		FadeManager.Instance.FadeOut(3.0f);
		yield return new WaitForSeconds(3.0f);
		cam.transform.position = new Vector3(365.5f, 50.0f, 360.0f);
		yield return new WaitForSeconds(0.5f);
		FadeManager.Instance.FadeIn(2.0f);
		iTween.MoveTo(cam,
			iTween.Hash("position", new Vector3(330, 80, 340),
				"time", 12.0f,
				"easetype", "linear"
			));
		iTween.RotateTo(cam,
			iTween.Hash("rotation", new Vector3(20, 130, 0),
				"time", 12.0f,
				"easetype", "linear"
			));
		yield return new WaitForSeconds(12.0f);

		iTween.MoveTo(cam,
			iTween.Hash("position", new Vector3(365, 110, 250),
				"time", 6.0f,
				"easetype", "linear"
			));
		iTween.RotateTo(cam,
			iTween.Hash("rotation", new Vector3(30, 35, 0),
				"time", 6.0f,
				"easetype", "linear"
			));
		yield return new WaitForSeconds(6.0f);
		iTween.MoveTo(cam,
			iTween.Hash("position", new Vector3(400, 110, 300),
				"time", 8.0f,
				"easetype", "linear"
			));
		iTween.RotateTo(cam,
			iTween.Hash("rotation", new Vector3(40, -60, 0),
				"time", 8.0f,
				"easetype", "linear"
			));
		yield return new WaitForSeconds(5.5f);
		FadeManager.Instance.FadeOut(2.0f);

	}
}
