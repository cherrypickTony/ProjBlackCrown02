using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Pool<T> : Base_Manager<T> where T : Component
{
    protected override void Awake()
    {
        base.Awake();
    }

    [SerializeField] protected Transform _poolSpawnTrans;
    [SerializeField][Range(10, 100)] protected int _poolMaxCount;
    [SerializeField] protected List<GameObject> _prefabList;

    [System.Serializable]
    public class PoolClass<O> where O : Component
    {
        public string _id;
        public List<O> _poolList = new List<O>();
    }

    protected virtual void InitPool()
    {
        //TODO..
    }
}
