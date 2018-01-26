using Photon;
using UnityEngine;
using UnityEngine.UI;

public class Connect : PunBehaviour
{
  public InputField usernameInput;
  public GameObject gameControllerObj;
  private GameController gameController;

  private const string AppVersion = "0.1";

  void Awake()
  {
    gameController = gameControllerObj.GetComponent<GameController>();
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("On Joined Room");
    if (PhotonNetwork.room.PlayerCount == 2)
    {
      gameController.StartGame();
    }
    else
    {
      ToastManager toastManager = FindObjectOfType<ToastManager>();
      toastManager.Toast("Connected, waiting for other player to join...");
    }
  }

  public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
  {
    Debug.Log("Other player arrived");

    if (PhotonNetwork.room.PlayerCount == 2)
    {
      gameController.StartGame();
    }
  }

  public void ConnectToServer()
  {
    ToastManager toastManager = FindObjectOfType<ToastManager>();
    string nickName = usernameInput.text;
    if (string.IsNullOrEmpty(nickName))
    {
      toastManager.Toast("Empty username");
      return;
    }
    PhotonNetwork.playerName = nickName;
    toastManager.Toast("Connecting...");
    PhotonNetwork.ConnectUsingSettings(AppVersion);
  }

  public override void OnConnectedToMaster()
  {
    Debug.Log("OnConnectedToMaster");
    PhotonNetwork.JoinRandomRoom();
  }

  public override void OnJoinedLobby()
  {
    Debug.Log("OnJoinedLobby");
    PhotonNetwork.JoinRandomRoom();
  }

  public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
  {
    Debug.Log("OnPhotonRandomJoinFailed");
    PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2, PlayerTtl = 50000 }, null);
  }
}