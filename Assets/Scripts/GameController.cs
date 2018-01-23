using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

  public Text[] buttonList;
  public Text gameOverText;
  public GameObject gameOverPanel;

  private string playerSide;
  private int moveCount;

  void Awake()
  {
    foreach (Text button in buttonList)
    {
      button.GetComponentInParent<GridSpace>().SetGameController(this);
    }
    playerSide = "X";
    moveCount = 0;
    gameOverPanel.SetActive(false);
  }

  public string GetPlayerSide()
  {
    return playerSide;
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
    foreach (Text button in buttonList)
    {
      button.GetComponentInParent<Button>().interactable = false;
    }
    gameOverPanel.SetActive(true);
    if (Win())
    {
      gameOverText.text = playerSide + " Wins!";
    }
    else
    {
      gameOverText.text = "Draw!";
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
