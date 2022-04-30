using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour: MonoBehaviour {
  private System.Boolean _isSelected;
  private Material _material;

  void Awake() {
    _material = GetComponent<MeshRenderer>().material;
    IsSelected = false;
  }

  public System.Boolean IsSelected {
    get {
      return _isSelected;
    }

    set {
      _material.color = (value == true) ? Color.red : Color.white;
      _isSelected = value;
    }
  }
}
