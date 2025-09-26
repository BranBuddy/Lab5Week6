using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

public class RunAwayAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public float radius = 5f;
    public float sampleAreaSize = 20f;

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (CheckVisibility())
        {
                MoveAwayFromPlayer();
        } 
        else
        {
            return;
        }
    }

    void MoveAwayFromPlayer()
    {
        PoissonDiscSampler sampler = new PoissonDiscSampler(sampleAreaSize, sampleAreaSize, radius);

        List<Vector3> candidates = sampler.Samples().Select(v2 => new Vector3(v2.x + transform.position.x, transform.position.y, v2.y + transform.position.z)).ToList();

        if (candidates.Count == 0) return;

        Vector3 bestSpot = candidates.OrderByDescending(c => Vector3.Distance(c, player.position)).First();

        NavMeshHit hit;
        if (NavMesh.SamplePosition(bestSpot, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    private bool CheckVisibility()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, -transform.forward, out hit, 20 ) && hit.collider.gameObject.tag == "Player")
        {
            Debug.DrawRay(transform.position, -transform.forward, Color.red);
                return true;

        }
        else
        {
            Debug.DrawRay(transform.position, -transform.forward, Color.green);
            return false;
        }
    }
}
