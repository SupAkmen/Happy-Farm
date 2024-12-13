using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetZ = 5f;
    public float smoothing = -2f;

    Transform playerPos;
    void Start()
    {
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPos = new Vector3(playerPos.position.x,transform.position.y,playerPos.position.z - offsetZ);
        
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
