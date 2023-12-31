using UnityEngine;

[AddComponentMenu("NGUI/Tween/Orthographic Size")]
[RequireComponent(typeof(Camera))]
public class TweenOrthoSize : UITweener
{
	public float from;

	private Camera mCam;

	public float to;

	public Camera cachedCamera
	{
		get
		{
			if (mCam == null)
			{
				mCam = base.GetComponent<Camera>();
			}
			return mCam;
		}
	}

	public float orthoSize
	{
		get
		{
			return cachedCamera.orthographicSize;
		}
		set
		{
			cachedCamera.orthographicSize = value;
		}
	}

	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.orthoSize;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		cachedCamera.orthographicSize = from * (1f - factor) + to * factor;
	}
}
