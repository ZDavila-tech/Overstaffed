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
        siblingmesh = sibling.GetComponent<MeshFilter>().sharedMesh;


        //Debug.Log(siblingmesh);

        if (Wireframe)
        {
            Gizmos.DrawWireMesh(siblingmesh, this.transform.position, sibling.transform.rotation, sibling.transform.localScale);
        }
        else
        {
            Gizmos.DrawMesh(siblingmesh,this.transform.position,sibling.transform.rotation, sibling.transform.localScale);
        }

        Gizmos.DrawLine(this.transform.position, sibling.transform.position);
    }
}
