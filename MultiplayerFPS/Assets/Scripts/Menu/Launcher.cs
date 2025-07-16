using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.instance.OpenMenu("Loading");
    }


    #region Overrides Methods

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to master server...");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("Title");
        Debug.Log("Joined lobby...");
        PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("Room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed " + message;
        MenuManager.instance.OpenMenu("Error");
    }

    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("Title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }


    #endregion

    #region Public Methods

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("Loading");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("Loading");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion


    #region Button Methods

    public void OnTitleCreateRoomClicked()
    {
        MenuManager.instance.OpenMenu("CreateRoom");
    }

    public void OnCRCreateRoomClicked()
    {
        MenuManager.instance.OpenMenu("Room");
        CreateRoom();
    }



    #endregion

}
