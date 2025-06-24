using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Transform attackPoint;
    public EnemyBrain brain;

    [Header("Melee Attack")]
    public float meleeDamage;

    [Header("Projectile Attack")]
    public GameObject Projectile;

    [Header("States")]
    public bool moving = false;
    public bool attacking = false;
    public enum attackStates
    {
        projectile,
        melee
    }
    public attackStates state;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private void Awake()
    {
        player = GameObject.Find("PlayerCharacter").transform;
        agent = GetComponent<NavMeshAgent>();
        brain = GetComponent<EnemyBrain>();
    }
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange && brain.canMove)
        {
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange && brain.canMove)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange && brain.canAttack)
        {
            AttackPlayer();
        }
        else
        {
            attacking = false;
        }
        if (!brain.canMove)
        {
            agent.speed = 0;
        }
    }
    private void Patrolling()
    {
        moving = true;
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        moving = true;
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        attacking = true;
        agent.SetDestination(transform.position);
        moving = false;

        transform.LookAt(player.position);
        if (!alreadyAttacked)
        {
            //Attack Code Here
            if (state == attackStates.projectile)
            {
                GameObject Temp_Projectile = Instantiate(Projectile, attackPoint.position, Quaternion.identity);
                Temp_Projectile.transform.parent = brain.GM.EnemiesParent;
                Rigidbody rb = Temp_Projectile.GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * 80f, ForceMode.Impulse);
                //rb.AddForce(transform.up, ForceMode.Impulse);
                Destroy(Temp_Projectile, 3f);
            }
            if (state == attackStates.melee)
            {
                Collider[] colliders = Physics.OverlapSphere(attackPoint.transform.position, attackRange);

                foreach (Collider c in colliders)
                {
                    if (c.transform.CompareTag("Player"))
                    {
                        Debug.Log("HitPlayer");
                        Health playerHealth = c.GetComponent<Health>();
                        playerHealth.TakeDamage(meleeDamage);
                    }
                }
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, sightRange);
    }
}
