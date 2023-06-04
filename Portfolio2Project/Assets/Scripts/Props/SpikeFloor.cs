using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeFloor : MonoBehaviour
{
    enum spikeModes
    {
        constant,
        triggered,
        timed
    }
    [Header("-------Config------")]
    [SerializeField] Transform spikePrefab;
    [SerializeField] int damage;
    [Range(0.2f,30)][SerializeField] float spikeSpacing = 1;
    [SerializeField] float spikeEdgeSpacing;
    [SerializeField] float spikeSpeed;
    [SerializeField] float spikeRetractionDelay;
    [SerializeField] spikeModes mode;
    [SerializeField] bool gizmoWireframe;

    bool isActive;
    List<List<Transform>> spikes = new List<List<Transform>>();

    private void Start()
    {
        switch (mode)
        {
            case spikeModes.constant:
                extend();
                break;

            case spikeModes.timed:
                StartCoroutine(timedExtend());
                break;
        }
    }

    void extend()
    {
        isActive = true;
        Debug.Log("Extending");
        int row = 0;
        int column = 0;

        for (float i = spikeEdgeSpacing; i < transform.localScale.x - spikeEdgeSpacing;i += spikeSpacing)
        {
            spikes.Add(new List<Transform>());
            for (float j = spikeEdgeSpacing; j < transform.localScale.z - spikeEdgeSpacing; j += spikeSpacing)
            {
                Vector3 spikePos = new Vector3((transform.position.x - transform.localScale.x / 2) + i,transform.position.y + (transform.localScale.y/2), (transform.position.z - transform.localScale.z / 2) + j);
                spikes[row].Add(Instantiate(spikePrefab, spikePos, transform.rotation));
                column++;
            }
            row++;
        }
    }

    void retract()
    {
        for (int i = 0; i < spikes.Count;i++)
        {
            for (int j = 0; j < spikes[i].Count; j++)
            {

                Destroy(spikes[i][j].gameObject);
            }
            spikes[i].Clear();
        }
        spikes.Clear();
        isActive= false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(mode == spikeModes.triggered)
        {
            StartCoroutine(timedExtend());
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive)
        {
            IDamage damageable = other.GetComponentInParent<IDamage>();

            if (damageable != null)
            {

                damageable.TakeDamage(damage);
            }
        }
    }

    IEnumerator timedExtend()
    {

        yield return new WaitForSeconds(spikeSpeed);

        extend();
        if (mode != spikeModes.constant) 
        {
            StartCoroutine(timedRetract());
        }
    }

    IEnumerator timedRetract()
    {
        yield return new WaitForSeconds(spikeRetractionDelay);
        retract();
        if(mode == spikeModes.timed) 
        {
            StartCoroutine(timedExtend());
        }
    }


    private void OnDrawGizmos()
    {
        int row = 0;
        int column = 0;
        for (float i = spikeEdgeSpacing; i < transform.localScale.x - spikeEdgeSpacing; i += spikeSpacing)
        {
            spikes.Add(new List<Transform>());
            for (float j = spikeEdgeSpacing; j < transform.localScale.z - spikeEdgeSpacing; j += spikeSpacing)
            {
                Vector3 spikePos = new Vector3((transform.position.x - transform.localScale.x / 2) + i, transform.position.y + (transform.localScale.y / 2), (transform.position.z - transform.localScale.z / 2) + j);
                if (gizmoWireframe)
                {
                    Gizmos.DrawWireMesh(spikePrefab.GetComponent<MeshFilter>().sharedMesh,spikePos);
                }
                else
                {
                    Gizmos.DrawMesh(spikePrefab.GetComponent<MeshFilter>().sharedMesh, spikePos);
                }

                column++;
            }
            row++;
        }
    }
}
