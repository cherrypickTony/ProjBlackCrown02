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
    }
}
