using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Pace,
        Follow,
        Attack
    }

    [SerializeField] GameObject[] route;
    public GameObject target;
    int routeIndex = 0;

    [SerializeField] float speed = 3.0f;
    public int enemyHealth = 100;
    public GameObject deathParticlesPrefab;
    public GameObject itemDropPrefab;

    public State currentState = State.Pace;
    private float attackRange = 2.0f;
    private float attackCooldown = 2.0f;
    private float lastAttackTime = 0f;

    void Update()
    {
        switch (currentState)
        {
            case State.Pace:
                OnPace();
                break;
            case State.Follow:
                OnFollow();
                break;
            case State.Attack:
                OnAttack();
                break;
        }

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void OnPace()
    {
        if (route == null || route.Length == 0) return;

        target = route[routeIndex];
        MoveTo(target);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            routeIndex = (routeIndex + 1) % route.Length;
        }

        GameObject obstacle = CheckForward();
        if (obstacle != null && obstacle.CompareTag("Player"))
        {
            target = obstacle;
            currentState = State.Follow;
        }
    }

    void OnFollow()
    {
        if (target == null) return;

        PlayerStateManager playerState = target.GetComponent<PlayerStateManager>();
        if (playerState != null && playerState.isSneaking)
        {
            currentState = State.Pace;
            return;
        }

        MoveTo(target);

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget < attackRange)
        {
            StartCoroutine(StartAttack());
        }

        GameObject obstacle = CheckForward();
        if (obstacle != null && obstacle.CompareTag("Player"))
        {
            target = obstacle;
            currentState = State.Follow;
        }
    }

    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(0.2f);
        currentState = State.Attack;
    }

    void OnAttack()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget >= attackRange + 1.0f)
        {
            currentState = State.Follow;
            return;
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            fpshealth playerHealth = target.GetComponent<fpshealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                lastAttackTime = Time.time;
                Debug.Log("Enemy attacked the player!");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Bullets bullet = other.GetComponent<Bullets>();
        if (bullet != null)
        {
            TakeDamage(5);
            Destroy(other.gameObject);
            Debug.Log("Enemy hit by a bullet!");
            return;
        }

        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            currentState = State.Follow;
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        Debug.Log("Enemy took damage. Remaining health: " + enemyHealth);

        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("EnemyAI.Die() called");

        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }

        if (itemDropPrefab != null)
        {
            Debug.Log("Spawning item drop...");
           Instantiate(itemDropPrefab, transform.position + Vector3.up * 0.3f, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("itemDropPrefab not assigned");
        }

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.EnemyDefeated();
        }

        Destroy(gameObject);
    }

    void MoveTo(GameObject t)
    {
        transform.position = Vector3.MoveTowards(transform.position, t.transform.position, speed * Time.deltaTime);
        Vector3 directionToTarget = t.transform.position - transform.position;

        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }

    GameObject CheckForward()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            PlayerStateManager player = hit.transform.gameObject.GetComponent<PlayerStateManager>();
            if (player != null && !player.isSneaking)
            {
                return hit.transform.gameObject;
            }
        }

        return null;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}
