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
                bool areWeCloser = true;
                foreach (GameObject enemy in enemies)
                {
                    if (!OurWeCloser(_agent.transform.position, enemy.transform.position, collectable.transform.position)) areWeCloser = false;
                }

                if (!areWeCloser) continue;

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

        if (_agent.transform.position == _agent.GetComponent<NavMeshAgent>().destination)
        {
            _agent.AgentActions.MoveToRandomLocation();
        }
        // check around
        // if not go to a new location carefully.

        // once we have a heal item, use it
    }

    private bool OurWeCloser(Vector3 ourPos, Vector3 theirPos, Vector3 targetPos)
    {
        if (Vector3.Distance(ourPos, targetPos) > Vector3.Distance(theirPos, targetPos))
        {
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return "Healing self";
    }

}
