using UnityEngine;

public class MovementUpdate1 : MonoBehaviour
{
	public bool disabled;

	private Vector3 lastPosition;

	private Quaternion lastRotation;

	private Vector3 lastVelocity;

	private void Start()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			disabled = true;
			base.enabled = false;
		}
		else if (base.GetComponent<NetworkView>().isMine)
		{
			object[] args = new object[3]
			{
				base.transform.position,
				base.transform.rotation,
				base.transform.lossyScale
			};
			base.GetComponent<NetworkView>().RPC("updateMovement1", RPCMode.OthersBuffered, args);
		}
		else
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		if (!disabled)
		{
			object[] args = new object[3]
			{
				base.transform.position,
				base.transform.rotation,
				base.transform.lossyScale
			};
			base.GetComponent<NetworkView>().RPC("updateMovement1", RPCMode.Others, args);
		}
	}

	[RPC]
	private void updateMovement1(Vector3 newPosition, Quaternion newRotation, Vector3 newScale)
	{
		base.transform.position = newPosition;
		base.transform.rotation = newRotation;
		base.transform.localScale = newScale;
	}
}
