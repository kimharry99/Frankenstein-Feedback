using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _inst = null;
    public static T Inst { get { return _inst; } private set { _inst = value; } }

    protected virtual void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}


