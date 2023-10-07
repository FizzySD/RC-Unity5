using UnityEngine;

public class ParticleScaling : MonoBehaviour
{
	public void OnWillRenderObject()
	{
		GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_Center", base.transform.position);
		GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetVector("_Scaling", base.transform.lossyScale);
		GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
		GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetMatrix("_CameraInv", Camera.current.worldToCameraMatrix.inverse);
	}
}
