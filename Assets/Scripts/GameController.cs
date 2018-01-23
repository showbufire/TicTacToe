﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

  public Text[] buttonList;
  public Text gameOverText;
  public GameObject gameOverPanel;
  public GameObject restartButton;

  private string playerSide;
  private int moveCount;

  void Awake()
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
    ChangeSide();
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
    }
    else
    {
      playerSide = "X";
    }
  }
}
