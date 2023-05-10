using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csDestroyEffect : MonoBehaviour {
   [SerializeField] float delay;
	void Update ()
    {
        StartCoroutine(DestroyObj());
    }

    public IEnumerator DestroyObj()
    {
        if(gameObject != null)
        {
        }
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
    }
}
