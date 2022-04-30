using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBehaviour: MonoBehaviour {

  void Awake() {
    _rigidbody = GetComponent<Rigidbody>();

    _eulerAngles = new Vector3(
      Random.Range(-1.0F, 1.0F),
      Random.Range(-1.0F, 1.0F),
      Random.Range(-1.0F, 1.0F)
    );
  }

  private Vector3 axle = new Vector3(100f, 100f, 60f);

	void FixedUpdate () {

    // Vector3 pos = _rigidbody.position;
    // pos.z -= _speed * Time.deltaTime;
    // _rigidbody.MovePosition(pos);


    Vector3 rot = _rigidbody.rotation.eulerAngles;
    rot += _eulerAngles * _rotationSpeed * Time.deltaTime;
    _rigidbody.MoveRotation(Quaternion.Euler(rot));

	}
  void Update(){
    transform.RotateAround(axle, Vector3.up, _angularVelocity * Time.deltaTime);
  }

  private Rigidbody _rigidbody;

  public System.Single _speed = 1.0F;
  public System.Single _rotationSpeed = 1.0F;
  public System.Single _angularVelocity = 15F;

  public System.Single _deadEnd = -10.0F;

  private Vector3 _eulerAngles;

}
