using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;

public class LancherScript : MonoBehaviourPunCallbacks
{
    public static LancherScript instance;

    [SerializeField] InputField roomNameInputField;

    [SerializeField] Text roomNameText;

    [SerializeField] Text errorText;

    [SerializeField] Transform roomListContent;

    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] Transform playerListContent;

    [SerializeField] GameObject playerListItemPrefab;

    public GameObject startButton;

    int nextTeamNumber = 1;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Debug.Log("Connecting to Master...!");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        //PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /*public override void OnDisable()
    {
        base.OnDisable();
    }*/

    public override void OnJoinedLobby()
    {
        MenuManagerScript.instance.OpenMenu("UserNameMenu");
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManagerScript.instance.OpenMenu("LoadingMenu");
    }

    public override void OnJoinedRoom()
    {
        MenuManagerScript.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i ++)
        {
            int teamNumber = GetNextTeamNumber();

            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItemScript>().SetUp(players[i], teamNumber);
        }

        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string errorMessage)
    {
        errorText.text = "Create Room Unsuccessfull" + errorMessage;
        MenuManagerScript.instance.OpenMenu("ErrorMenu");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManagerScript.instance.OpenMenu("LoadingMenu");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManagerScript.instance.OpenMenu("LoadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuManagerScript.instance.OpenMenu("TitleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItemScript>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int teamNumber = GetNextTeamNumber();

        GameObject playerItem = Instantiate(playerListItemPrefab, playerListContent);

        playerItem.GetComponent<PlayerListItemScript>().SetUp(newPlayer, teamNumber);
    }

    private int GetNextTeamNumber()
    {
        int teamNumber = nextTeamNumber;

        nextTeamNumber = 3 - nextTeamNumber;

        return teamNumber;
    }

    public void ExitGame()
    {
        Debug.Log("EXIT GAME!");
        Application.Quit();
    }
}
