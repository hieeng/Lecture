using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Coroutine _coroutineDead = null;
    [SerializeField] GameObject hpBar_Base;
    [SerializeField] Image hpBar;
    [SerializeField] int hp = 15;
    Vector3 fixedHPBar = new Vector3(45, 0, 0);
    float time = 0;
    int currentHp;
    bool isDie = false;
    bool isHpBar = false;
    Animator anim;
    [SerializeField] GameObject particle;
    [SerializeField] ParticleSystem hitEffect1;
    [SerializeField] ParticleSystem hitEffect2;
    NavMeshAgent agent;
    Rigidbody[] ragdollR;
    Collider[] ragdollC;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        ragdollR = GetComponentsInChildren<Rigidbody>();
        ragdollC = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() 
    {
        currentHp = hp;
        hpBar.fillAmount = 1f;
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        HpBarRotationFixed();
        Move();
    }

    private void Move()
    {
        if (agent.enabled == false)
            return;
        if (agent.remainingDistance > 0.1f)
            return;
        else
            gameObject.transform.position = agent.destination;

        Vector3 nextPoint;

        if (RandomPoint(gameObject.transform.position, 5f, out nextPoint))
            agent.SetDestination(nextPoint);
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    public void Hit(int damage)
    {
        var tempHp = currentHp;
        currentHp -= damage;
        hpBar_Base.SetActive(true);
        isHpBar = true;
        HitEffect();
        StartCoroutine(CoroutineHpBar(damage, tempHp));

        anim.SetTrigger("Hit");
        GameManager.instance.soundManager.ExplosionSound();
        GameManager.instance.soundManager.BloodSound();
        
        if (currentHp <= 0)
        {
            isDie = true;
            this.gameObject.layer = 0;
            agent.enabled = false;
            GameManager.instance.EnemyKillCount++;
            SetRegdoll();
            Knockback();
            Dead();
        }
    }

    private void HitEffect()
    {
        particle.SetActive(true);
        particle.transform.LookAt(GameManager.instance.player.transform);
        hitEffect1.Play();
        hitEffect2.Play();
    }

    IEnumerator CoroutineHpBar(int damage, int tempHp)
    {
        float hitTime = 0;

         while (hitTime <= 0.5f)
        {
            yield return null;

            hitTime += Time.deltaTime;
            var dps = hitTime / 0.5f;
            dps *= damage;
            hpBar.fillAmount = (tempHp - dps) / hp;
        }
        while ( hitTime <= 2f)
        {
            yield return null;

            hitTime += Time.deltaTime;
        }
        hpBar_Base.SetActive(false);
        isHpBar = false;
    }

    private void HpBarRotationFixed()
    {
        if (!isHpBar)
            return;
        //hpBar_Base.transform.eulerAngles = Vector3.zero;
        hpBar_Base.transform.eulerAngles = fixedHPBar;
    }
    
    private void Knockback()
    {
        if (!isDie)
            return;
        var reactVec = transform.position - GameManager.instance.player.transform.position;
        reactVec = reactVec.normalized;
        reactVec += new Vector3 (0, 0.5f, 0);
        ragdollR[1].AddForce(reactVec * 4000);
    }

    private void SetRegdoll()
    {
        for (int i = 1, size = ragdollR.Length; i < size; i++)
        {
            ragdollC[i].isTrigger = false;
            ragdollR[i].isKinematic = false;
        }
        ragdollC[0].isTrigger = true;
        anim.enabled = false;
    }

    private void Dead() 
    {
        {
            if (_coroutineDead != null)
                StopCoroutine(_coroutineDead);
            _coroutineDead = StartCoroutine(CoroutineDead());
        }
    }

    IEnumerator CoroutineDead()
    {
        float deadTime = 0;

        while (deadTime <= 3f)
        {
            yield return null;
            deadTime += Time.deltaTime;
        }
        for (int i = 0, size = ragdollC.Length; i < size; i++)
            ragdollC[i].enabled = false;
        Destroy(gameObject, 1f);
    }
}
