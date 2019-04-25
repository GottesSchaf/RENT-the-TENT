using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuestController : MonoBehaviour {

    private NavMeshAgent agent;
    public List<Vector3> Destinations;

    const float STOPPING_DISTANCE = 1f;

    public float Speed
    {
        get
        {
            return Speed;
        }
        set
        {
            Speed = value;
            agent.speed = value;
        }
    }

    protected void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (Destinations == null || Destinations.Count == 0)
        {
            Destinations = new List<Vector3>();
        }
        else
        {
            GoTo(Destinations[0]);
        }
	}


    protected void Update () {

        if (Destinations.Count > 0)
        {
            if (TargetReached())
            {
            
                Destinations.RemoveAt(0);

                if (Destinations.Count > 0)
                {
                    GoTo(Destinations[0]);
                }
            }
            
        }
        else
        {
            Stop();
        }
    }

    private bool TargetReached()
    {
        return Vector3.Distance(transform.position, Destinations[0]) < STOPPING_DISTANCE;
       
    }

    public void Stop()
    {
        agent.isStopped = true;
    }


    public void AddDestination(Vector3 v3)
    {
        Destinations.Add(v3);
        if(Destinations.Count == 1)
        {
            GoTo(v3);
        }
    }

    public void AddDestinationPrioritized(Vector3 v3)
    {
        Destinations.Insert(0, v3);
        if(Destinations.Count == 1)
        {
            GoTo(v3);
        }
    }

    private void GoTo(Vector3 v3)
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        agent.SetDestination(v3);
    }

}
