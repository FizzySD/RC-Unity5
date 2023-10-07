using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class iTween : MonoBehaviour
{
	private delegate void ApplyTween();

	private class CRSpline
	{
		public Vector3[] pts;

		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		public Vector3 Interp(float t)
		{
			int num = pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 vector = pts[num2];
			Vector3 vector2 = pts[num2 + 1];
			Vector3 vector3 = pts[num2 + 2];
			Vector3 vector4 = pts[num2 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
		}
	}

	public static class Defaults
	{
		public static int cameraFadeDepth = 999999;

		public static Color color = Color.white;

		public static float delay = 0f;

		public static EaseType easeType = EaseType.easeOutExpo;

		public static bool isLocal = false;

		public static float lookAhead = 0.05f;

		public static float lookSpeed = 3f;

		public static LoopType loopType = LoopType.none;

		public static NamedValueColor namedColorValue = NamedValueColor._Color;

		public static bool orientToPath = false;

		public static Space space = Space.Self;

		public static float time = 1f;

		public static Vector3 up = Vector3.up;

		public static float updateTime = 1f * updateTimePercentage;

		public static float updateTimePercentage = 0.05f;

		public static bool useRealTime = false;
	}

	public enum EaseType
	{
		easeInQuad = 0,
		easeOutQuad = 1,
		easeInOutQuad = 2,
		easeInCubic = 3,
		easeOutCubic = 4,
		easeInOutCubic = 5,
		easeInQuart = 6,
		easeOutQuart = 7,
		easeInOutQuart = 8,
		easeInQuint = 9,
		easeOutQuint = 10,
		easeInOutQuint = 11,
		easeInSine = 12,
		easeOutSine = 13,
		easeInOutSine = 14,
		easeInExpo = 15,
		easeOutExpo = 16,
		easeInOutExpo = 17,
		easeInCirc = 18,
		easeOutCirc = 19,
		easeInOutCirc = 20,
		linear = 21,
		spring = 22,
		easeInBounce = 23,
		easeOutBounce = 24,
		easeInOutBounce = 25,
		easeInBack = 26,
		easeOutBack = 27,
		easeInOutBack = 28,
		easeInElastic = 29,
		easeOutElastic = 30,
		easeInOutElastic = 31,
		punch = 32
	}

	private delegate float EasingFunction(float start, float end, float Value);

	public enum LoopType
	{
		none = 0,
		loop = 1,
		pingPong = 2
	}

	public enum NamedValueColor
	{
		_Color = 0,
		_SpecColor = 1,
		_Emission = 2,
		_ReflectColor = 3
	}

	public string _name;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap10;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap11;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap12;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap13;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap14;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap8;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmap9;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapA;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapB;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapC;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapD;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapE;

	[CompilerGenerated]
	private static Dictionary<string, int> fswitchSmapF;

	private ApplyTween apply;

	private AudioSource audioSource;

	private static GameObject cameraFade;

	private Color[,] colors;

	public float delay;

	private float delayStarted;

	private EasingFunction ease;

	public EaseType easeType;

	private float[] floats;

	public string id;

	private bool isLocal;

	public bool isPaused;

	public bool isRunning;

	private bool kinematic;

	private float lastRealTime;

	private bool loop;

	public LoopType loopType;

	public string method;

	private NamedValueColor namedcolorvalue;

	private CRSpline path;

	private float percentage;

	private bool physics;

	private Vector3 postUpdate;

	private Vector3 preUpdate;

	private Rect[] rects;

	private bool reverse;

	private float runningTime;

	private Space space;

	private Transform thisTransform;

	public float time;

	private Hashtable tweenArguments;

	public static List<Hashtable> tweens = new List<Hashtable>();

	public string type;

	private bool useRealTime;

	private Vector2[] vector2s;

	private Vector3[] vector3s;

	private bool wasPaused;

	private iTween(Hashtable h)
	{
		tweenArguments = h;
	}

	private void ApplyAudioToTargets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		audioSource.volume = vector2s[2].x;
		audioSource.pitch = vector2s[2].y;
		if (percentage == 1f)
		{
			audioSource.volume = vector2s[1].x;
			audioSource.pitch = vector2s[1].y;
		}
	}

	private void ApplyColorTargets()
	{
		colors[0, 2].r = ease(colors[0, 0].r, colors[0, 1].r, percentage);
		colors[0, 2].g = ease(colors[0, 0].g, colors[0, 1].g, percentage);
		colors[0, 2].b = ease(colors[0, 0].b, colors[0, 1].b, percentage);
		colors[0, 2].a = ease(colors[0, 0].a, colors[0, 1].a, percentage);
		tweenArguments["onupdateparams"] = colors[0, 2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = colors[0, 1];
		}
	}

	private void ApplyColorToTargets()
	{
		for (int i = 0; i < colors.GetLength(0); i++)
		{
			colors[i, 2].r = ease(colors[i, 0].r, colors[i, 1].r, percentage);
			colors[i, 2].g = ease(colors[i, 0].g, colors[i, 1].g, percentage);
			colors[i, 2].b = ease(colors[i, 0].b, colors[i, 1].b, percentage);
			colors[i, 2].a = ease(colors[i, 0].a, colors[i, 1].a, percentage);
		}
		if (GetComponent<GUITexture>() != null)
		{
			base.GetComponent<GUITexture>().color = colors[0, 2];
		}
		else if (GetComponent<GUIText>() != null)
		{
			base.GetComponent<GUIText>().material.color = colors[0, 2];
		}
		else if (base.GetComponent<Renderer>() != null)
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				base.GetComponent<Renderer>().materials[j].SetColor(namedcolorvalue.ToString(), colors[j, 2]);
			}
		}
		else if (base.GetComponent<Light>() != null)
		{
			base.GetComponent<Light>().color = colors[0, 2];
		}
		if (percentage != 1f)
		{
			return;
		}
		if (GetComponent<GUITexture>() != null)
		{
			base.GetComponent<GUITexture>().color = colors[0, 1];
		}
		else if (GetComponent<GUIText>() != null)
		{
			base.GetComponent<GUIText>().material.color = colors[0, 1];
		}
		else if (base.GetComponent<Renderer>() != null)
		{
			for (int k = 0; k < colors.GetLength(0); k++)
			{
				base.GetComponent<Renderer>().materials[k].SetColor(namedcolorvalue.ToString(), colors[k, 1]);
			}
		}
		else if (base.GetComponent<Light>() != null)
		{
			base.GetComponent<Light>().color = colors[0, 1];
		}
	}

	private void ApplyFloatTargets()
	{
		floats[2] = ease(floats[0], floats[1], percentage);
		tweenArguments["onupdateparams"] = floats[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = floats[1];
		}
	}

	private void ApplyLookToTargets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			thisTransform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			thisTransform.rotation = Quaternion.Euler(vector3s[2]);
		}
	}

	private void ApplyMoveByTargets()
	{
		preUpdate = thisTransform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = thisTransform.eulerAngles;
			thisTransform.eulerAngles = vector3s[4];
		}
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		thisTransform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			thisTransform.eulerAngles = eulerAngles;
		}
		postUpdate = thisTransform.position;
		if (physics)
		{
			thisTransform.position = preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveToPathTargets()
	{
		preUpdate = thisTransform.position;
		float value = ease(0f, 1f, percentage);
		if (isLocal)
		{
			thisTransform.localPosition = path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		else
		{
			thisTransform.position = path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			float num = ((!tweenArguments.Contains("lookahead")) ? Defaults.lookAhead : ((float)tweenArguments["lookahead"]));
			float value2 = ease(0f, 1f, Mathf.Min(1f, percentage + num));
			tweenArguments["looktarget"] = path.Interp(Mathf.Clamp(value2, 0f, 1f));
		}
		postUpdate = thisTransform.position;
		if (physics)
		{
			thisTransform.position = preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveToTargets()
	{
		preUpdate = thisTransform.position;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			thisTransform.localPosition = vector3s[2];
		}
		else
		{
			thisTransform.position = vector3s[2];
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				thisTransform.localPosition = vector3s[1];
			}
			else
			{
				thisTransform.position = vector3s[1];
			}
		}
		postUpdate = thisTransform.position;
		if (physics)
		{
			thisTransform.position = preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyPunchPositionTargets()
	{
		preUpdate = thisTransform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = thisTransform.eulerAngles;
			thisTransform.eulerAngles = vector3s[4];
		}
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		thisTransform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			thisTransform.eulerAngles = eulerAngles;
		}
		postUpdate = thisTransform.position;
		if (physics)
		{
			thisTransform.position = preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyPunchRotationTargets()
	{
		preUpdate = thisTransform.eulerAngles;
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		thisTransform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = thisTransform.eulerAngles;
		if (physics)
		{
			thisTransform.eulerAngles = preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyPunchScaleTargets()
	{
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		thisTransform.localScale = vector3s[0] + vector3s[2];
	}

	private void ApplyRectTargets()
	{
		rects[2].x = ease(rects[0].x, rects[1].x, percentage);
		rects[2].y = ease(rects[0].y, rects[1].y, percentage);
		rects[2].width = ease(rects[0].width, rects[1].width, percentage);
		rects[2].height = ease(rects[0].height, rects[1].height, percentage);
		tweenArguments["onupdateparams"] = rects[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = rects[1];
		}
	}

	private void ApplyRotateAddTargets()
	{
		preUpdate = thisTransform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		thisTransform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = thisTransform.eulerAngles;
		if (physics)
		{
			thisTransform.eulerAngles = preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyRotateToTargets()
	{
		preUpdate = thisTransform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			thisTransform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			thisTransform.rotation = Quaternion.Euler(vector3s[2]);
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				thisTransform.localRotation = Quaternion.Euler(vector3s[1]);
			}
			else
			{
				thisTransform.rotation = Quaternion.Euler(vector3s[1]);
			}
		}
		postUpdate = thisTransform.eulerAngles;
		if (physics)
		{
			thisTransform.eulerAngles = preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyScaleToTargets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		thisTransform.localScale = vector3s[2];
		if (percentage == 1f)
		{
			thisTransform.localScale = vector3s[1];
		}
	}

	private void ApplyShakePositionTargets()
	{
		if (isLocal)
		{
			preUpdate = thisTransform.localPosition;
		}
		else
		{
			preUpdate = thisTransform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = thisTransform.eulerAngles;
			thisTransform.eulerAngles = vector3s[3];
		}
		if (percentage == 0f)
		{
			thisTransform.Translate(vector3s[1], space);
		}
		if (isLocal)
		{
			thisTransform.localPosition = vector3s[0];
		}
		else
		{
			thisTransform.position = vector3s[0];
		}
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		if (isLocal)
		{
			thisTransform.localPosition += vector3s[2];
		}
		else
		{
			thisTransform.position += vector3s[2];
		}
		if (tweenArguments.Contains("looktarget"))
		{
			thisTransform.eulerAngles = eulerAngles;
		}
		postUpdate = thisTransform.position;
		if (physics)
		{
			thisTransform.position = preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyShakeRotationTargets()
	{
		preUpdate = thisTransform.eulerAngles;
		if (percentage == 0f)
		{
			thisTransform.Rotate(vector3s[1], space);
		}
		thisTransform.eulerAngles = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		thisTransform.Rotate(vector3s[2], space);
		postUpdate = thisTransform.eulerAngles;
		if (physics)
		{
			thisTransform.eulerAngles = preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyShakeScaleTargets()
	{
		if (percentage == 0f)
		{
			thisTransform.localScale = vector3s[1];
		}
		thisTransform.localScale = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		thisTransform.localScale += vector3s[2];
	}

	private void ApplyStabTargets()
	{
	}

	private void ApplyVector2Targets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		tweenArguments["onupdateparams"] = vector2s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector2s[1];
		}
	}

	private void ApplyVector3Targets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		tweenArguments["onupdateparams"] = vector3s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector3s[1];
		}
	}

	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!(target.GetComponent<AudioSource>() != null))
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector = default(Vector2);
		Vector2 vector2 = default(Vector2);
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		object[] args = new object[6] { "volume", volume, "pitch", pitch, "time", time };
		AudioFrom(target, Hash(args));
	}

	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		object[] args = new object[6] { "volume", volume, "pitch", pitch, "time", time };
		AudioTo(target, Hash(args));
	}

	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!(target.GetComponent<AudioSource>() != null))
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		object[] args = new object[6] { "volume", volume, "pitch", pitch, "time", time };
		AudioUpdate(target, Hash(args));
	}

	private void Awake()
	{
		thisTransform = base.transform;
		RetrieveArgs();
		lastRealTime = Time.realtimeSinceStartup;
	}

	private void CallBack(string callbackType)
	{
		if (tweenArguments.Contains(callbackType) && !tweenArguments.Contains("ischild"))
		{
			GameObject gameObject = ((!tweenArguments.Contains(callbackType + "target")) ? base.gameObject : ((GameObject)tweenArguments[callbackType + "target"]));
			if (tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)tweenArguments[callbackType], tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
				return;
			}
			Debug.LogError("iTween Error: Callback method references must be passed as a String!");
			UnityEngine.Object.Destroy(this);
		}
	}

	public static GameObject CameraFadeAdd()
	{
		if (cameraFade != null)
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, Defaults.cameraFadeDepth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = CameraTexture(Color.black);
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		if (cameraFade != null)
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, Defaults.cameraFadeDepth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = texture;
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		if (cameraFade != null)
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.transform.position = new Vector3(0.5f, 0.5f, depth);
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().texture = texture;
		cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return cameraFade;
	}

	public static void CameraFadeDepth(int depth)
	{
		if (cameraFade != null)
		{
			cameraFade.transform.position = new Vector3(cameraFade.transform.position.x, cameraFade.transform.position.y, depth);
		}
	}

	public static void CameraFadeDestroy()
	{
		if (cameraFade != null)
		{
			UnityEngine.Object.Destroy(cameraFade);
		}
	}

	public static void CameraFadeFrom(Hashtable args)
	{
		if (cameraFade != null)
		{
			ColorFrom(cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeFrom(float amount, float time)
	{
		if (cameraFade != null)
		{
			object[] args = new object[4] { "amount", amount, "time", time };
			CameraFadeFrom(Hash(args));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeSwap(Texture2D texture)
	{
		if (cameraFade != null)
		{
			cameraFade.GetComponent<GUITexture>().texture = texture;
		}
	}

	public static void CameraFadeTo(Hashtable args)
	{
		if (cameraFade != null)
		{
			ColorTo(cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(float amount, float time)
	{
		if (cameraFade != null)
		{
			object[] args = new object[4] { "amount", amount, "time", time };
			CameraFadeTo(Hash(args));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		foreach (DictionaryEntry arg in args)
		{
			hashtable.Add(arg.Key, arg.Value);
		}
		foreach (DictionaryEntry item in hashtable)
		{
			if (item.Value.GetType() == typeof(int))
			{
				int num = (int)item.Value;
				float num2 = num;
				args[item.Key] = num2;
			}
			if (item.Value.GetType() == typeof(double))
			{
				double num3 = (double)item.Value;
				float num4 = (float)num3;
				args[item.Key] = num4;
			}
		}
		foreach (DictionaryEntry arg2 in args)
		{
			string key = arg2.Key.ToString().ToLower();
			hashtable2.Add(key, arg2.Value);
		}
		args = hashtable2;
		return args;
	}

	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) * 0.5f);
		float num4 = 0f;
		if (end - start < 0f - num3)
		{
			num4 = (num2 - start + end) * value;
			return start + num4;
		}
		if (end - start > num3)
		{
			num4 = (0f - (num2 - end + start)) * value;
			return start + num4;
		}
		return start + (end - start) * value;
	}

	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorFrom(item.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		if (target.GetComponent<GUITexture>() != null)
		{
			color2 = (color = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent<GUIText>() != null)
		{
			color2 = (color = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>() != null)
		{
			color2 = (color = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>() != null)
		{
			color2 = (color = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent<GUITexture>() != null)
		{
			target.GetComponent<GUITexture>().color = color;
		}
		else if (target.GetComponent<GUIText>() != null)
		{
			target.GetComponent<GUIText>().material.color = color;
		}
		else if (target.GetComponent<Renderer>() != null)
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (target.GetComponent<Light>() != null)
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ColorFrom(GameObject target, Color color, float time)
	{
		object[] args = new object[4] { "color", color, "time", time };
		ColorFrom(target, Hash(args));
	}

	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorTo(item.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ColorTo(GameObject target, Color color, float time)
	{
		object[] args = new object[4] { "color", color, "time", time };
		ColorTo(target, Hash(args));
	}

	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				ColorUpdate(item.gameObject, args);
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		if (target.GetComponent<GUITexture>() != null)
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent<GUIText>() != null)
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>() != null)
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>() != null)
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent<GUITexture>() != null)
		{
			target.GetComponent<GUITexture>().color = array[3];
		}
		else if (target.GetComponent<GUIText>() != null)
		{
			target.GetComponent<GUIText>().material.color = array[3];
		}
		else if (target.GetComponent<Renderer>() != null)
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if (target.GetComponent<Light>() != null)
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		object[] args = new object[4] { "color", color, "time", time };
		ColorUpdate(target, Hash(args));
	}

	private void ConflictCheck()
	{
		iTween[] components = GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if (iTween2.type == "value")
			{
				break;
			}
			if (!iTween2.isRunning || !(iTween2.type == type))
			{
				continue;
			}
			if (iTween2.method != method)
			{
				break;
			}
			if (iTween2.tweenArguments.Count != tweenArguments.Count)
			{
				iTween2.Dispose();
				break;
			}
			foreach (DictionaryEntry tweenArgument in tweenArguments)
			{
				if (!iTween2.tweenArguments.Contains(tweenArgument.Key))
				{
					iTween2.Dispose();
					break;
				}
				if (!iTween2.tweenArguments[tweenArgument.Key].Equals(tweenArguments[tweenArgument.Key]) && (string)tweenArgument.Key != "id")
				{
					iTween2.Dispose();
					break;
				}
			}
			Dispose();
		}
	}

	public static int Count()
	{
		return tweens.Count;
	}

	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			if (((string)hashtable["type"] + (string)hashtable["method"]).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static int Count(GameObject target)
	{
		return target.GetComponents<iTween>().Length;
	}

	public static int Count(GameObject target, string type)
	{
		int num = 0;
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	private void DisableKinematic()
	{
	}

	private void Dispose()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			if ((string)hashtable["id"] == id)
			{
				tweens.RemoveAt(i);
				break;
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	public static void DrawLine(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "handles");
		}
	}

	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "handles");
		}
	}

	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	public static void DrawPath(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "handles");
		}
	}

	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "handles");
		}
	}

	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 to = Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector = Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector, to);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			to = vector;
		}
	}

	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - easeOutBounce(0f, end, num - value) + start;
	}

	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / 6.283185f * Mathf.Asin(end / num4);
		}
		return 0f - num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.283185f / num2) + start;
	}

	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return (0f - end) * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / 6.283185f * Mathf.Asin(end / num4);
		}
		if (value < 1f)
		{
			return -0.5f * (num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.283185f / num2)) + start;
		}
		return num4 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * 6.283185f / num2) * 0.5f + end + start;
	}

	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return (0f - end) * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return (0f - end) * 0.5f * (value * value * value * value - 2f) + start;
	}

	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * 0.5f * (Mathf.Cos(3.141593f * value) - 1f) + start;
	}

	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * Mathf.Cos(value * 1.570796f) + end + start;
	}

	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.3636364f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.7272727f)
		{
			value -= 0.5454546f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.9545454f;
		return end * (7.5625f * value * value + 63f / 64f) + start;
	}

	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 * 0.25f;
		}
		else
		{
			num3 = num2 / 6.283185f * Mathf.Asin(end / num4);
		}
		return num4 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num3) * 6.283185f / num2) + end + start;
	}

	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * (value * value * value * value - 1f) + start;
	}

	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * 1.570796f) + start;
	}

	private void EnableKinematic()
	{
	}

	public static void FadeFrom(GameObject target, Hashtable args)
	{
		ColorFrom(target, args);
	}

	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		object[] args = new object[4] { "alpha", alpha, "time", time };
		FadeFrom(target, Hash(args));
	}

	public static void FadeTo(GameObject target, Hashtable args)
	{
		ColorTo(target, args);
	}

	public static void FadeTo(GameObject target, float alpha, float time)
	{
		object[] args = new object[4] { "alpha", alpha, "time", time };
		FadeTo(target, Hash(args));
	}

	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		ColorUpdate(target, args);
	}

	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		object[] args = new object[4] { "alpha", alpha, "time", time };
		FadeUpdate(target, Hash(args));
	}

	private void FixedUpdate()
	{
		if (!isRunning || !physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	private void GenerateAudioToTargets()
	{
		vector2s = new Vector2[3];
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if (GetComponent<AudioSource>() != null)
		{
			audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
			Dispose();
		}
		vector2s[0] = (vector2s[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (tweenArguments.Contains("volume"))
		{
			vector2s[1].x = (float)tweenArguments["volume"];
		}
		if (tweenArguments.Contains("pitch"))
		{
			vector2s[1].y = (float)tweenArguments["pitch"];
		}
	}

	private void GenerateColorTargets()
	{
		colors = new Color[1, 3];
		colors[0, 0] = (Color)tweenArguments["from"];
		colors[0, 1] = (Color)tweenArguments["to"];
	}

	private void GenerateColorToTargets()
	{
		if (GetComponent<GUITexture>() != null)
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = base.GetComponent<GUITexture>().color);
		}
		else if (GetComponent<GUIText>() != null)
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = base.GetComponent<GUIText>().material.color);
		}
		else if (base.GetComponent<Renderer>() != null)
		{
			colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
			{
				colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(namedcolorvalue.ToString());
				colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(namedcolorvalue.ToString());
			}
		}
		else if (base.GetComponent<Light>() != null)
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = base.GetComponent<Light>().color);
		}
		else
		{
			colors = new Color[1, 3];
		}
		if (tweenArguments.Contains("color"))
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				colors[j, 1] = (Color)tweenArguments["color"];
			}
		}
		else
		{
			if (tweenArguments.Contains("r"))
			{
				for (int k = 0; k < colors.GetLength(0); k++)
				{
					colors[k, 1].r = (float)tweenArguments["r"];
				}
			}
			if (tweenArguments.Contains("g"))
			{
				for (int l = 0; l < colors.GetLength(0); l++)
				{
					colors[l, 1].g = (float)tweenArguments["g"];
				}
			}
			if (tweenArguments.Contains("b"))
			{
				for (int m = 0; m < colors.GetLength(0); m++)
				{
					colors[m, 1].b = (float)tweenArguments["b"];
				}
			}
			if (tweenArguments.Contains("a"))
			{
				for (int n = 0; n < colors.GetLength(0); n++)
				{
					colors[n, 1].a = (float)tweenArguments["a"];
				}
			}
		}
		if (tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < colors.GetLength(0); num++)
			{
				colors[num, 1].a = (float)tweenArguments["amount"];
			}
		}
		else if (tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < colors.GetLength(0); num2++)
			{
				colors[num2, 1].a = (float)tweenArguments["alpha"];
			}
		}
	}

	private void GenerateFloatTargets()
	{
		floats = new float[3];
		floats[0] = (float)tweenArguments["from"];
		floats[1] = (float)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(floats[0] - floats[1]);
			time = num / (float)tweenArguments["speed"];
		}
	}

	private static string GenerateID()
	{
		return Guid.NewGuid().ToString();
	}

	private void GenerateLookToTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = thisTransform.eulerAngles;
		if (tweenArguments.Contains("looktarget"))
		{
			if (tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				Vector3? vector = (Vector3?)tweenArguments["up"];
				thisTransform.LookAt((Transform)tweenArguments["looktarget"], (!vector.HasValue) ? Defaults.up : vector.Value);
			}
			else if (tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				Vector3? vector2 = (Vector3?)tweenArguments["up"];
				thisTransform.LookAt((Vector3)tweenArguments["looktarget"], (!vector2.HasValue) ? Defaults.up : vector2.Value);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
			Dispose();
		}
		vector3s[1] = thisTransform.eulerAngles;
		thisTransform.eulerAngles = vector3s[0];
		if (tweenArguments.Contains("axis"))
		{
			switch ((string)tweenArguments["axis"])
			{
			case "x":
				vector3s[1].y = vector3s[0].y;
				vector3s[1].z = vector3s[0].z;
				break;
			case "y":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].z = vector3s[0].z;
				break;
			case "z":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].y = vector3s[0].y;
				break;
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveByTargets()
	{
		vector3s = new Vector3[6];
		vector3s[4] = thisTransform.eulerAngles;
		Vector3 vector = (vector3s[3] = thisTransform.position);
		vector3s[0] = (vector3s[1] = vector);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = vector3s[0] + (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = vector3s[0].x + (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = vector3s[0].y + (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = vector3s[0].z + (float)tweenArguments["z"];
			}
		}
		thisTransform.Translate(vector3s[1], space);
		vector3s[5] = thisTransform.position;
		thisTransform.position = vector3s[0];
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (thisTransform.position != array2[0])
		{
			if (!tweenArguments.Contains("movetopath") || (bool)tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			vector3s[1] = thisTransform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, vector3s, num, array2.Length);
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);
		if (vector3s[1] == vector3s[vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[vector3s.Length];
			Array.Copy(vector3s, array4, vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			vector3s = new Vector3[array4.Length];
			Array.Copy(array4, vector3s, array4.Length);
		}
		path = new CRSpline(vector3s);
		if (tweenArguments.Contains("speed"))
		{
			float num2 = PathLength(vector3s);
			time = num2 / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveToTargets()
	{
		vector3s = new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = thisTransform.localPosition);
		}
		else
		{
			vector3s[0] = (vector3s[1] = thisTransform.position);
		}
		if (tweenArguments.Contains("position"))
		{
			if (tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["position"];
				vector3s[1] = transform.position;
			}
			else if (tweenArguments["position"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["position"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GeneratePunchPositionTargets()
	{
		vector3s = new Vector3[5];
		vector3s[4] = thisTransform.eulerAngles;
		vector3s[0] = thisTransform.position;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GeneratePunchRotationTargets()
	{
		vector3s = new Vector3[4];
		vector3s[0] = thisTransform.eulerAngles;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GeneratePunchScaleTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = thisTransform.localScale;
		vector3s[1] = Vector3.zero;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateRectTargets()
	{
		rects = new Rect[3];
		rects[0] = (Rect)tweenArguments["from"];
		rects[1] = (Rect)tweenArguments["to"];
	}

	private void GenerateRotateAddTargets()
	{
		vector3s = new Vector3[5];
		Vector3 vector = (vector3s[3] = thisTransform.eulerAngles);
		vector3s[0] = (vector3s[1] = vector);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateRotateByTargets()
	{
		vector3s = new Vector3[4];
		Vector3 vector = (vector3s[3] = thisTransform.eulerAngles);
		vector3s[0] = (vector3s[1] = vector);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += Vector3.Scale((Vector3)tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += 360f * (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += 360f * (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += 360f * (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateRotateToTargets()
	{
		vector3s = new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = thisTransform.localEulerAngles);
		}
		else
		{
			vector3s[0] = (vector3s[1] = thisTransform.eulerAngles);
		}
		if (tweenArguments.Contains("rotation"))
		{
			if (tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["rotation"];
				vector3s[1] = transform.eulerAngles;
			}
			else if (tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["rotation"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleAddTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = thisTransform.localScale);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleByTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = thisTransform.localScale);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = Vector3.Scale(vector3s[1], (Vector3)tweenArguments["amount"]);
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x *= (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y *= (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z *= (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleToTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = thisTransform.localScale);
		if (tweenArguments.Contains("scale"))
		{
			if (tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["scale"];
				vector3s[1] = transform.localScale;
			}
			else if (tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["scale"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateShakePositionTargets()
	{
		vector3s = new Vector3[4];
		vector3s[3] = thisTransform.eulerAngles;
		vector3s[0] = thisTransform.position;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateShakeRotationTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = thisTransform.eulerAngles;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateShakeScaleTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = thisTransform.localScale;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateStabTargets()
	{
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if (GetComponent<AudioSource>() != null)
		{
			audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent<AudioSource>();
			audioSource = base.GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		audioSource.clip = (AudioClip)tweenArguments["audioclip"];
		if (tweenArguments.Contains("pitch"))
		{
			audioSource.pitch = (float)tweenArguments["pitch"];
		}
		if (tweenArguments.Contains("volume"))
		{
			audioSource.volume = (float)tweenArguments["volume"];
		}
		time = audioSource.clip.length / audioSource.pitch;
	}

	private void GenerateTargets()
	{
		int value;
		switch (type)
		{
		case "value":
			switch (method)
			{
			case "float":
				GenerateFloatTargets();
				apply = ApplyFloatTargets;
				break;
			case "vector2":
				GenerateVector2Targets();
				apply = ApplyVector2Targets;
				break;
			case "vector3":
				GenerateVector3Targets();
				apply = ApplyVector3Targets;
				break;
			case "color":
				GenerateColorTargets();
				apply = ApplyColorTargets;
				break;
			case "rect":
				GenerateRectTargets();
				apply = ApplyRectTargets;
				break;
			}
			break;
		case "color":
		{
			string text = method;
			if (text != null)
			{
				if (fswitchSmapA == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("to", 0);
					fswitchSmapA = dictionary;
				}
				if (fswitchSmapA.TryGetValue(text, out value) && value == 0)
				{
					GenerateColorToTargets();
					apply = ApplyColorToTargets;
				}
			}
			break;
		}
		case "audio":
		{
			string text = method;
			if (text != null)
			{
				if (fswitchSmapB == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("to", 0);
					fswitchSmapB = dictionary;
				}
				if (fswitchSmapB.TryGetValue(text, out value) && value == 0)
				{
					GenerateAudioToTargets();
					apply = ApplyAudioToTargets;
				}
			}
			break;
		}
		case "move":
			switch (method)
			{
			case "to":
				if (tweenArguments.Contains("path"))
				{
					GenerateMoveToPathTargets();
					apply = ApplyMoveToPathTargets;
				}
				else
				{
					GenerateMoveToTargets();
					apply = ApplyMoveToTargets;
				}
				break;
			case "by":
			case "add":
				GenerateMoveByTargets();
				apply = ApplyMoveByTargets;
				break;
			}
			break;
		case "scale":
			switch (method)
			{
			case "to":
				GenerateScaleToTargets();
				apply = ApplyScaleToTargets;
				break;
			case "by":
				GenerateScaleByTargets();
				apply = ApplyScaleToTargets;
				break;
			case "add":
				GenerateScaleAddTargets();
				apply = ApplyScaleToTargets;
				break;
			}
			break;
		case "rotate":
			switch (method)
			{
			case "to":
				GenerateRotateToTargets();
				apply = ApplyRotateToTargets;
				break;
			case "add":
				GenerateRotateAddTargets();
				apply = ApplyRotateAddTargets;
				break;
			case "by":
				GenerateRotateByTargets();
				apply = ApplyRotateAddTargets;
				break;
			}
			break;
		case "shake":
			switch (method)
			{
			case "position":
				GenerateShakePositionTargets();
				apply = ApplyShakePositionTargets;
				break;
			case "scale":
				GenerateShakeScaleTargets();
				apply = ApplyShakeScaleTargets;
				break;
			case "rotation":
				GenerateShakeRotationTargets();
				apply = ApplyShakeRotationTargets;
				break;
			}
			break;
		case "punch":
			switch (method)
			{
			case "position":
				GeneratePunchPositionTargets();
				apply = ApplyPunchPositionTargets;
				break;
			case "rotation":
				GeneratePunchRotationTargets();
				apply = ApplyPunchRotationTargets;
				break;
			case "scale":
				GeneratePunchScaleTargets();
				apply = ApplyPunchScaleTargets;
				break;
			}
			break;
		case "look":
		{
			string text = method;
			if (text != null)
			{
				if (fswitchSmap11 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
					dictionary.Add("to", 0);
					fswitchSmap11 = dictionary;
				}
				if (fswitchSmap11.TryGetValue(text, out value) && value == 0)
				{
					GenerateLookToTargets();
					apply = ApplyLookToTargets;
				}
			}
			break;
		}
		case "stab":
			GenerateStabTargets();
			apply = ApplyStabTargets;
			break;
		}
	}

	private void GenerateVector2Targets()
	{
		vector2s = new Vector2[3];
		vector2s[0] = (Vector2)tweenArguments["from"];
		vector2s[1] = (Vector2)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			Vector3 a = new Vector3(vector2s[0].x, vector2s[0].y, 0f);
			Vector3 b = new Vector3(vector2s[1].x, vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(a, b));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateVector3Targets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (Vector3)tweenArguments["from"];
		vector3s[1] = (Vector3)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GetEasingFunction()
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			ease = easeInQuad;
			break;
		case EaseType.easeOutQuad:
			ease = easeOutQuad;
			break;
		case EaseType.easeInOutQuad:
			ease = easeInOutQuad;
			break;
		case EaseType.easeInCubic:
			ease = easeInCubic;
			break;
		case EaseType.easeOutCubic:
			ease = easeOutCubic;
			break;
		case EaseType.easeInOutCubic:
			ease = easeInOutCubic;
			break;
		case EaseType.easeInQuart:
			ease = easeInQuart;
			break;
		case EaseType.easeOutQuart:
			ease = easeOutQuart;
			break;
		case EaseType.easeInOutQuart:
			ease = easeInOutQuart;
			break;
		case EaseType.easeInQuint:
			ease = easeInQuint;
			break;
		case EaseType.easeOutQuint:
			ease = easeOutQuint;
			break;
		case EaseType.easeInOutQuint:
			ease = easeInOutQuint;
			break;
		case EaseType.easeInSine:
			ease = easeInSine;
			break;
		case EaseType.easeOutSine:
			ease = easeOutSine;
			break;
		case EaseType.easeInOutSine:
			ease = easeInOutSine;
			break;
		case EaseType.easeInExpo:
			ease = easeInExpo;
			break;
		case EaseType.easeOutExpo:
			ease = easeOutExpo;
			break;
		case EaseType.easeInOutExpo:
			ease = easeInOutExpo;
			break;
		case EaseType.easeInCirc:
			ease = easeInCirc;
			break;
		case EaseType.easeOutCirc:
			ease = easeOutCirc;
			break;
		case EaseType.easeInOutCirc:
			ease = easeInOutCirc;
			break;
		case EaseType.linear:
			ease = linear;
			break;
		case EaseType.spring:
			ease = spring;
			break;
		case EaseType.easeInBounce:
			ease = easeInBounce;
			break;
		case EaseType.easeOutBounce:
			ease = easeOutBounce;
			break;
		case EaseType.easeInOutBounce:
			ease = easeInOutBounce;
			break;
		case EaseType.easeInBack:
			ease = easeInBack;
			break;
		case EaseType.easeOutBack:
			ease = easeOutBack;
			break;
		case EaseType.easeInOutBack:
			ease = easeInOutBack;
			break;
		case EaseType.easeInElastic:
			ease = easeInElastic;
			break;
		case EaseType.easeOutElastic:
			ease = easeOutElastic;
			break;
		case EaseType.easeInOutElastic:
			ease = easeInOutElastic;
			break;
		}
	}

	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	public static void Init(GameObject target)
	{
		MoveBy(target, Vector3.zero, 0f);
	}

	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	private void LateUpdate()
	{
		if (tweenArguments.Contains("looktarget") && isRunning && (type == "move" || type == "shake" || type == "punch"))
		{
			LookUpdate(base.gameObject, tweenArguments);
		}
	}

	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		tweens.Insert(0, args);
		target.AddComponent<iTween>();
	}

	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			Vector3? vector = (Vector3?)args["up"];
			target.transform.LookAt((Transform)args["looktarget"], (!vector.HasValue) ? Defaults.up : vector.Value);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			Vector3? vector2 = (Vector3?)args["up"];
			target.transform.LookAt((Vector3)args["looktarget"], (!vector2.HasValue) ? Defaults.up : vector2.Value);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			switch ((string)args["axis"])
			{
			case "x":
				eulerAngles2.y = eulerAngles.y;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "y":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "z":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.y = eulerAngles.y;
				break;
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		object[] args = new object[4] { "looktarget", looktarget, "time", time };
		LookFrom(target, Hash(args));
	}

	public static void LookTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		object[] args = new object[4] { "looktarget", looktarget, "time", time };
		LookTo(target, Hash(args));
	}

	public static void LookUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				Vector3? vector = (Vector3?)args["up"];
				target.transform.LookAt((Transform)args["looktarget"], (!vector.HasValue) ? Defaults.up : vector.Value);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				Vector3? vector2 = (Vector3?)args["up"];
				target.transform.LookAt((Vector3)args["looktarget"], (!vector2.HasValue) ? Defaults.up : vector2.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				switch ((string)args["axis"])
				{
				case "x":
					array[4].y = array[0].y;
					array[4].z = array[0].z;
					break;
				case "y":
					array[4].x = array[0].x;
					array[4].z = array[0].z;
					break;
				case "z":
					array[4].x = array[0].x;
					array[4].y = array[0].y;
					break;
				}
				target.transform.eulerAngles = array[4];
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
		}
	}

	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		object[] args = new object[4] { "looktarget", looktarget, "time", time };
		LookUpdate(target, Hash(args));
	}

	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		MoveAdd(target, Hash(args));
	}

	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		MoveBy(target, Hash(args));
	}

	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector;
			Vector3 vector2 = ((!flag) ? (vector = target.transform.position) : (vector = target.transform.localPosition));
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					Transform transform = (Transform)args["position"];
					vector = transform.position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		object[] args = new object[4] { "position", position, "time", time };
		MoveFrom(target, Hash(args));
	}

	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		object[] args = new object[4] { "position", position, "time", time };
		MoveTo(target, Hash(args));
	}

	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		object[] args = new object[4] { "position", position, "time", time };
		MoveUpdate(target, Hash(args));
	}

	private void OnDisable()
	{
		DisableKinematic();
	}

	private void OnEnable()
	{
		if (isRunning)
		{
			EnableKinematic();
		}
		if (isPaused)
		{
			isPaused = false;
			if (delay > 0f)
			{
				wasPaused = true;
				ResumeDelay();
			}
		}
	}

	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = PathControlPointGenerator(array);
		Vector3 a = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector = Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 a = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector = Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	public static void Pause()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			Pause(target);
		}
	}

	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Pause((GameObject)arrayList[j], type);
		}
	}

	public static void Pause(GameObject target)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if (iTween2.delay > 0f)
			{
				iTween2.delay -= Time.time - iTween2.delayStarted;
				iTween2.StopCoroutine("TweenDelay");
			}
			iTween2.isPaused = true;
			iTween2.enabled = false;
		}
	}

	public static void Pause(GameObject target, bool includechildren)
	{
		Pause(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Pause(item.gameObject, true);
		}
	}

	public static void Pause(GameObject target, string type)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					iTween2.StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				iTween2.enabled = false;
			}
		}
	}

	public static void Pause(GameObject target, string type, bool includechildren)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					iTween2.StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				iTween2.enabled = false;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Pause(item.gameObject, type, true);
		}
	}

	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return Interp(PathControlPointGenerator(array), percent);
	}

	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return Interp(PathControlPointGenerator(path), percent);
	}

	private float punch(float amplitude, float value)
	{
		float num = 9f;
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num2 = 0.3f;
		num = num2 / 6.283185f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num) * 6.283185f / num2);
	}

	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		PunchPosition(target, Hash(args));
	}

	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		PunchRotation(target, Hash(args));
	}

	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		PunchScale(target, Hash(args));
	}

	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		return new Rect(FloatUpdate(currentValue.x, targetValue.x, speed), FloatUpdate(currentValue.y, targetValue.y, speed), FloatUpdate(currentValue.width, targetValue.width, speed), FloatUpdate(currentValue.height, targetValue.height, speed));
	}

	public static void Resume()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			Resume(target);
		}
	}

	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Resume((GameObject)arrayList[j], type);
		}
	}

	public static void Resume(GameObject target)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			iTween2.enabled = true;
		}
	}

	public static void Resume(GameObject target, bool includechildren)
	{
		Resume(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Resume(item.gameObject, true);
		}
	}

	public static void Resume(GameObject target, string type)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.enabled = true;
			}
		}
	}

	public static void Resume(GameObject target, string type, bool includechildren)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.enabled = true;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Resume(item.gameObject, type, true);
		}
	}

	private void ResumeDelay()
	{
		StartCoroutine("TweenDelay");
	}

	private void RetrieveArgs()
	{
		foreach (Hashtable tween in tweens)
		{
			if ((GameObject)tween["target"] == base.gameObject)
			{
				tweenArguments = tween;
				break;
			}
		}
		id = (string)tweenArguments["id"];
		type = (string)tweenArguments["type"];
		_name = (string)tweenArguments["name"];
		method = (string)tweenArguments["method"];
		if (tweenArguments.Contains("time"))
		{
			time = (float)tweenArguments["time"];
		}
		else
		{
			time = Defaults.time;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			physics = true;
		}
		if (tweenArguments.Contains("delay"))
		{
			delay = (float)tweenArguments["delay"];
		}
		else
		{
			delay = Defaults.delay;
		}
		if (tweenArguments.Contains("namedcolorvalue"))
		{
			if (tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
			{
				namedcolorvalue = (NamedValueColor)(int)tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					namedcolorvalue = (NamedValueColor)(int)Enum.Parse(typeof(NamedValueColor), (string)tweenArguments["namedcolorvalue"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					namedcolorvalue = NamedValueColor._Color;
				}
			}
		}
		else
		{
			namedcolorvalue = Defaults.namedColorValue;
		}
		if (tweenArguments.Contains("looptype"))
		{
			if (tweenArguments["looptype"].GetType() == typeof(LoopType))
			{
				loopType = (LoopType)(int)tweenArguments["looptype"];
			}
			else
			{
				try
				{
					loopType = (LoopType)(int)Enum.Parse(typeof(LoopType), (string)tweenArguments["looptype"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
					loopType = LoopType.none;
				}
			}
		}
		else
		{
			loopType = LoopType.none;
		}
		if (tweenArguments.Contains("easetype"))
		{
			if (tweenArguments["easetype"].GetType() == typeof(EaseType))
			{
				easeType = (EaseType)(int)tweenArguments["easetype"];
			}
			else
			{
				try
				{
					easeType = (EaseType)(int)Enum.Parse(typeof(EaseType), (string)tweenArguments["easetype"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
					easeType = Defaults.easeType;
				}
			}
		}
		else
		{
			easeType = Defaults.easeType;
		}
		if (tweenArguments.Contains("space"))
		{
			if (tweenArguments["space"].GetType() == typeof(Space))
			{
				space = (Space)(int)tweenArguments["space"];
			}
			else
			{
				try
				{
					space = (Space)(int)Enum.Parse(typeof(Space), (string)tweenArguments["space"], true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
					space = Defaults.space;
				}
			}
		}
		else
		{
			space = Defaults.space;
		}
		if (tweenArguments.Contains("islocal"))
		{
			isLocal = (bool)tweenArguments["islocal"];
		}
		else
		{
			isLocal = Defaults.isLocal;
		}
		if (tweenArguments.Contains("ignoretimescale"))
		{
			useRealTime = (bool)tweenArguments["ignoretimescale"];
		}
		else
		{
			useRealTime = Defaults.useRealTime;
		}
		GetEasingFunction();
	}

	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		RotateAdd(target, Hash(args));
	}

	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		RotateBy(target, Hash(args));
	}

	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		Vector3 vector;
		Vector3 vector2 = ((!flag) ? (vector = target.transform.eulerAngles) : (vector = target.transform.localEulerAngles));
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				vector = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		object[] args = new object[4] { "rotation", rotation, "time", time };
		RotateFrom(target, Hash(args));
	}

	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		object[] args = new object[4] { "rotation", rotation, "time", time };
		RotateTo(target, Hash(args));
	}

	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		object[] args = new object[4] { "rotation", rotation, "time", time };
		RotateUpdate(target, Hash(args));
	}

	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		ScaleAdd(target, Hash(args));
	}

	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		ScaleBy(target, Hash(args));
	}

	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		Vector3 localScale;
		Vector3 vector = (localScale = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				localScale = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = vector;
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		object[] args = new object[4] { "scale", scale, "time", time };
		ScaleFrom(target, Hash(args));
	}

	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		object[] args = new object[4] { "scale", scale, "time", time };
		ScaleTo(target, Hash(args));
	}

	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		object[] args = new object[4] { "scale", scale, "time", time };
		ScaleUpdate(target, Hash(args));
	}

	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		Launch(target, args);
	}

	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		ShakePosition(target, Hash(args));
	}

	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		Launch(target, args);
	}

	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		ShakeRotation(target, Hash(args));
	}

	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		Launch(target, args);
	}

	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		object[] args = new object[4] { "amount", amount, "time", time };
		ShakeScale(target, Hash(args));
	}

	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.141593f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public static void Stab(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "stab";
		Launch(target, args);
	}

	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		object[] args = new object[4] { "audioclip", audioclip, "delay", delay };
		Stab(target, Hash(args));
	}

	private IEnumerator Start()
	{
		if (!(delay <= 0f))
		{
			yield return StartCoroutine("TweenDelay");
		}
		TweenStart();
	}

	public static void Stop()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			Stop(target);
		}
		tweens.Clear();
	}

	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Stop((GameObject)arrayList[j], type);
		}
	}

	public static void Stop(GameObject target)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			iTween2.Dispose();
		}
	}

	public static void Stop(GameObject target, bool includechildren)
	{
		Stop(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Stop(item.gameObject, true);
		}
	}

	public static void Stop(GameObject target, string type)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
	}

	public static void Stop(GameObject target, string type, bool includechildren)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Stop(item.gameObject, type, true);
		}
	}

	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			StopByName((GameObject)arrayList[j], name);
		}
	}

	public static void StopByName(GameObject target, string name)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
	}

	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		iTween[] components = target.GetComponents<iTween>();
		foreach (iTween iTween2 in components)
		{
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			StopByName(item.gameObject, name, true);
		}
	}

	private void TweenComplete()
	{
		isRunning = false;
		if (percentage > 0.5f)
		{
			percentage = 1f;
		}
		else
		{
			percentage = 0f;
		}
		apply();
		if (type == "value")
		{
			CallBack("onupdate");
		}
		if (loopType == LoopType.none)
		{
			Dispose();
		}
		else
		{
			TweenLoop();
		}
		CallBack("oncomplete");
	}

	private IEnumerator TweenDelay()
	{
		delayStarted = Time.time;
		yield return new WaitForSeconds(delay);
		if (wasPaused)
		{
			wasPaused = false;
			TweenStart();
		}
	}

	private void TweenLoop()
	{
		DisableKinematic();
		switch (loopType)
		{
		case LoopType.loop:
			percentage = 0f;
			runningTime = 0f;
			apply();
			StartCoroutine("TweenRestart");
			break;
		case LoopType.pingPong:
			reverse = !reverse;
			runningTime = 0f;
			StartCoroutine("TweenRestart");
			break;
		}
	}

	private IEnumerator TweenRestart()
	{
		if (!(delay <= 0f))
		{
			delayStarted = Time.time;
			yield return new WaitForSeconds(delay);
		}
		loop = true;
		TweenStart();
	}

	private void TweenStart()
	{
		CallBack("onstart");
		if (!loop)
		{
			ConflictCheck();
			GenerateTargets();
		}
		if (type == "stab")
		{
			audioSource.PlayOneShot(audioSource.clip);
		}
		if (type == "move" || type == "scale" || type == "rotate" || type == "punch" || type == "shake" || type == "curve" || type == "look")
		{
			EnableKinematic();
		}
		isRunning = true;
	}

	private void TweenUpdate()
	{
		apply();
		CallBack("onupdate");
		UpdatePercentage();
	}

	private void Update()
	{
		if (!isRunning || physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	private void UpdatePercentage()
	{
		if (useRealTime)
		{
			runningTime += Time.realtimeSinceStartup - lastRealTime;
		}
		else
		{
			runningTime += Time.deltaTime;
		}
		if (reverse)
		{
			percentage = 1f - runningTime / time;
		}
		else
		{
			percentage = runningTime / time;
		}
		lastRealTime = Time.realtimeSinceStartup;
	}

	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		Launch(target, args);
	}

	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}
}