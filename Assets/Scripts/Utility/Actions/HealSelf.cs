using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealSelf : Action
{
    public HealSelf(AI agent) : base(agent)
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void Execute(float deltaTime)
    {
        // check if we have a heal item in inventory. and use it.
        if (_agent._agentInventory.HasItem(Names.HealthKit))
        {
            _agent._agentActions.UseItem(_agent._agentInventory.GetItem(Names.HealthKit));
            _complete = true;

            return;
        }

        // if not, find one,
        List<GameObject> collectables = _agent._agentSenses.GetCollectablesInView();

        GameObject closestHeal = null;
        float closestDistance = 0;

        foreach (GameObject collectable in collectables)
        {
            if (collectable.name == Names.HealthKit)
            {
                float collectableDistance = Vector3.Distance(_agent.transform.position, collectable.transform.position);

                if (closestHeal != null && collectableDistance > closestDistance)
                {
                    continue;
                }

                closestHeal = collectable;
                closestDistance = Vector3.Distance(_agent.transform.position, closestHeal.transform.position);

                continue;
            }
        }

        if (closestHeal != null)
        {

            _agent._agentActions.MoveTo(closestHeal);

            _agent._agentActions.CollectItem(closestHeal);

            return;
        }

        if (_agent.transform.position == _agent.GetComponent<NavMeshAgent>().destination)
        {
            _agent._agentActions.MoveToRandomLocation();
        }
        // check around
        // if not go to a new location carefully.

        // once we have a heal item, use it
    }

    public override string ToString()
    {
        return "Healing self";
    }

}
