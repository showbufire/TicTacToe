using UnityEngine;
using UnityEngine.UI;
class ToastManager : MonoBehaviour
{
  private Text toastBox;

  void Awake()
  {
    toastBox = GetComponent<Text>();
  }

  public void Toast(string msg)
  {
    toastBox.text = msg;
  }

  public void Clear()
  {
    toastBox.text = "";
  }
}

