using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement: MonoBehaviour {
  private Rigidbody _rb = null;
  public System.Single _distance = 1.0F;
  public System.Single _speed = 100.0F;

  void Awake() {
    _rb = GetComponent<Rigidbody>();
  }

	void Update() { 
    if(_rb != null) {
      Vector3 pos = _rb.position;
      pos.z += _speed * Time.deltaTime;
      if(pos.z > _distance) {
        GameObject.Destroy(gameObject);
        return;
      }
      _rb.MovePosition(pos);
    }
	}
}
