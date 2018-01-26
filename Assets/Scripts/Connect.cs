using Photon;
using UnityEngine;
using UnityEngine.UI;

public class Connect : PunBehaviour
{
  public InputField usernameInput;

  private const string AppVersion = "0.1";

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