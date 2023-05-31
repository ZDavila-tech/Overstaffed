using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
