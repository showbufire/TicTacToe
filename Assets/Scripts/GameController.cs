using Photon;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : PunBehaviour {

  public Text[] buttonList;
  public Text gameOverText;
  public GameObject gameOverPanel;
  public GameObject restartButton;
  public GameObject gameBoard;
  public GameObject connectionPanel;

  private string playerSide;
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

  public Player playerX;
  public Player playerO;
  public PlayerColor activePlayerColor;
  public PlayerColor inactivePlayerColor;

  private void Awake()
  {
    connectionPanel.SetActive(true);
    gameBoard.SetActive(false);
  }

  public override void OnJoinedRoom()
  {
    Debug.Log("On Joined Room");

    if (PhotonNetwork.room.PlayerCount == 2)
    {
      StartGame();
    }
    else
    {
      ToastManager toastManager = FindObjectOfType<ToastManager>();
      toastManager.Toast("Connected, Waiting for other player to join...");
    }
  }

  public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
  {
    Debug.Log("Other player arrived");

    if (PhotonNetwork.room.PlayerCount == 2)
    {
      StartGame();
    }
  }

  private void StartGame()
  {
    Debug.Log("Game Started");
    connectionPanel.SetActive(false);
    gameBoard.SetActive(true);
    SetUp();
  }

  public void SetUp()
  {
    foreach (Text button in buttonList)
    {
      button.GetComponentInParent<GridSpace>().SetGameController(this);
    }
    InitGameState();
  }

  private void InitGameState()
  {
    playerSide = "X";
    moveCount = 0;
    gameOverPanel.SetActive(false);
    restartButton.SetActive(false);
    SetPlayerColors(playerX, playerO);
  }

  private void SetPlayerColors(Player activePlayer, Player inactivePlayer)
  {
    activePlayer.panel.color = activePlayerColor.panelColor;
    activePlayer.text.color = activePlayerColor.textColor;
    inactivePlayer.panel.color = inactivePlayerColor.panelColor;
    inactivePlayer.text.color = inactivePlayerColor.textColor;
  }

  public string GetPlayerSide()
  {
    return playerSide;
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

  public void EndTurn()
  {
    moveCount += 1;
    if (IsGameEnd())
    {
      GameOver();
    }
    else
    {
      ChangeSide();
    }
  }

  private bool IsGameEnd()
  {
    return Win() || moveCount >= 9;
  }

  private bool Win()
  {
    return Win(0, 1, 2) || Win(3, 4, 5) || Win(6, 7, 8) || Win(0, 3, 6) || Win(1, 4, 7) || Win(2, 5, 8) || Win(0, 4, 8) || Win(2, 4, 6);
  }

  private bool Win(int x, int y, int z)
  {
    return buttonList[x].text == playerSide && buttonList[y].text == playerSide && buttonList[z].text == playerSide;
  }

  public void GameOver()
  {
    gameOverPanel.SetActive(true);
    restartButton.SetActive(true);
    if (Win())
    {
      gameOverText.text = playerSide + " Wins!";
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

  private void ChangeSide()
  {
    if (playerSide == "X")
    {
      playerSide = "O";
      SetPlayerColors(playerO, playerX);
    }
    else
    {
      playerSide = "X";
      SetPlayerColors(playerX, playerO);
    }
  }
}
