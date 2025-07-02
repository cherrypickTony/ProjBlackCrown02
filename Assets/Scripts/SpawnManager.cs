using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] Transform _poolSpawnTrans;
    [SerializeField] [Range(10, 100)] int _poolMaxCount;
    [SerializeField] List<Enemy> _prefabList;

    [System.Serializable]
    public class PoolClass
    {
        public string _id;
        public List<Enemy> _poolList = new List<Enemy>();
    }
    [SerializeField] List<PoolClass> _poolClassList = new List<PoolClass>();

    private void Awake()
    {
        if(instance != null)
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
        StartCoroutine(SpawnCor());
    }

    private void InitPool()
    {
        for (int i = 0; i < _prefabList.Count; i++)
        {
            string _id = _prefabList[i].gameObject.name;

            PoolClass _pc = new PoolClass();
            _pc._id = _id;

            for (int j = 0; j < _poolMaxCount; j++)
            {
                var _enemy = Instantiate(_prefabList[i], _poolSpawnTrans);
                _enemy.gameObject.SetActive(false);
                _pc._poolList.Add(_enemy);
            }

            _poolClassList.Add(_pc);
        }
    }

    private Enemy Pop(string _id)
    {
        for (int i = 0; i < _poolClassList.Count; i++)
        {
            if (_poolClassList[i]._id == _id)
            {
                var _item = _poolClassList[i]._poolList[0];

                _poolClassList[i]._poolList.RemoveAt(0);
                _poolClassList[i]._poolList.Add(_item);

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
        _spawnDelayTime = new WaitForSeconds(2.0f);

        while (true)
        {
            var _popItem = Pop("0");
            if (_popItem != null)
            {
                _popItem.transform.position = GetSpawnPos();
                _popItem.gameObject.SetActive(true);
            }

            yield return _spawnDelayTime;
        }
    }
}
