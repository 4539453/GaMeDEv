using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBehaviour: MonoBehaviour {

  void Start()
  {
    StartCoroutine(create());
  }


  IEnumerator create()
  {
    while (true)
    {
      Vector3 pos = new Vector3(
        Random.Range(-_width, _width),
        Random.Range(-_height, _height),
        _distance);

      GameObject obj = 
        (GameObject)Instantiate(Resources.Load("Rock"), pos, Quaternion.identity);

      RockBehaviour rock = obj.GetComponent<RockBehaviour>();
      rock._speed = Random.Range(_speedMin, _speedMax);
      rock._rotationSpeed = Random.Range(_rotationSpeedMin, _rotationSpeedMax);

     yield return new WaitForSeconds(0.15f);
    }
  }

  public System.Single _respawnTime = 1.0F;
  public System.Single _distance = 100.0F;
  public System.Single _speedMin = 10.0F;
  public System.Single _speedMax = 30.0F;
  public System.Single _rotationSpeedMin = 1.0F;
  public System.Single _rotationSpeedMax = 10.0F;
  public System.Single _width = 10.0F;
  public System.Single _height = 10.0F;

}
