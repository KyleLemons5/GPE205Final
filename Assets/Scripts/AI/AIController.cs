using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState{Chase, Attack, Patrol, Flee};
    public AIState currentState;
    private float lastStateChangeTime;
    public GameObject target;
    public float chaseDistance;
    public float shootDistance;
    public Transform[] waypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;
    public bool loopWaypoint;
    public float hearingDistance;
    public float fieldOfView;
    public float maxSteeringDistance;
    public float minSteeringDistance;
    public float steeringAmount;
    private float turn;
    public float retargetDelay;
    private float retargetTime;

    public override void Start()
    {
        //ChangeState(AIState.Guard);
        if(GameManager.instance != null)
        {
            GameManager.instance.bots.Add(this);
        }
        if(!IsHasTarget())
            TargetClosestPlayer();
        retargetTime = Time.time + retargetDelay;
        target = this.gameObject;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(pawn != null)
        {
            if(!IsHasTarget()) {
                TargetClosestPlayer();
            }
            // Check every retargetDelay if a player is closer than current target to keep target as the closest player
            if(Time.time >= retargetTime) {
                TargetClosestPlayer();
                retargetTime = Time.time + retargetDelay;
            }

            base.Update();
        }
    }

    // AI decision maker
    public override void ProcessInputs()
    {
        
    }

    //Patrol State
    protected void DoPatrolState()
    {
        // Check to make sure there's another waypoint
        if(waypoints.Length > currentWaypoint)
        {
            Seek(waypoints[currentWaypoint]);
            // If close enough, target next waypoint
            if(Vector3.Distance(pawn.transform.position, waypoints[currentWaypoint].position) < waypointStopDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            if(loopWaypoint)
                RestartPatrol();
        }
    }

    // Rotates toward a ghost player on the opposite side of the tank and goes forward
    protected void DoFleeState(){
        if(target == null)
            return;
        Vector3 away = 2 * pawn.transform.position - target.transform.position;
        pawn.RotateTowards(away);
        pawn.MoveForward();
    }

    // Resets the waypoint variable
    protected void RestartPatrol()
    {
        currentWaypoint = 0;
    }

    // Chase player State
    protected void DoChaseState()
    {
        //Debug.Log("ChaseState");
        Seek(target);
    }

    // Attack player state
    public void DoAttackState()
    {
        Seek(target);
        Shoot();
    }

    // Tries to shoot, but only actually shoots if the pawn is off cooldown
    public void Shoot()
    {
        pawn.Shoot();
    }

    // Overload of Seek
    public void Seek (GameObject target)
    {
        //Seek the player
        if(target == null)
            return;
        Seek(target.transform.position);
    }

    // Overload of Seek
    public void Seek (Transform target)
    {
        //Seek the player
        if(target == null)
            return;
        Seek(target.position);
    }

    // Rotates the tank toward a target and moves it forward while trying to avoid obstacles
    public void Seek (Vector3 target)
    {
        //Seek the player
        if(target == null)
            return;
        pawn.RotateTowards(target);
        AvoidObstacles(target);
        pawn.MoveForward();
    }

    // Checks if a target is less than a distance
    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if(target == null)
            return false;
        if(Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
            return true;
        else
            return false;
    }

    // Changes the AIState
    public virtual void ChangeState(AIState newState)
    {
        currentState = newState;
        lastStateChangeTime = Time.time;
    }

    // Assigns target to player one
    public void TargetClosestPlayer() // Change to target closer player
    {
        if(GameManager.instance != null)
        {
            if(GameManager.instance.players.Count > 0)
            {
                // Make sure the pawns have actually spawned in
                if(GameManager.instance.players[0].pawn != null){

                    GameObject closest = GameManager.instance.players[0].pawn.gameObject;

                    for(int i = 0; i < GameManager.instance.players.Count; i++){
                        // check each pawn in players to see which is the closest
                        if(GameManager.instance.players[i].pawn == null)
                            continue;
                        
                        if(Vector3.Distance(pawn.transform.position, GameManager.instance.players[i].transform.position) < Vector3.Distance(pawn.transform.position, closest.transform.position))
                        {
                            closest = GameManager.instance.players[i].pawn.gameObject;
                        }
                    }
                    target = closest;
                    Debug.Log(this + " is targetting " + target);
                }
            }
        }
    }

    // Attemps to avoid obstacles by turning clockwise
    public virtual void AvoidObstacles(Vector3 target)
    {
        RaycastHit hitData;
        
        if(Physics.Raycast(pawn.transform.position, pawn.transform.forward, out hitData, maxSteeringDistance, 3))
        {
            //Debug.Log(hitData.transform.gameObject);
            if(hitData.transform.gameObject != this.target)
            {
                turn = steeringAmount * (minSteeringDistance/hitData.distance);
                if(turn > steeringAmount)
                    turn = steeringAmount;
                pawn.mover.Rotate(turn);
            }
        }
    }

    // Checks to see if the target exists
    protected bool IsHasTarget()
    {
        return (target != null);
    }

    // Checks to see if the tank can hear the player
    protected bool IsCanHear()
    {
        if(target != null){
            
            Noisemaker noiseMaker = target.GetComponent<Noisemaker>();
            if(noiseMaker == null)
                return false;

            if(noiseMaker.volumeDistance <= 0)
                return false;

            float totalDistance = noiseMaker.volumeDistance + hearingDistance;

            if(Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
                return true;
            else
                return false;
        }
        else 
            return false;
    }

    // Checks to see if the tank can see the target
    public bool IsCanSee(GameObject target)
    {
        if (target != null){
            Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;

            float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);

            RaycastHit hitData;

            if(angleToTarget < fieldOfView)
            {
                if(Physics.Raycast(pawn.transform.position, pawn.transform.forward, out hitData, chaseDistance))
                {
                    //Debug.Log(hitData.transform.gameObject);
                    if(hitData.transform.gameObject == target)
                        return true;
                    else
                    {
                        float distanceToTarget = Vector3.Distance(target.transform.position, pawn.transform.position);
                        float distanceToHit = hitData.distance;
                        if(distanceToTarget < distanceToHit)
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return true;
            }
            else
                return false;
        }
        else
            return false;
    }
}
