using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    public string Identifier;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
