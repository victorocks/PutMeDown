using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Texture2D ShakeTexture;
	public Texture2D FlatTexture;
	public Texture2D BlackTexture;

	private Rect centerRect;
	private Rect meterRect;
	private float totalTime = 0.0f;
	private float maxShakeTime = 3.0f;
	private float minShakeTime = 0.5f;

	private enum GameState
	{
		PUT_ME_DOWN,
		SHAKE_ME_UP
	}
	private GameState currentState;

	void OnEnable()
	{
		GestureDetector.FlatDetected += FlatDetected;
		GestureDetector.ShakeDetected += ShakeDetected;
	}
	
	void OnDisable()
	{
		GestureDetector.FlatDetected -= FlatDetected;
		GestureDetector.ShakeDetected -= ShakeDetected;
	}

	void FlatDetected()
	{
		if (this.currentState == GameState.PUT_ME_DOWN)
		{
			this.currentState = GameState.SHAKE_ME_UP;
			this.GetComponent<AudioSource>().Play();
		}
	}

	void ShakeDetected()
	{
		if(this.currentState == GameState.SHAKE_ME_UP)
		{
			this.meterRect.y -= this.maxShakeTime;
			this.meterRect.y = Mathf.Max(this.meterRect.y, 0);
		}
	}

	// Use this for initialization
	void Start ()
	{
		this.centerRect = new Rect ((Screen.width - this.ShakeTexture.width) / 2,
		                            (Screen.height - this.ShakeTexture.height) / 2,
		                            this.ShakeTexture.width,
		                            this.ShakeTexture.height);
		this.meterRect = new Rect (0, 0, Screen.width, Screen.height);

		this.currentState = GameState.PUT_ME_DOWN;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.totalTime += Time.deltaTime;
		this.maxShakeTime -= (Time.deltaTime * 0.1f);
		this.maxShakeTime = Mathf.Max (this.maxShakeTime, this.minShakeTime);
		this.meterRect.y++;

		if (this.currentState == GameState.SHAKE_ME_UP)
		{
			Handheld.Vibrate ();
			float timeLeft = this.maxShakeTime - GestureDetector.timeShaking - Random.value;
			if (timeLeft < 0.0f)
			{
				this.currentState = GameState.PUT_ME_DOWN;
				this.GetComponent<AudioSource>().Pause();
			}
		}

		if (this.meterRect.y > Screen.height)
		{
			if(PlayerPrefs.GetFloat("HighScore") < this.totalTime)
			{
				PlayerPrefs.SetFloat("HighScore", this.totalTime);
			}
			PlayerPrefs.SetFloat("Score", this.totalTime);
			PlayerPrefs.Save();
			Application.LoadLevel ("Menu");
		}
	}

	void OnGUI()
	{
		GUI.DrawTexture (this.meterRect, this.BlackTexture);

		if (this.currentState == GameState.PUT_ME_DOWN)
		{
			GUI.DrawTexture (this.centerRect, this.FlatTexture);
		}
		else if (this.currentState == GameState.SHAKE_ME_UP)
		{
			GUI.DrawTexture (this.centerRect, this.ShakeTexture);
		}
	}
}
