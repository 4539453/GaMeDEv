using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore: MonoBehaviour {
  private static Text _text = null;
  private void Awake() {
    _text = GetComponent<Text>();
    setScore(0);
  }

  public static void setScore(System.UInt32 score) {
    if(_text != null) {
      _text.text = score.ToString("D8");
    }
  }
}
