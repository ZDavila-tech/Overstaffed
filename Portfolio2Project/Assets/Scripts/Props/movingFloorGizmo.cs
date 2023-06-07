using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingFloorGizmo : MonoBehaviour
{
    Transform sibling;
    Mesh siblingmesh;
    public bool Wireframe;
    private void OnDrawGizmosSelected()
    {

        sibling = this.transform.parent.transform.GetChild(0);


        //Debug.Log(siblingmesh);

        if (Wireframe)
        {
            Gizmos.DrawWireSphere(transform.position, 1);
        }
        else
        {
            Gizmos.DrawSphere(transform.position, 1);
        }

        Gizmos.DrawLine(this.transform.position, sibling.transform.position);
    }
}
