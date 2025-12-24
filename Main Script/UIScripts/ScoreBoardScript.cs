using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ScoreBoardScript : MonoBehaviour
{
    public static ScoreBoardScript instance;

    public Text blueTeamText;

    public Text redTeamText;

    public int blueTeamScore = 0;

    public int redTeamScore = 0;

    private PhotonView view;

    public Text messageText;

    void Awake()
    {
        view = GetComponent<PhotonView>();
        instance = this;
    }

    void Update()
    {
        if (blueTeamScore >= 5)
        {
            StartCoroutine(DisplayMessage("Team 1 Wins"));
        }
        if (redTeamScore >= 5)
        {
            StartCoroutine(DisplayMessage("Team 2 Wins"));
        }
    }

    IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        yield return new WaitForSeconds(5);
        //Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(2);
    }

    public void PlayerDied(int playerTeam)
    {
        if (playerTeam == 2)
        {
            blueTeamScore++;
        }

        if (playerTeam == 1)
        {
            redTeamScore++;
        }

        view.RPC("UpdateScores", RpcTarget.All, blueTeamScore, redTeamScore);
    }

    [PunRPC]
    void UpdateScores(int blueScore, int redScore)
    {
        blueTeamScore = blueScore;
        redTeamScore = redScore;

        blueTeamText.text = blueTeamScore.ToString();
        redTeamText.text = redTeamScore.ToString();
    }
}
