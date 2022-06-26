using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [HideInInspector]public float _str;
    [HideInInspector]public float _agi;
    [HideInInspector]public float _con;
    [HideInInspector]public float _armor;    
    [HideInInspector]public float HitPoints;
    [HideInInspector]public float MinDmg;
    [HideInInspector]public float MaxDmg;
    [HideInInspector]public float Level;
    [HideInInspector]public bool IsAlive = true; 
    [HideInInspector]public float AttackSpeed = 1f;
    [HideInInspector]public float AttackRange = 4;
    [HideInInspector]public float MaxHitPoints;
    [HideInInspector]public float XPHolds;
    [HideInInspector]public float MoneyHolds;
    public Item[] PossibleLoot;

    public bool isEpick;

    public ParticleSystem GetHitParticle;
    
    private NavMeshAgent _agent;
    private RaycastHit _hit;
    public GameObject Target;
    private Animator _animator;
    private PlayerMovement Player;
    private void OnEnable()
    {
        _str = Random.Range(10, 20);
        _agi = Random.Range(10, 20);
        _con = Random.Range(10, 20);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        InitEnemy();
        Player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private IEnumerator MoveDown()
    {
        while (true)
        {
            if (_agent.baseOffset <= -0.07)
                yield break;
            _agent.baseOffset -= 0.0001f;
            yield return null;
        }
    }

    private IEnumerator DeleteEnemy()
    {
        yield return new WaitForSeconds(7f);
        Destroy(transform.gameObject);
    }

    private IEnumerator Death()
    {
        _animator.SetTrigger("death");
        if(Random.Range(0,2) == 0)
            Instantiate(PossibleLoot[Random.Range(0, PossibleLoot.Length)], new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z + 3f), Quaternion.identity);
        yield return new WaitForSeconds(5f);
        StartCoroutine(MoveDown());
        StartCoroutine(DeleteEnemy());
    }

    private void Update()
    {
        if (!IsAlive)
            return;
        if (_agent.velocity.magnitude < 0.1f)
        {
            _animator.SetBool("walk", false);
            _agent.updateRotation = false;
        }
        else
        {
            _agent.updateRotation = true;
            _animator.SetBool("walk", true);
        }
    }

    public bool TakeDamage(float damage)
    {
        if (!IsAlive)
            return IsAlive;
        HitPoints -= damage;
        if (damage > 0)
            GetHitParticle.Play();
        if (HitPoints <= 0)
        {
            StopAllCoroutines();
            GetHitParticle.gameObject.SetActive(false);
            HitPoints = 0;
            IsAlive = false;
            Player.Xp += XPHolds;
            Player.Money += MoneyHolds;  
            Die();
        }
        return IsAlive;
    }

    private IEnumerator PrepareAttack()
    {
        while (Target != null)
        {
            if (!IsAlive)
            {
                _agent.isStopped = true;
                yield break;
            }
            var targetPosition = Target.transform.position;
            _agent.destination = targetPosition;
            var dist = Vector3.Distance(targetPosition, transform.position);
            while (dist > AttackRange)
            {
                targetPosition = Target.transform.position;
                _agent.destination = targetPosition;
                dist = Vector3.Distance(targetPosition, transform.position);
                if (!IsAlive)
                {
                    _agent.isStopped = true;
                    yield break;
                }
                yield return null;
            }
            _agent.destination = transform.position;
            transform.LookAt(targetPosition);
            _animator.SetTrigger("meelee");
            if (!IsAlive)
            {
                _agent.isStopped = true;
                yield break;
            }
            if (!Target.GetComponent<PlayerMovement>().TakeDamage(CalculateDamage()))
            {
                Target = null;
                yield break;
            }
            yield return new WaitForSeconds(10f / AttackSpeed); 
        }
    }

    private float CalculateDamage()
    {                
        var baseDamage = Random.Range(MinDmg, MaxDmg);
        if (Target != null)
        {
            var player = Target.GetComponent<PlayerMovement>();
            if (player != null)
            {
                baseDamage = baseDamage * (1 - player.Armor / 200f);
                var chance = 75 + _agi - player._agi;
                if (Random.Range(0f, 100f) >= chance)
                    baseDamage *= 0;
            }
        }
        return baseDamage;
    }

    private void Die()
    {
        StartCoroutine(Death());
    }

    public void SetLevel(float lvl)
    {
        Level = lvl;
        var multiplier = 1 + Level / 10;
        _str *= multiplier;
        _agi *= multiplier;
        _con *= multiplier;
        if (_armor < 195)
            _armor += 2;
        InitEnemy();
    }

    private void InitEnemy()
    {
        if (isEpick)
        {
            MaxHitPoints = _con * 10;
            HitPoints = MaxHitPoints;
            MinDmg = _str ;
            MaxDmg = MinDmg + 8;
            XPHolds = _con * Level;
            MoneyHolds = Level * _armor;
            AttackRange = 8;
        }
        else
        {
            MaxHitPoints = _con * 5;
            HitPoints = MaxHitPoints;
            MinDmg = _str / 2;
            MaxDmg = MinDmg + 4;
            XPHolds = _con * Level;
            MoneyHolds = Level * _armor;
        }
    }

    public void SetTarget(GameObject target)
    {
        if (Target!= null)
            return;
        Target = target;
        StartCoroutine(PrepareAttack());
    }
}


