using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlueCar : EnemyVehicle
{

    protected override void Start()
    {
        StartComponent();
    }

    protected override void Update()
    {
        UpdateComponent();
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        OnCollisionEnter2DComponent(col);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        OnTriggerEnter2DComponent(col);
    }

}