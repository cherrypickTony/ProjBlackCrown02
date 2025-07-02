using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpawnManager;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager instance;

    [SerializeField] Transform _poolSpawnTrans;
    [SerializeField][Range(10, 100)] int _poolMaxCount;
    [SerializeField] DamageText _damageTextPrefab;

    [SerializeField] List<DamageText> _poolClassList = new List<DamageText>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitPool();
    }

    private void InitPool()
    {
        for (int i = 0; i < _poolMaxCount; i++)
        {
            var _item = Instantiate(_damageTextPrefab, _poolSpawnTrans);
            _item.gameObject.SetActive(false);
            _poolClassList.Add(_item);
        }
    }

    private DamageText Pop()
    {
        var _item = _poolClassList[0];
        _poolClassList.RemoveAt(0);
        _poolClassList.Add(_item);

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
