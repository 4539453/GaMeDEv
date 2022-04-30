using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBehaviour: MonoBehaviour {
  private Rigidbody _rb = null;
  private System.Single _speed = 0.0F;
  public System.Single _angularReaction = 10.0F;
  public System.Single _slowing = 1.5F;
  public System.Single _acceleration = 1.0F;
  public System.Single _maxSpeed = 10.0F;
  public GameObject _sadText;
	void Awake() {
		_rb = GetComponent<Rigidbody>();
    _sadText.SetActive(false);
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      _sadText.SetActive(true);
    }
  }

  void turnAround() {
    System.Single inpX = -1.0F * Input.GetAxis("Horizontal");
    if(inpX != 0.0F) {
      Vector3 euler = _rb.rotation.eulerAngles;
      euler.z += -1.0F * Mathf.Sign(inpX) * _angularReaction * Time.deltaTime;
      _rb.MoveRotation(Quaternion.Euler(euler));
    }
  }

  void moveForward() {
    System.Single inpZ = -1.0F * Input.GetAxis("Vertical");

    if(inpZ != 0.0F) {
      _speed += 1.0F * Mathf.Sign(inpZ) * _acceleration * Time.deltaTime;
      _speed = Mathf.Clamp(_speed, -_maxSpeed, _maxSpeed);
    } else {
      _speed += 1.0F * Mathf.Sign(_speed) * _slowing * Time.deltaTime;
      _speed = Mathf.Clamp(_speed, 0.0F, _maxSpeed);
    }

    if(_speed != 0.0F && _speed < 0.0F) {
      _rb.MovePosition(_rb.position + transform.up * _speed * Time.deltaTime);
    }
  }
	
	void FixedUpdate() {
    turnAround();
    moveForward();
	}
    



}
