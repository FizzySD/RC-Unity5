using System.Collections.Generic;
using Photon;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PickupItem : Photon.MonoBehaviour, IPunObservable
{
	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();

	public UnityEngine.MonoBehaviour OnPickedUpCall;

	public bool PickupIsMine;

	public bool PickupOnTrigger;

	public float SecondsBeforeRespawn = 2f;

	public bool SentPickup;

	public double TimeOfRespawn;

	public int ViewID
	{
		get
		{
			return base.photonView.viewID;
		}
	}

	public void Drop()
	{
		if (PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer);
		}
	}

	public void Drop(Vector3 newPosition)
	{
		if (PickupIsMine)
		{
			object[] parameters = new object[1] { newPosition };
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, parameters);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && SecondsBeforeRespawn <= 0f)
		{
			stream.SendNext(base.gameObject.transform.position);
			return;
		}
		Vector3 position = (Vector3)stream.ReceiveNext();
		base.gameObject.transform.position = position;
	}

	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (PickupOnTrigger && component != null && component.isMine)
		{
			Pickup();
		}
	}

	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		DisabledPickupItems.Add(this);
		TimeOfRespawn = 0.0;
		if (timeUntilRespawn > 0f)
		{
			TimeOfRespawn = PhotonNetwork.time + (double)timeUntilRespawn;
			Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	public void Pickup()
	{
		if (!SentPickup)
		{
			SentPickup = true;
			base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer);
		}
	}

	[RPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender.isLocal)
		{
			SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat("Ignored PU RPC, cause item is inactive. ", base.gameObject, " SecondsBeforeRespawn: ", SecondsBeforeRespawn, " TimeOfRespawn: ", TimeOfRespawn, " respawn in future: ", TimeOfRespawn > PhotonNetwork.time));
			return;
		}
		PickupIsMine = msgInfo.sender.isLocal;
		if (OnPickedUpCall != null)
		{
			OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (SecondsBeforeRespawn <= 0f)
		{
			PickedUp(0f);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		double num2 = (double)SecondsBeforeRespawn - num;
		if (num2 > 0.0)
		{
			PickedUp((float)num2);
		}
	}

	[RPC]
	internal void PunRespawn()
	{
		DisabledPickupItems.Remove(this);
		TimeOfRespawn = 0.0;
		PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	[RPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		PunRespawn();
		base.gameObject.transform.position = pos;
	}
}
