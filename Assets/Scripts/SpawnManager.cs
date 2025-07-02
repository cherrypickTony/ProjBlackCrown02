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
}
