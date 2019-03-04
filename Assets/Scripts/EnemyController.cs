using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Character
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float viewRadius;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private State state;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject envir;
    [SerializeField]
    private Animator anim;
    public Transform Path { get; set; }
    private Transform[] points;
    public int Number { get; set; }
    private int destPoint = 0;
    enum State
    {
        patrol,
        check,
        attack,
    }
    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        envir = GameObject.Find("Environment");
        state = State.patrol;
        anim.SetBool("IsRun", false);
        IsAlive = true;
        HitPoints = 20;
    }
	
	public void SetPaths()
    {
        points = new Transform[Path.childCount];
        int i = 0;
        foreach (Transform t in Path)
        {
            points[i++] = t;
        }
    }

	void Update ()
    {
        if (IsAlive)
        {
            FindPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (IsAlive)
        {
            switch(state)
            {
                case State.patrol:
                    {
                        if (Path != null)
                        {
                            if (agent.remainingDistance < 0.5f)
                            {
                                if (points.Length == 0)
                                    return;
                                agent.destination = points[destPoint].position;
                                anim.SetBool("IsRun", true);
                                destPoint = (destPoint + 1) % points.Length;
                            }
                        }
                        break;
                    }
                case State.check:
                    {
                        if (Vector3.Distance(Path.position, player.transform.position) < viewRadius)
                        {
                            if (Vector3.Distance(transform.position, player.transform.position) > minDistance)
                            {
                                anim.SetBool("IsRun", true);
                                agent.destination = player.transform.position;
                            }
                            else
                            {
                                anim.SetBool("IsRun", false);
                                agent.destination = transform.position;
                            }
                        }
                        break;
                    }
                case State.attack:
                    {
                        anim.SetBool("IsRun", false);
                        agent.destination = transform.position;
                        break;
                    }
            }
        }
    }

    public void FindPlayer()
    {
        RaycastHit hitInfo;
        if (Vector3.Distance(Path.position, player.transform.position) < viewRadius)
        {
            Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo);
            if ((hitInfo.transform.tag == "Player") && (hitInfo.transform.GetComponent<PlayerController>().IsAlive))
            {
                state = State.attack;
                transform.LookAt(player.transform);
                SpawnPosition.transform.LookAt(player.transform);
                StartCoroutine(Shoot(0.5f));
            }
            else
            {
                state = State.check;
            }
        }
        else
        {
            if (state != State.patrol)
            {
                agent.destination = Path.position;
                state = State.patrol;
            }
        }
    }

    protected override void Die()
    {
        anim.SetBool("IsRun", false);
        agent.SetDestination(transform.position);
        agent.enabled = false;
        player.GetComponent<PlayerController>().EnemyDie();
        envir.GetComponent<Spawn>().FillPoint[Number] = false;
        base.Die();
        Destroy(gameObject);
    }
}
