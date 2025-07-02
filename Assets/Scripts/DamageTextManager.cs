using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : Base_Pool<DamageTextManager>
{
    [SerializeField] List<PoolClass<DamageText>> _poolList = new List<PoolClass<DamageText>>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        InitPool();
    }

    protected override void InitPool()
    {
        base.InitPool();

        for (int i = 0; i < _prefabList.Count; i++)
        {
            string _id = _prefabList[i].gameObject.name;

            PoolClass<DamageText> _pc = new PoolClass<DamageText>();
            _pc._id = _id;

            for (int j = 0; j < _poolMaxCount; j++)
            {
                var _item = Instantiate(_prefabList[i], _poolSpawnTrans).GetComponent<DamageText>();
                _item.gameObject.SetActive(false);
                _pc._poolList.Add(_item);
            }

            _poolList.Add(_pc);
        }
    }

    private DamageText Pop()
    {
        var _item = _poolList[0]._poolList[0];
        _poolList[0]._poolList.RemoveAt(0);
        _poolList[0]._poolList.Add(_item);

        return _item;
    }

    public void SetDamageText(int _dmg, Transform _trans)
    {
        var _popItem = Pop();
        if (_popItem != null)
        {
            _popItem.transform.position = _trans.position;
            _popItem.gameObject.SetActive(true);
            _popItem.Initalize(_dmg);
        }
    }
}
