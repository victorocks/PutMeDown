using UnityEngine;
using System.Collections;

public class GestureDetector : MonoBehaviour {

	public GUIStyle Style;
	public delegate void GestureEvent();
	public static event GestureEvent ShakeDetected;
	public static event GestureEvent FlatDetected;

	public static float timeShaking = 0.0f;
	public static float timeFlat = 0.0f;

	private Vector3 flatAccel = new Vector3(0.0f, 0.0f, -1.0f);
	private Vector3 lastAccel = Vector3.zero;

	private enum GestureState
	{
//		NONE,
		FLAT,
		SHAKING
	}
	private GestureState currentState;
	private float holdingTime = 1.0f;

	// Use this for initialization
	void Start ()
	{
		this.currentState = GestureState.FLAT;
	}

	void Update ()
	{
		float diffAccel = (Input.acceleration - this.lastAccel).magnitude;
		float closeToFlat = Vector3.Dot (Input.acceleration, this.flatAccel);

		if (diffAccel > 2.0f)
		{
			this.currentState = GestureState.SHAKING;
			timeFlat = 0.0f;
		}
		else if (closeToFlat < 1.025f && closeToFlat > 0.975f)
		{
			this.currentState = GestureState.FLAT;
			timeShaking = 0.0f;
		}

		if (this.currentState == GestureState.SHAKING)
		{
			timeShaking += Time.deltaTime;

			if (ShakeDetected != null)
				ShakeDetected();
		}
		else if (this.currentState == GestureState.FLAT)
		{
			timeFlat += Time.deltaTime;

			if (FlatDetected != null && timeFlat > this.holdingTime)
				FlatDetected();
		}

/*		else
		{
			this.currentState = GestureState.NONE;
			timeShaking = 0.0f;
			timeFlat = 0.0f;
		}
*/
		this.lastAccel = Input.acceleration;
	}

	void OnGUI()
	{
//		GUI.Label (new Rect (0, 0, 200, 60), this.currentState + " | tShake: " + timeShaking + " | tFlat: " + timeFlat, this.Style);
	}
}
