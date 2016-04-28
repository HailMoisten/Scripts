using UnityEngine;
using System.Collections;

public static class GameManager : MonoBehaviour {
    public static int Brightness { get; set; }
    public static int Sound { get; set; }
    public static int Difficulty { get; set; }
    public static int Form { get; set; }
	public static string SceneName { get; set; }

	public void Awake() {
		Difficulty = 200;
	}

}
