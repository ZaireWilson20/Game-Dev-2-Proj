using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : EnemyProjectile
{
    protected override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.collide(collision);
    }

    protected override void Update()
    {
        base.Update();
    }
}