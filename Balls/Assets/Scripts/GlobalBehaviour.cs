using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBehaviour: MonoBehaviour {
  public Camera _cam = null;
  public System.Single _horsepower = 1.0F;
  private List<GameObject> _selectedBalls;
  void Awake() {
    _selectedBalls = new List<GameObject>();
  }

  void LateUpdate() {
    bool[] mouseUp = { Input.GetMouseButtonUp(0), Input.GetMouseButtonUp(1) };
    bool[] mouseDown = { Input.GetMouseButtonDown(0), Input.GetMouseButtonDown(1) };
    bool[] mouseHold = { Input.GetMouseButton(0), Input.GetMouseButton(1) };


    if (mouseDown[1])
    {
      Cursor.visible = false;
    }
    else if (mouseUp[1])
    {
      Cursor.visible = true;
    }

    if (mouseDown[0] || mouseDown[1])
    {
      RaycastHit hit;
      Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray.origin, ray.direction * 25.0F, out hit) == true)
      {
        GameObject hitObject = hit.collider.gameObject;
        switch (hitObject.tag)
        {
          case "PlayableBall":
            if(mouseDown[0])
            {
              BallBehaviour behaviour = hitObject.GetComponent<BallBehaviour>();
              behaviour.IsSelected = true;
              _selectedBalls.Add(hitObject);
            }
            else if(mouseDown[1])
            {
              if(_selectedBalls.Count <= 0)
                Object.Destroy(hitObject);
            }
            break;

          case "FirmGround":
            if(mouseDown[0])
            {
              if(_selectedBalls.Count > 0)
              {
                for (System.Int32 i = 0; i < _selectedBalls.Count; ++i)
                {
                  Vector3 force = hit.point - _selectedBalls[i].transform.position;
                  force.Normalize();
                  force *= _horsepower;
                  Rigidbody body = _selectedBalls[i].GetComponent<Rigidbody>();
                  body.AddForce(force);
                }
              }
              else
              {
                Vector3 position = hit.point;
                position.y += 2;
                Instantiate(Resources.Load("Ball"), position, Quaternion.identity);
              }
            }
            else if(mouseDown[1])
            {
              if(_selectedBalls.Count > 0)
              {
                for (System.Int32 i = 0; i < _selectedBalls.Count; ++i)
                {
                  BallBehaviour behaviour = _selectedBalls[i].GetComponent<BallBehaviour>();
                  behaviour.IsSelected = false;
                }
                _selectedBalls.Clear();
              }
            }
            break;

          default:
            break;
        }
      }
    }
  }
}
