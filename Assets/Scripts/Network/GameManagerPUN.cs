using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManagerPUN : MonoBehaviour {

    public string m_RoomName;    
    public string m_PlayerPrefabName = "TankPUN";    
    public List<Transform> m_SpawnPoints;
    public Text m_UILog;
    
    private int m_PlayerNumber;
    private string m_winnercandidate;

    const string VERSION = "v0.0.1";

	public void Start ()
    {
        StartCoroutine(GameLoop());
    }

    public void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundDelay());
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    private IEnumerator RoundDelay()
    {
        yield return new WaitForSeconds(5);
    }

    IEnumerator RoundStarting()
    {
        var index = (int)PhotonNetwork.player.CustomProperties["number"];
        var spawnPoint = m_SpawnPoints[index];
        var tank = PhotonNetwork.Instantiate(m_PlayerPrefabName, spawnPoint.position, spawnPoint.rotation, 0);

        yield return new WaitForSeconds(5);
    }

    IEnumerator RoundPlaying()
    {
        while (!ShouldEnd()) yield return null;
    }

    IEnumerator RoundEnding()
    {
        if (m_winnercandidate != null)
        {
            var gratz = new List<string> {
                "Você domina, {0}!!!",
                "{0}, você é expetacular!",
                "Quem manda no pedaço? {0} manda!",
                "Mandou bem, {0}!",
                "{0}, você detonou!"
            };
            m_winnercandidate = string.Format(gratz.OrderBy(x => System.Guid.NewGuid()).FirstOrDefault(), m_winnercandidate);
        }
        else
        {
            m_winnercandidate = "Não há vencedor";
        }
        
        m_UILog.text = m_winnercandidate;
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Disconnect();        
    }

    void OnDisconnectedFromPhoton()
    {
        SceneManager.LoadScene("Lobby");
    }

    private bool ShouldEnd()
    {
        var tanks = FindObjectsOfType<TankControllerPUN>();
        var activeTanks = 0;

        foreach (var tank in tanks)
        {
            if (tank.gameObject.activeSelf)
            {
                activeTanks++;
                m_winnercandidate = tank.GetPlayerName();
            } 
        }

        if (activeTanks == 0)
        {
            m_winnercandidate = null;
            return true;
        }

        var maxPlayers = PhotonNetwork.room.MaxPlayers;

        if (activeTanks < 1) return true;
        if (activeTanks < 2 && maxPlayers > 1) return true;

       return false;
    }
}
