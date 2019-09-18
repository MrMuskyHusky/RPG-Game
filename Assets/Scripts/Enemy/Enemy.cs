using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Seek,
        Attack,
        Die
    }
    public AIState state;
    public float curHealth, maxHealth, moveSpeed, attackRange, attackSpeed, sightRange;
    public int curWaypoint;

    [Space(5), Header("Base References")]
    public GameObject self;
    public Transform player;
    public Transform waypointParent;
    protected Transform[] waypoints;
    public NavMeshAgent agent;
    public GameObject healthCanvas;
    public Image healthBar;

    public void Start()
    {
        waypoints = waypointParent.GetComponentsInChildren<Transform>();
        agent = self.GetComponent<NavMeshAgent>();
        curWaypoint = 1;
        agent.speed = moveSpeed;
        Patrol();
    }
    private void Update()
    {
        Patrol();
        Seek();
    }

    public void Patrol()
    {
        // DO NOT CONTINUE IF NO WAYPOINTS
        if (waypoints.Length == 0 || Vector3.Distance(player.position, self.transform.position) <= sightRange)
        {
            return;
        }
        // Follow waypoints
        // Set agent to target
        agent.destination = waypoints[curWaypoint].position;
        // Are we at the waypoint?
        if (self.transform.position.x.Equals(agent.destination.x) && self.transform.position.z == agent.destination.z)
        {
            if (curWaypoint < waypoints.Length - 1)
            {
                // If so go to next waypoint
                curWaypoint++;
            }
            else
            {
                // If at the end of patrol go to start
                curWaypoint = 1;
            }
        }
        // If so go to next waypoint
    }
    public void Seek()
    {

        if (Vector3.Distance(player.position, self.transform.position) > sightRange || Vector3.Distance(player.position, self.transform.position) < attackRange)
        {
            // Stop seeking
            return;
        }
        state = AIState.Seek;
        // If player in sight range and not attack range then chase
        agent.destination = player.position;
    }
    public virtual void Attack()
    {
        if(Vector3.Distance(player.position, self.transform.position) > attackRange)
        {
            return;
        }
        state = AIState.Attack;
        Debug.Log("Attacking");
        // If player in attack range then attack
    }
    public void Die()
    {
        state = AIState.Die;
        // If health is <= 0 #Dead
    }
}
