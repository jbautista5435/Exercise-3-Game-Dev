using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask groundLM, playerLM;

    public Vector3 patrolPoint;
    bool walkPointFlag;
    public float walkPointRadius;

    public float attackCooldown;
    public bool attackDebounce;

    public float sightRange, attackRange;
    public bool playerInSight, playerinAttackRange;

    public float shootSpeed;

    public GameObject bulletPrefab;
    public int health;

    public Vector3 bulletSpreadVariance = new Vector3(0.1f,0.1f,0.1f);

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void ChasePlayer()
    {
        // agent.SetDestination(player.position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) Invoke(nameof(DestroySelf), .5f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void AttackPlayer()
    {
        // agent.SetDestination(transform.position);
        transform.LookAt(player);

        Vector3 bulletSpread = new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );
        Vector3 direction = transform.forward + bulletSpread;

        if(!attackDebounce) {
            attackDebounce = true;
            GameObject nBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            nBullet.GetComponent<Rigidbody>().velocity = direction * shootSpeed;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        attackDebounce = false;
    }

    private void PatrolArea()
    {
        if(!walkPointFlag) GenerateWalkPoint();
        // if(walkPointFlag) agent.SetDestination(patrolPoint);

        Vector3 distanceToWP = transform.position - patrolPoint;
        if(distanceToWP.magnitude < 1f) {
            walkPointFlag = false;
        }
    }

    private void GenerateWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRadius, walkPointRadius);
        float randomX = Random.Range(-walkPointRadius, walkPointRadius);
        patrolPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(patrolPoint, -transform.up, 2f, groundLM)) {
            walkPointFlag = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.Find("Player");
        Vector3 directionToTarget = transform.position - target.transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (Mathf.Abs(angle) > 90) {
            playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLM);
            playerinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLM);
        }
        if(!playerInSight && !playerinAttackRange) PatrolArea();
        if(!playerinAttackRange && playerInSight) ChasePlayer();
        if(playerinAttackRange && playerInSight) AttackPlayer();
    }
}
