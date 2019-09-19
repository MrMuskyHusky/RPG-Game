using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : MonoBehaviour
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
    public bool isDead;

    public Transform target;
    public NavMeshAgent agent;
    public Animator anim;
    public float dist;
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float fireRate, timeTillNextFire;
    public float turnSpeed;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(target.position, transform.position);
        if (dist <= 25)
        {
            anim.SetBool("IsMoving", false);
            anim.SetBool("IsRunning", false);
            agent.destination = transform.position;
            Vector3 targetDir = target.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            Shoot();
        }
        else
        {
            anim.SetBool("IsMoving", true);
            anim.SetBool("IsRunning", true);
            agent.destination = target.position;
            timeTillNextFire = 0;
        }
    }
    private void LateUpdate()
    {
        if (curHealth <= 0)
        {
            Destroy(this.gameObject, 0.5f);
        }
    }
    void Shoot()
    {
        if (PlayerHandler.isDead == false)
        {
            timeTillNextFire += Time.deltaTime;
            if (timeTillNextFire >= fireRate)
            {
                anim.SetTrigger("Attack");
                Invoke("Fire", 0.75f);
                timeTillNextFire = 0;
            }
        }
    }
    void Fire()
    {
        Transform clone = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation).GetComponent<Transform>();
        clone.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 50);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            curHealth -= 50;
        }
    }
}
