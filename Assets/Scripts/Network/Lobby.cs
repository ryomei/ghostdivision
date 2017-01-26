using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : Photon.PunBehaviour 
{
    public Text m_RoomName;
    public Text m_MaxPlayers;
    public Text m_PlayerNickname;
    public Text m_UILog;
    public Button m_CreateRoomBtn;
    public Button m_LeaveRoomBtn;
    public ScrollRect m_ActiveRooms;

    const string VERSION = "v0.0.1";

    public void Start()
    {
        m_UILog.text = "Connecting to server";
        PhotonNetwork.ConnectUsingSettings(VERSION);
        SetListeners();
    }

    public void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        m_UILog.text = "Connected to server, joining Lobby...";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        m_UILog.text = "Connected to lobby";
    }

    void SetListeners()
    {
        m_CreateRoomBtn.onClick.AddListener(CreateRoom);
        m_LeaveRoomBtn.onClick.AddListener(LeaveRoom);
    }

    void CreateRoom()
    {
        if (!PhotonNetwork.insideLobby) return;
        var roomName = "SabatonArtofWar";
        if (!string.IsNullOrEmpty(m_RoomName.text))
        {
            roomName = m_RoomName.text;
        }
        var maxPlayers = 4;
        int.TryParse(m_MaxPlayers.text, out maxPlayers);
        var roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = (byte)maxPlayers, IsOpen = true };
        
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        m_UILog.text = "Left room, back on lobby.";
    }

    public override void OnCreatedRoom()
    {
        m_UILog.text = "Room created";
    }

    public override void OnJoinedRoom()
    {
        m_UILog.text = "Joined room";
        PhotonNetwork.player.NickName = string.IsNullOrEmpty(m_PlayerNickname.text) ? "Player" + PhotonNetwork.playerList.Length : m_PlayerNickname.text;
        PhotonNetwork.player.CustomProperties["number"] = PhotonNetwork.playerList.Length;
        StartCoroutine(UpdateRoomPlayers());
    }

    IEnumerator UpdateRoomPlayers()
    {
        while(PhotonNetwork.room.MaxPlayers > PhotonNetwork.playerList.Length)
        {
            m_UILog.text = string.Format("Jogadores nesta sala ({0}/{1}):{2}", PhotonNetwork.playerList.Length, PhotonNetwork.room.MaxPlayers, Environment.NewLine);

            foreach (var player in PhotonNetwork.playerList)
            {
                m_UILog.text += player.NickName + Environment.NewLine;
            }
            yield return new WaitForSeconds(3);
        }
        Init();
    }

    void Init()
    {
        SceneManager.LoadScene("PhotonMultiplayer");
    }
    
}
