using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = new Vector3(0, player.transform.position.y, transform.position.z);
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }
    }
}
