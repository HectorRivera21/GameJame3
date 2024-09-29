using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    public LayerMask targetPlayer;
    public LayerMask obstacleMask;

    public GameObject player;
    void Update()
    {
        Vector3 playerTarget = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, playerTarget)< viewAngle/2)
        {
            float distanceTarget = Vector3.Distance(transform.position, player.transform.position);
            if (distanceTarget <= viewRadius)
            {
                if (Physics.Raycast(transform.position,playerTarget,distanceTarget,obstacleMask) == false)
                {
                    Debug.Log("i see player");
                }
            }
        }
    }
}
