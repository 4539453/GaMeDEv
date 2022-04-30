using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOInputBehaviour: MonoBehaviour {

  void Awake() {
    _rb = GetComponent<Rigidbody>();
    _sadText.SetActive(false);
  }
  
	void FixedUpdate() {
	  System.Single inpX = Input.GetAxis("Horizontal");
    System.Single inpY = Input.GetAxis("Vertical");

    Vector3 pos = _rb.position;

    pos.x += inpX * _speed * Time.deltaTime;
    pos.y += inpY * _speed * Time.deltaTime;

    pos.x = Mathf.Clamp(pos.x, -_amplitude, _amplitude);
    pos.y = Mathf.Clamp(pos.y, -_amplitude*0.6F, _amplitude*0.4F);
    
    _rb.MovePosition(pos);
	}

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      _sadText.SetActive(true);
    }
  }



  public System.Single _speed = 1.0F;
  public System.Single _amplitude = 1.0F;

  private Rigidbody _rb;


  public GameObject _sadText;


}
