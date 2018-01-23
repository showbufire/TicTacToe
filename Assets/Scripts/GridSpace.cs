using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

  public Button button;
  public Text buttonText;
  public string playerSide;

  private GameController gameController;

  public void SetGameController(GameController gameController)
  {
    this.gameController = gameController;
  }

  public void SetSpace()
  {
    buttonText.text = gameController.GetPlayerSide();
    button.interactable = false;
    gameController.EndTurn();
  }
}
