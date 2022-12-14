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
    public Transform bulletSpawnPoint;
    public float losRadius = 45f;
    public int fieldOfViewAngle;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
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
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        Vector3 bulletSpread = new Vector3(
            Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );
        Vector3 direction = transform.forward + bulletSpread;

        if(!attackDebounce) {
            attackDebounce = true;
            GameObject nBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            nBullet.transform.LookAt(player);
            nBullet.GetComponent<Rigidbody>().AddForce(nBullet.transform.forward * 32f, ForceMode.Impulse);
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
        if(walkPointFlag) agent.SetDestination(patrolPoint);

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
        CheckLos();
        if(!playerInSight && !playerinAttackRange) PatrolArea();
        if(!playerinAttackRange && playerInSight) ChasePlayer();
        if(playerinAttackRange && playerInSight) AttackPlayer();
    }

    void CheckLos()
    {
        playerInSight = false;
        playerinAttackRange = false;
        if(!Physics.CheckSphere(transform.position, sightRange, playerLM)) {
            return;
        }
        GameObject target = GameObject.FindWithTag("Player");
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(directionToTarget, transform.forward);
        // Debug.Log("Angle for FOV is" + angle);
        if (angle < fieldOfViewAngle * 0.5f) {
            // Debug.Log("Checking LOS");
            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, groundLM)) {
                // Debug.Log("Collided with" + hit.collider.name);
                playerInSight = true;
                playerinAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLM);
            }
        }
    }
}
