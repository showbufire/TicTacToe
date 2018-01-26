using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : UnityEngine.MonoBehaviour {

  public Text[] buttonList;
  public Text gameOverText;
  public GameObject gameOverPanel;
  public GameObject restartButton;
  public GameObject gameBoard;
  public GameObject connectionPanel;

  private string localSide;
  private string remoteSide;
  private int moveCount;

  [Serializable]
  public class Player
  {
    public Image panel;
    public Text text;
  }

  [Serializable]
  public class PlayerColor
  {
    public Color panelColor;
    public Color textColor;
  }

  public Player localPlayer;
  public Player remotePlayer;
  public PlayerColor activePlayerColor;
  public PlayerColor inactivePlayerColor;

  private void Awake()
  {
    gameBoard.SetActive(false);
    connectionPanel.SetActive(true);
  }

  public void StartGame()
  {
    Debug.Log("Game Started");
    ToastManager toastManager = FindObjectOfType<ToastManager>();
    toastManager.Clear();

    connectionPanel.SetActive(false);
    gameBoard.SetActive(true);
    foreach (Text button in buttonList)
    {
      button.GetComponentInParent<GridSpace>().SetGameController(this);
    }
    InitGameState();
  }

  private void InitGameState()
  {
    localSide = PhotonNetwork.isMasterClient ? "X" : "O";
    remoteSide = PhotonNetwork.isMasterClient ? "O" : "X";
    localPlayer.text.text = localSide;
    remotePlayer.text.text = remoteSide;
    moveCount = 0;
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    SetPlayerColors();
    SetBoardInteractable(localSide == GetCurrentSide());
  }

  public void OnClickButton(Text button)
  {
    int position = -1;
    for (int i = 0; i < buttonList.Length; i++)
    {
      if (buttonList[i] == button)
      {
        position = i;
        break;
      }
    }
    if (position >= 0)
    {
      PhotonView photonView = GetComponent<PhotonView>();
      photonView.RPC("OnClickPosition", PhotonTargets.All, position, localSide);
    }
  }

  [PunRPC]
  void OnClickPosition(int position, string side)
  {
    GridSpace gridSpace = buttonList[position].GetComponentInParent<GridSpace>();
    gridSpace.SetSpaceSide(side);
    EndTurn();
  }

  private string GetCurrentSide()
  {
    if (moveCount % 2 == 0)
    {
      return "X";
    }
    return "O";
  }

  private void SetPlayerColors()
  {
    Player activePlayer = GetCurrentSide() == localSide ? localPlayer : remotePlayer;
    Player inactivePlayer = GetCurrentSide() == localSide ? remotePlayer : localPlayer;
    activePlayer.panel.color = activePlayerColor.panelColor;
    activePlayer.text.color = activePlayerColor.textColor;
    inactivePlayer.panel.color = inactivePlayerColor.panelColor;
    inactivePlayer.text.color = inactivePlayerColor.textColor;
  }

  public void RestartGame()
  {
    InitGameState();
    SetBoardInteractable(true);
    for (int i = 0; i < buttonList.Length; i++)
    {
      buttonList[i].text = "";
    }
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
  }

  private void EndTurn()
  {
    if (Win())
    {
      GameOver();
      return;
    }
    moveCount += 1;
    if (moveCount >= 9)
    {
      GameOver();
      return;
    }
    SetPlayerColors();
    SetBoardInteractable(GetCurrentSide() == localSide);
  }

  private bool Win()
  {
    return Win(0, 1, 2) || Win(3, 4, 5) || Win(6, 7, 8) || Win(0, 3, 6) || Win(1, 4, 7) || Win(2, 5, 8) || Win(0, 4, 8) || Win(2, 4, 6);
  }

  private bool Win(int x, int y, int z)
  {
    string currentSide = GetCurrentSide();
    return buttonList[x].text == currentSide && buttonList[y].text == currentSide && buttonList[z].text == currentSide;
  }

  public void GameOver()
  {
    gameOverPanel.SetActive(true);
    restartButton.SetActive(true);
    if (Win())
    {
      gameOverText.text = GetCurrentSide() + " Wins!";
      SetBoardInteractable(false);
    }
    else
    {
      gameOverText.text = "Draw!";
    }
  }

  private void SetBoardInteractable(bool toggle)
  {
    foreach (Text button in buttonList)
    {
      button.GetComponentInParent<Button>().interactable = toggle;
    }
  }
}
