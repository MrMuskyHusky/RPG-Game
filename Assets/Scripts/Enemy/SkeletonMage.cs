using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : Enemy
{
    [Header("SkeletonMage Stats")]
    public float curStamina;
    public float maxStamina;

    public override void Attack()
    {
        if (Vector3.Distance(player.position, self.transform.position) > attackRange)
        {
            return;
        }
        Debug.Log("Action 1");

        base.Attack();

        Debug.Log("Action 2");
    }
}
