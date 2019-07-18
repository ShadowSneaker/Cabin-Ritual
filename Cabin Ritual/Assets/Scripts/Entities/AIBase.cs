using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMesh))]
[RequireComponent(typeof(Entity))]
public class AIBase : MonoBehaviour
{
    /// Properties

    // A reference to this object's nav mesh agent.
    protected NavMeshAgent Agent;
    
    // A reference to the attached Entity script.
    protected Entity AI;


    /// Overridables


    // Start is called before the first frame update
    private void OnValidate()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (!Agent) Debug.LogError("Error: Could not find NavMeshAgent script.");

        AI = GetComponent<Entity>();
        if (!AI) Debug.LogError("Error: Could not find Entity script.");
    }


    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (!Agent) Debug.LogError("Error: Could not find NavMeshAgent script.");

        AI = GetComponent<Entity>();
        if (!AI) Debug.LogError("Error: Could not find Entity script.");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Agent.speed = AI.GetSpeed;
    }

    
    /// Functions


    public bool MoveTowardsPoint(Vector3 Destination)
    {
        return Agent.SetDestination(Destination);
    }
}
