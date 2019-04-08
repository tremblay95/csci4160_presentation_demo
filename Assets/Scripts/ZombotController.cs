using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombotController : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    Animator anim;
    QuestObject questObj;
    public float navTargetRefreshDelay = 0.1f;

    bool playerSpotted = false;
    float spotDelay = 0;
    public float spotRange = 20.0f;

    public float attackDistance = 1.5f;
    public int attackDamage = 10;
    public float attackDelay = 3.5f;

    float attackCounter = 0;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        questObj = GetComponent<QuestObject>();

        player = PlayerInput.pos;

        attackCounter = attackDelay;

        nav.avoidancePriority = 10;

        StartCoroutine(NavTargetUpdate());
    }

    void Update()
    {
        if (spotDelay > 0) spotDelay -= Time.deltaTime;
        if (attackCounter > 0) attackCounter -= Time.deltaTime;

        anim.SetFloat("Speed", nav.velocity.magnitude / nav.speed);
        if (attackCounter <= 0 && Vector3.Distance(player.position, transform.position) <= attackDistance)
        {
            anim.SetTrigger("Attack");
            nav.isStopped = true;
        }

        
    }


    IEnumerator NavTargetUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        while(true)
        {
           
            if (Vector3.Distance(player.position, transform.position) < spotRange)
            {
                if (!playerSpotted)
                {
                    yield return new WaitForSeconds(Random.Range(0.0f, 0.5f));
                    anim.SetTrigger("PlayerSpotted");
                    playerSpotted = true;
                    spotDelay = 2.8f;
                }
                if(spotDelay <= 0)
                {
                    nav.SetDestination(player.position);
                }
            }

            yield return new WaitForSeconds(navTargetRefreshDelay);
        }
    }

    public void Attack()
    {
        Debug.Log(name + " attacking");
    }
    public void ResetNav()
    {
        nav.isStopped = false;
        anim.ResetTrigger("Attack");
        attackCounter = attackDelay;
    }

    void Die()
    {
        StopAllCoroutines();
        anim.SetTrigger("Dead");
        
        nav.enabled = false;
        enabled = false;
        questObj.AdvanceQuest();
    }
}
