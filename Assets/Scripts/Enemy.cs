using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    SkeletonAnimation _anim;
    SortingGroup _sortGroup;
    SpriteRenderer _shadowSortGroup;

    float _movSpeed = 1.0f;

    [SerializeField] int _currHp;
    private int _maxHp;

    private void Awake()
    {
        _anim = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        _sortGroup = transform.GetChild(0).GetComponent<SortingGroup>();
        _shadowSortGroup = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void Initalize(int _maxHp)
    {
        this._maxHp = _maxHp;
        _currHp = this._maxHp;

        int order = -(int)(transform.position.y * 100);
        _sortGroup.sortingOrder = order;
        _shadowSortGroup.sortingOrder = order - 1;

        SetSpineAnim("Walk");

        StartCoroutine(MoveCor());
    }

    IEnumerator MoveCor()
    {
        while(true)
        {
            float distanceX = Mathf.Abs(transform.position.x - Player.TR.position.x);

            // X축 거리가 1보다 클 때만 왼쪽으로 이동
            if (distanceX > 2.5f && transform.position.x > Player.TR.position.x)
            {
                transform.position += Vector3.left * _movSpeed * Time.deltaTime;
            }
            else
            {
                SetSpineAnim("Attack3");
            }

            yield return null;
        }
    }

    private void SetSpineAnim(string _animID)
    {
        if (_anim.AnimationState.GetCurrent(0)?.Animation.Name != _animID)
        {
            _anim.AnimationState.SetAnimation(0, _animID, true);
        }
    }

    public bool TakeDamage(int _dmg)
    {
        _currHp = Mathf.Clamp(_currHp - _dmg, 0, _maxHp);

        if (_currHp <= 0)
        {
            this.gameObject.SetActive(false);
            return true;
        }

        return false;
    }
}
