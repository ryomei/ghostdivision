using UnityEngine;

public class TankUpdatePositionPUN : Photon.MonoBehaviour {
    
    private Vector3 realPosition = Vector3.zero;
    private Quaternion realRotation = Quaternion.identity;

    public void Update () {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);            
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {        
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
