using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : AIController
{
    public float fleeHealth;

    public float fleeTime;

    private float currentFleeTime;

    public float patrolLeeshDistance;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        currentState = AIState.Patrol;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(pawn != null)
        {
            ProcessInputs();
            base.Update();
        }
    }

    // AI decision maker
    public override void ProcessInputs()
    {
        switch(currentState){
            case AIState.Patrol:
                DoPatrolState();
                if(IsDistanceLessThan(target, chaseDistance) && IsCanSee(target) && IsCanHear()){
                    ChangeState(AIState.Chase);
                }
                if(pawn.GetComponent<Health>().currentHealth < fleeHealth){
                    currentFleeTime = Time.time + fleeTime;
                    ChangeState(AIState.Flee);
                }
                break;

            case AIState.Chase:
                DoChaseState();
                if(IsDistanceLessThan(target, shootDistance)){
                    ChangeState(AIState.Attack);
                }
                if(!IsDistanceLessThan(target, chaseDistance) || !IsCanSee(target)){
                    ChangeState(AIState.Patrol);
                }
                if(pawn.GetComponent<Health>().currentHealth < fleeHealth){
                    currentFleeTime = Time.time + fleeTime;
                    ChangeState(AIState.Flee);
                }
                if(!IsDistanceLessThan(waypoints[0].gameObject, patrolLeeshDistance)){
                    ChangeState(AIState.Patrol);
                }
                break;
            
            case AIState.Attack:
                DoAttackState();
                if(!IsDistanceLessThan(target, shootDistance)){
                    ChangeState(AIState.Chase);
                }
                if(pawn.GetComponent<Health>().currentHealth < fleeHealth){
                    currentFleeTime = Time.time + fleeTime;
                    ChangeState(AIState.Flee);
                }
                if(!IsDistanceLessThan(waypoints[0].gameObject, patrolLeeshDistance)){
                    ChangeState(AIState.Patrol);
                }
                break;

            case AIState.Flee: // Flees for a time
                DoFleeState();
                if(Time.time >= currentFleeTime){
                    ChangeState(AIState.Patrol);
                }
                break;
        }
    }
}
