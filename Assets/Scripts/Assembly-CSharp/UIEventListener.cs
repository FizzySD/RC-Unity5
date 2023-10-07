using UnityEngine;

[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	public delegate void BoolDelegate(GameObject go, bool state);

	public delegate void FloatDelegate(GameObject go, float delta);

	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);

	public delegate void StringDelegate(GameObject go, string text);

	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	public delegate void VoidDelegate(GameObject go);

	public VoidDelegate onClick;

	public VoidDelegate onDoubleClick;

	public VectorDelegate onDrag;

	public ObjectDelegate onDrop;

	public BoolDelegate onHover;

	public StringDelegate onInput;

	public KeyCodeDelegate onKey;

	public BoolDelegate onPress;

	public FloatDelegate onScroll;

	public BoolDelegate onSelect;

	public VoidDelegate onSubmit;

	public object parameter;

	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uIEventListener = go.GetComponent<UIEventListener>();
		if (uIEventListener == null)
		{
			uIEventListener = go.AddComponent<UIEventListener>();
		}
		return uIEventListener;
	}

	private void OnClick()
	{
		if (onClick != null)
		{
			onClick(base.gameObject);
		}
	}

	private void OnDoubleClick()
	{
		if (onDoubleClick != null)
		{
			onDoubleClick(base.gameObject);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (onDrag != null)
		{
			onDrag(base.gameObject, delta);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (onDrop != null)
		{
			onDrop(base.gameObject, go);
		}
	}

	private void OnHover(bool isOver)
	{
		if (onHover != null)
		{
			onHover(base.gameObject, isOver);
		}
	}

	private void OnInput(string text)
	{
		if (onInput != null)
		{
			onInput(base.gameObject, text);
		}
	}

	private void OnKey(KeyCode key)
	{
		if (onKey != null)
		{
			onKey(base.gameObject, key);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (onPress != null)
		{
			onPress(base.gameObject, isPressed);
		}
	}

	private void OnScroll(float delta)
	{
		if (onScroll != null)
		{
			onScroll(base.gameObject, delta);
		}
	}

	private void OnSelect(bool selected)
	{
		if (onSelect != null)
		{
			onSelect(base.gameObject, selected);
		}
	}

	private void OnSubmit()
	{
		if (onSubmit != null)
		{
			onSubmit(base.gameObject);
		}
	}
}
