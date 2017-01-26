using System.Collections.Generic;
using UnityEngine;

public class TankControllerPUN : Photon.MonoBehaviour {

    public List<MonoBehaviour> m_ComponentsToEnable;
    public Color[] m_Colors;

    private string m_PlayerName;

    void Start()
    {
        if (photonView.isMine)
        {
            photonView.RPC("SetplayerName", PhotonTargets.All, PhotonNetwork.player.NickName);
            photonView.RPC("Paint", PhotonTargets.All, (int)PhotonNetwork.player.CustomProperties["number"]);
            m_ComponentsToEnable.ForEach(component => component.enabled = true);
        }
    }

    public string GetPlayerName()
    {
        return m_PlayerName;
    }

    [PunRPC]
    public void Paint(int index)
    {
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_Colors[index - 1];
        }
    }

    [PunRPC]
    public void SetplayerName(string name)
    {
        m_PlayerName = name;
    }

}
