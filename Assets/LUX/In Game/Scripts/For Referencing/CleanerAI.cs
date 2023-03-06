using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class CleanerAI : MonoBehaviour
{
    #region Variables
    public GameObject jeffrey;
    [SerializeField] enum STATES { CLEAN_IDLE, CLEAN_ATTACKING, CLEAN_VICTORY };
    [SerializeField] STATES currentState;
    [SerializeField] GameObject player, speech;
    [SerializeField] Transform vacEnd;
    [SerializeField] Rigidbody projectile;
    private Animator animator;
    public float projectileForce;
    private NavMeshAgent agent;
    [SerializeField] bool angy, dying, still, canShoot;
    private Quaternion targetRot;
    private Vector3 turnVar;
    public float distBetw, playerHealthCheck;
    public bool playerDead, shouldIdle;
    BoxCollider hitbox;
    #endregion


    #region Functions
    private void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        currentState = STATES.CLEAN_IDLE;
        dying = false; still = false; playerDead = false; shouldIdle = false;
        //playerHealthCheck = player.GetComponent<Stats>().currentHealth; THIS IS FUNCTIONAL IN FINAL WORKING CONDITIONS FOR THIS PARTICULAR PROJECT
        speech.SetActive(false);
        animator = jeffrey.GetComponent<Animator>();
        hitbox = gameObject.GetComponent<BoxCollider>();
        canShoot = false;
    }

    private void Start()
    {
        StartCoroutine(EnemySTM());
    }

    private void Update()
    {

        if (angy == true && dying == false)
        {
            //playerHealthCheck = player.GetComponent<Stats>().currentHealth; Debug.Log(playerHealthCheck); THIS IS FUNCTIONAL IN FINAL WORKING CONDITIONS FOR THIS PARTICULAR PROJECT

            turnVar = new Vector3(player.transform.position.x, 2, player.transform.position.z) + Vector3.up - transform.position;
            targetRot = Quaternion.LookRotation(turnVar);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
            if (playerHealthCheck <= 0)
            {
                playerDead = true;
            }
        }
    }

    public void CanShoot()
    {
        canShoot = true;
        Debug.Log("can now shoot");
    }

    public void OnDam()
    {
        if (playerDead == false)
        {
            if (angy == false)
            {
                animator.SetBool("anger", true);
            }
            angy = true;
            currentState = STATES.CLEAN_ATTACKING;
        }
    }

    IEnumerator DieBitch()
    {
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    public void OnDie()
    {
        animator.SetBool("dying", true);
        dying = true;
        hitbox.enabled = false;
        StartCoroutine("DieBitch");
    }

    IEnumerator ShootAt()
    {
        float jeffTime = 0.8f;
        while (!dying)
        {
            if (playerDead == false && canShoot == true)
            {
                Debug.Log("fukin shoot dikhed");
                Rigidbody projectileInstance;
                projectileInstance = Instantiate(projectile, vacEnd.position, vacEnd.rotation) as Rigidbody;
                projectileInstance.AddForce(vacEnd.forward * projectileForce);

                animator.Play("LUX_Cleanman_Shoot");

                if (still == true)
                {
                    jeffTime = 0.2f;
                }
                else
                {
                    jeffTime = 0.8f;
                }

                yield return new WaitForSeconds(jeffTime);
            }
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
    #endregion

    #region Glorious State Machine
    IEnumerator EnemySTM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator CLEAN_IDLE()
    {
        while (currentState == STATES.CLEAN_IDLE)
        {
            Debug.Log("Idle");
            if (playerDead == true)
            {
                speech.SetActive(true);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.ResetPath();
                }
                else
                {
                    agent.destination = player.transform.position;
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CLEAN_ATTACKING()
    {
        StartCoroutine(ShootAt());
        while (currentState == STATES.CLEAN_ATTACKING)
        {
            if (dying != true || shouldIdle == false)
            {
                distBetw = Vector3.Distance(gameObject.transform.position, player.transform.position);

                if (distBetw <= 5)
                {
                    agent.destination = gameObject.transform.position;
                    still = true;
                    animator.SetBool("still", true);
                }
                else
                {
                    agent.destination = player.transform.position;
                    still = false;
                    animator.SetBool("still", false);
                }

                if (playerDead == true)
                {
                    Debug.Log("Switching to idle");
                    shouldIdle = true;
                    currentState = STATES.CLEAN_IDLE;
                    yield return null;
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CLEAN_VICTORY()
    {
        Debug.Log("Victory bitch");
        while (currentState == STATES.CLEAN_VICTORY)
        {
            Debug.Log("ye ye ");
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
    #endregion
}