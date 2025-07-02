using UnityEngine;

public class Base_Manager<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static T instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
