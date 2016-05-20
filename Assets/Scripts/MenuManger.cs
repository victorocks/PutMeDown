using UnityEngine;
using System.Collections;

public class MenuManger : MonoBehaviour {

	public GUIStyle HighScoreStyle;
	public GUIStyle ScoreStyle;
	public Texture2D StartTexture;

	private Rect centerRect;
	private Rect titleRect;
	private Rect highScoreRect;
	private Rect scoreRect;
	private string highScoreText;
	private string scoreText;

	void OnEnable()
	{
		StartCoroutine ("Initialize");
	}

	IEnumerator Initialize()
	{
		yield return new WaitForSeconds (2.0f);
		GestureDetector.ShakeDetected += ShakeDetected;
	}

	void OnDisable()
	{
		GestureDetector.ShakeDetected -= ShakeDetected;
	}

	void ShakeDetected()
	{
		Handheld.Vibrate ();
		Application.LoadLevel ("Game");
	}

	// Use this for initialization
	void Start ()
	{
		this.centerRect = new Rect ((Screen.width - this.StartTexture.width) / 2,
		                           (Screen.height - this.StartTexture.height) / 3,
		                           this.StartTexture.width,
		                           this.StartTexture.height);

		this.highScoreText = "HIGH SCORE: ";
		this.scoreText = "LAST SCORE: ";
		Vector2 scoreSize = this.HighScoreStyle.CalcSize (new GUIContent(this.highScoreText + "XXXX"));
		this.highScoreRect = new Rect ((Screen.width - scoreSize.x) / 2,
		                           (Screen.height - scoreSize.y) / 3 * 2,
		                           scoreSize.x,
		                           scoreSize.y);
		this.scoreRect = this.highScoreRect;
		this.scoreRect.y -= this.highScoreRect.height;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.titleRect = this.centerRect;
		this.titleRect.x = Mathf.Lerp(this.titleRect.x, this.titleRect.x * Random.value, Time.deltaTime * 1.0f);
		this.titleRect.y = Mathf.Lerp(this.titleRect.y, this.titleRect.y * Random.value, Time.deltaTime * 1.0f);
	}

	void OnGUI()
	{
		GUI.DrawTexture (this.titleRect, this.StartTexture);

		if (PlayerPrefs.HasKey("HighScore"))
		{
			GUI.Label (this.highScoreRect, this.highScoreText + System.String.Format("{0:00.0}", PlayerPrefs.GetFloat ("HighScore")), this.HighScoreStyle);
		}
		if (PlayerPrefs.HasKey("Score"))
		{
			GUI.Label (this.scoreRect, this.scoreText + System.String.Format("{0:00.0}", PlayerPrefs.GetFloat ("Score")), this.ScoreStyle);
		}
	}
}
