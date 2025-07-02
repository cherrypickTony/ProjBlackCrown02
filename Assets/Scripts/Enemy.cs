using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform damageTextTrans;
    DamageTextManager damageTextManager;
    SkeletonAnimation _anim;
    SortingGroup _sortGroup;
    SpriteRenderer _shadowSortGroup;

    float _movSpeed = 2.0f;

    [SerializeField] int _currHp;
    private int _maxHp;

    private MaterialPropertyBlock _mpb;
    private Renderer _renderer;

    private void Awake()
    {
        _anim = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        _sortGroup = transform.GetChild(0).GetComponent<SortingGroup>();
        _shadowSortGroup = transform.GetChild(1).GetComponent<SpriteRenderer>();

        _renderer = _anim.GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
    }

    private void Start()
    {
        damageTextManager = DamageTextManager.instance;
    }

    public void Initalize(int _maxHp)
    {
        _mpb.SetFloat("_FillPhase", 0.0f);
        _renderer.SetPropertyBlock(_mpb);

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
        damageTextManager.SetDamageText(_dmg, damageTextTrans);
        _currHp = Mathf.Clamp(_currHp - _dmg, 0, _maxHp);
        FlashWhite();

        if (_currHp <= 0)
        {
            this.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    public void FlashWhite()
    {
        StartCoroutine(FlashCoroutine());
    }

    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;
    IEnumerator FlashCoroutine()
    {
        // 원래 색상을 모를 때 기본으로 흰색 넣음
        // 필요하면 기본 색상 저장 및 복구 로직 추가 가능

        // 점멸 색상 설정
        _mpb.SetFloat("_FillPhase", 1.0f);
        _renderer.SetPropertyBlock(_mpb);

        yield return new WaitForSeconds(flashDuration);

        // 색상 원복 (투명한 검정 또는 기본색으로)
        _mpb.SetFloat("_FillPhase", 0.0f);
        _renderer.SetPropertyBlock(_mpb);
    }
}
