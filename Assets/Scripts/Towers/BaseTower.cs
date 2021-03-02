using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [HideInInspector] public ScriptableTower towerStats;
    [SerializeField] protected Transform towerCannon;
    [SerializeField] protected CircleCollider2D rangeCollider;

    private Action currentStateMethod;
    protected TowerState state;
    protected Enemy currentTarget;

    // Abstraktit funktiot tarkoittaa sitä, että luokan joka perii BaseTowerin täytyy
    // implementoida nämä funktiot.
    protected abstract void Idle();
    protected abstract void Active();
    
    private void OnValidate()
    {
        rangeCollider = GetComponent<CircleCollider2D>();    
    }

    private void Awake()
    {
        ChangeState(TowerState.IDLE);
    }

    public void SetStats()
    {
        rangeCollider.radius = towerStats.range;
    }

    private void Update()
    {
        currentStateMethod();
    }

    protected void ChangeState(TowerState newState)
    {
        if (newState == TowerState.IDLE)
        {
            currentStateMethod = Idle;
        }
        else if (newState == TowerState.ACTIVE)
        {
            currentStateMethod = Active;
        }
        state = newState;
    }

    protected void CheckForNewTarget()
    {
        if (!currentTarget)
        {
            ChangeState(TowerState.IDLE);
            return;
        }
        float distance = Vector2.Distance(currentTarget.transform.position, towerCannon.position);
        if (distance > towerStats.range * 2f)
        {
            ChangeState(TowerState.IDLE);
            currentTarget = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentTarget) return;
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            currentTarget = enemy;
            ChangeState(TowerState.ACTIVE);
        }
    }
}

    
public enum TowerState
{
    IDLE,
    ACTIVE
}
