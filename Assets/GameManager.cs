using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(Input.inputString);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = new Ray(mousePosition,Vector3.forward);

            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);
            if(hitInfo)
            {
                Debug.Log(hitInfo.collider.gameObject.name);
            }
        }

    }
}
