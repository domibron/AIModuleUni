using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealSelf : Action
{
    public HealSelf(AI agent) : base(agent)
    {

    }


    public override void Execute(float deltaTime)
    {

        // check if we have a heal item in inventory. and use it.
        if (_agent.AgentInventory.HasItem(Names.HealthKit))
        {
            _agent.AgentActions.UseItem(_agent.AgentInventory.GetItem(Names.HealthKit));
            _complete = true;

            return;
        }

        // if not, find one,
        List<GameObject> collectables = _agent.AgentSenses.GetCollectablesInView();

        List<GameObject> teammates = _agent.AgentSenses.GetFriendliesInView();

        List<GameObject> enemies = _agent.AgentSenses.GetEnemiesInView();

        GameObject closestHeal = null;
        float closestDistance = 0;

        foreach (GameObject collectable in collectables)
        {
            if (collectable.name == Names.HealthKit)
            {

                if (!OurWeCloser(enemies.ToArray(), collectable.transform.position)) continue;



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

            _agent.AgentActions.MoveTo(closestHeal);

            _agent.AgentActions.CollectItem(closestHeal);

            return;
        }

        if (Vector3.Distance(_agent.transform.position, _agent.GetComponent<NavMeshAgent>().destination) <= 2.5f || _agent.GetComponent<NavMeshAgent>().hasPath == false)
        {
            _agent.AgentActions.MoveToRandomLocation();
        }
        // check around
        // if not go to a new location carefully.

        // once we have a heal item, use it
    }

    private bool OurWeCloser(GameObject[] positionsToCompare, Vector3 targetPos)
    {
        bool weAreCloser = true;
        foreach (var posToCompare in positionsToCompare)
        {
            if (Vector3.Distance(_agent.transform.position, targetPos) > Vector3.Distance(posToCompare.transform.position, targetPos))
            {
                weAreCloser = false;
            }
        }

        return weAreCloser;
    }

    public override string ToString()
    {
        return "Healing self";
    }

}
