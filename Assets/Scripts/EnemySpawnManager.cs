using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : Base_Pool<EnemySpawnManager>
{
    [SerializeField] List<PoolClass<Enemy>> _poolList = new List<PoolClass<Enemy>>();

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        InitPool();
        StartCoroutine(SpawnCor());
    }

    protected override void InitPool()
    {
        base.InitPool();

        for (int i = 0; i < _prefabList.Count; i++)
        {
            string _id = _prefabList[i].gameObject.name;

            PoolClass<Enemy> _pc = new PoolClass<Enemy>();
            _pc._id = _id;

            for (int j = 0; j < _poolMaxCount; j++)
            {
                var _item = Instantiate(_prefabList[i], _poolSpawnTrans).GetComponent<Enemy>();
                _item.gameObject.SetActive(false);
                _pc._poolList.Add(_item);
            }

            _poolList.Add(_pc);
        }
    }

    private Enemy Pop(string _id)
    {
        for (int i = 0; i < _poolList.Count; i++)
        {
            if (_poolList[i]._id == _id)
            {
                var _item = _poolList[i]._poolList[0];

                _poolList[i]._poolList.RemoveAt(0);
                _poolList[i]._poolList.Add(_item);

                return _item;
            }
        }

        return null;
    }

    private Vector3 GetSpawnPos()
    {
        Vector3 _rPos = Vector3.zero;

        _rPos.x = 20f;
        _rPos.y = Random.Range(-4.0f, 3.0f);

        return _rPos;
    }

    private WaitForSeconds _spawnDelayTime;
    IEnumerator SpawnCor()
    {
        //추후, 스폰간격 결정
        _spawnDelayTime = new WaitForSeconds(1.0f);

        while (true)
        {
            var _popItem = Pop("0");
            if (_popItem != null)
            {
                _popItem.transform.position = GetSpawnPos();
                _popItem.gameObject.SetActive(true);
                _popItem.Initalize(5);
            }

            yield return _spawnDelayTime;
        }
    }
}
