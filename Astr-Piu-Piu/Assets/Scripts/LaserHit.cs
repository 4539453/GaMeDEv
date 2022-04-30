using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit: MonoBehaviour {
  static private System.UInt16 _score = 0;

  void Awake() {
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "Enemy")
    {
      GameObject.Destroy(collision.gameObject);
      GameObject.Destroy(gameObject);
      UpdateScore.setScore(++_score);
    }
  }
}
