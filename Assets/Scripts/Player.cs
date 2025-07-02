using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    public static Transform TR; 

    private void Awake()
    {
        instance = this;
        TR = this.gameObject.transform;

        _anim = transform.GetChild(0).GetComponent<SkeletonAnimation>();
    }

    private void Start()
    {
        StartCoroutine(AttackCor());
    }

    private WaitForSeconds _attackDelayTime;
    IEnumerator AttackCor()
    {
        _attackDelayTime = new WaitForSeconds(1.0f);

        while (true)
        {
            AttackNearestEnemy();

            if (_findTarget == null)
            {
                yield return _attackDelayTime;
            }
            else
            {
                TrackEntry current = _anim.AnimationState.GetCurrent(0);
                while (current.TrackTime < current.AnimationEnd) yield return null;
            }
        }
    }

    [SerializeField] [Range(1, 20)] float attackRange = 2f;
    [SerializeField] Color gizmoColor = Color.red;
    [SerializeField] LayerMask enemyLayer;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void AttackNearestEnemy()
    {
        Vector2 playerPos = transform.position;

        // 플레이어 중심에서 공격 범위 내 적 검색
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(playerPos, attackRange, enemyLayer);

        if (enemiesInRange.Length == 0)
            return;

        Collider2D closestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(playerPos, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Debug.Log("가장 가까운 적 공격: " + closestEnemy.name);

            _findTarget = closestEnemy.GetComponent<Enemy>();
            SetSpineAnim("Attack_Bow");
        }
        else
        {
            SetSpineAnim("Idle_Bow");
        }
    }

    Enemy _findTarget = null;
    void OnAttackAnimationEnded(TrackEntry trackEntry)
    {
        Debug.Log("Attack 애니메이션이 끝났습니다!");

        // 여기에 원하는 동작 실행
        if (_findTarget == null) return;
        if (_findTarget.TakeDamage(1)) _findTarget = null;
    }

    SkeletonAnimation _anim;
    private void SetSpineAnim(string _animID)
    {
        switch (_animID)
        {
            case "Attack_Bow":
                TrackEntry current = _anim.AnimationState.GetCurrent(0);

                if (current.Animation.Name == _animID &&
                    current.TrackTime < current.AnimationEnd) return; //애니메이션 실행중이면 변경불가

                TrackEntry track = _anim.AnimationState.SetAnimation(0, _animID, false);
                track.End += OnAttackAnimationEnded; // 완료 콜백 연결
                break;

            default:
                if (_anim.AnimationState.GetCurrent(0)?.Animation.Name != _animID)
                {
                    _anim.AnimationState.SetAnimation(0, _animID, true);
                }
                break;
        }
    }
}
