using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    public FloatingJoystick joystick;
    private Vector3 inputVec;
    public GameObject target = null;
    Enemy enemy;
    public bool space = false;
    bool isMoved = false;
    public bool isShoot = false;
    bool isReload = false;
    float idleDelay = 3f;
    float idleTime = 0;
    float shootDelay = 1.2f;
    float shootTime = 0;
    int damage;

    [SerializeField] GameObject shootPos;
    [SerializeField] GameObject particle;
    [SerializeField] ParticleSystem shootEffect;
    Coroutine _coroutineShoot = null;

    Rigidbody rigid;
    Animator anim;

    int layerEnemy;
    int layerWall;

    private float speed = 1;
    private int maxBullet = 7;
    private int currentBullet = 7;


    private void Awake() 
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        layerEnemy = LayerMask.GetMask("Enemy");
        layerWall = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        AddTime();
        GetInput();
        Idle();
        Move();
        ShootAuto(target);
        //ReloadAuto();
        //Reload();
    }

    private void LateUpdate() 
    {
        UpdateTarget();
        Look();
    }

    private void AddTime()
    {
        idleTime += Time.deltaTime;
        shootTime += Time.deltaTime;
    }

    private void GetInput()
    {
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.z = Input.GetAxisRaw("Vertical");
        space = Input.GetKeyDown(KeyCode.Space);
        inputVec.x = joystick.Horizontal;
        inputVec.z = joystick.Vertical;
    }

    private void Idle()
    {
        if (idleTime > idleDelay)
        {
            int rand = Random.Range(0, 3);
            anim.SetInteger("Idle", rand);
            idleTime = 0;
        }
    }

    private void Move()
    {
        if (speed <= 10.0f)
            speed += Time.deltaTime * 2;
        if (inputVec == Vector3.zero)
        {
            speed = 1f;
            isMoved = false;
        }
        else
            isMoved = true;

        var nextVec = inputVec.normalized * speed * Time.deltaTime;

        transform.LookAt(rigid.position + nextVec);
        rigid.MovePosition(rigid.position + nextVec);

        anim.SetBool("isWalk", isMoved);
        anim.SetFloat("Speed", speed);
    }

    private void Shoot(GameObject obj)
    {
/*         if (!space)
            return;
        if (currentBullet == 0)
            return;
        if (isReload)
            return;

        anim.SetTrigger("shoot");
        space = false;
        currentBullet--;
        shootTime = 0;
        Debug.Log(currentBullet); */

        if (_coroutineShoot != null)
            StopCoroutine(_coroutineShoot);
        _coroutineShoot = StartCoroutine(CoroutineShoot(obj));
    }

    IEnumerator CoroutineShoot(GameObject obj)
    {
        float time = 0f;
        particle.transform.position = shootPos.transform.position;
        particle.SetActive(true);

        while (time < 0.1f)
        {
            time += Time.deltaTime;
            var nextVec = (obj.transform.position - particle.transform.position);
            nextVec.y = 0.1f;
            nextVec = nextVec.normalized * time;
            particle.transform.position += nextVec;

            yield return null;
        }
        particle.SetActive(false);
        _coroutineShoot = null;
    }
    
    private void Reload()
    {
        if (!space)
            return;
        if (currentBullet != 0)
            return;
        
        anim.SetTrigger("reload");
        space = false;
        isReload = true;

        Invoke(nameof(ReloadOut), 2f);
    }

    private void ReloadOut()
    {
        currentBullet = maxBullet;
        isReload = false;
    }

    private void Look()
    {
        if (target == null)
            return;

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0f;

        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        anim.GetBoneTransform(HumanBodyBones.UpperChest).transform.rotation = rot;
    }

    private void UpdateTarget()
    {
        Collider[] cols = Physics.OverlapSphere(this.transform.position, 5f, layerEnemy);
        float temp;
        float min = 9999;

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (i == 0)
                {
                    min = Vector3.Distance(cols[i].transform.position, transform.position);
                    target = cols[i].gameObject;
                    if (Physics.Linecast(player.transform.position, target.transform.position, layerWall))
                        target = null;
                }
                else
                {
                    temp = Vector3.Distance(cols[i].transform.position, transform.position);
                    if (temp < min)
                    {
                        min = temp;
                        target = cols[i].gameObject;
                        if (Physics.Linecast(player.transform.position, target.transform.position, layerWall))
                        target = null;
                    }
                }
            }
        }
        else
        {
            target = null;
            min = 9999;
        }
    }

    private void ShootAuto(GameObject obj)
    {
        if (target == null)
            return;
        if (shootTime <= shootDelay)
            return;
        if (isReload)
            return;
        if (currentBullet == 0)
        {
            ReloadAuto();
            return;
        }
        anim.SetTrigger("shoot");
        GameManager.instance.soundManager.ShootSound();
        currentBullet--;
        shootTime = 0;
        damage = Random.Range(10, 21);
        enemy = obj.GetComponent<Enemy>();
        
        var forward = transform.forward;
        var dir = new Vector2(forward.x, forward.z);
        GameManager.instance.mainCamera.Shake(dir, GameManager.instance.mainCamera.curve, 0.1f, 1f);

        Shoot(obj);
        enemy.Hit(damage);
        Debug.Log(currentBullet);
    }
    
    private void ReloadAuto()
    {
        if (currentBullet != 0)
            return;
        anim.SetTrigger("reload");
        GameManager.instance.soundManager.ReloadSound();
        isReload = true;
        Invoke(nameof(ReloadOut), 2.5f);
    }
}
