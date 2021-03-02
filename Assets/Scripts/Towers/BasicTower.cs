using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : BaseTower
{
    protected override void Active()
    {
        CheckForNewTarget();
        if (!currentTarget) return;
        Vector3 direction = currentTarget.transform.position - towerCannon.position;
        towerCannon.up = direction;
    }

    protected override void Idle()
    {
        towerCannon.Rotate(0f, 0f, towerStats.rotationSpeed * Time.deltaTime);
    }
}
