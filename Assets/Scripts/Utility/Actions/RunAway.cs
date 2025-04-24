using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : Action
{
    public RunAway(AI agent) : base(agent)
    {

    }

    public override void Execute(float deltaTime)
    {
        // we panic,

        // where are teammates, go to them
        List<GameObject> teammates = _agent.AgentSenses.GetFriendliesInView();

        List<GameObject> enemies = _agent.AgentSenses.GetEnemiesInView();

        Vector3 targetPos = Vector3.zero;

        if (enemies.Count > 0)
        {
            Vector3 averageOfEnemiesPositions = Vector3.zero;

            foreach (GameObject enemy in enemies)
            {
                averageOfEnemiesPositions += enemy.transform.position;
            }

            averageOfEnemiesPositions /= enemies.Count;

            // do we have any teammates
            if (teammates.Count > 0)
            {
                GameObject closestTeammate = null;
                float distanceFromTeammate = 0;

                foreach (GameObject teammate in teammates)
                {
                    if (Vector3.Dot(averageOfEnemiesPositions, teammate.transform.position) < 0.2f)
                    {
                        if (closestTeammate == null || Vector3.Distance(closestTeammate.transform.position, teammate.transform.position) < distanceFromTeammate)
                        {
                            closestTeammate = teammate;
                        }
                    }
                }

                targetPos = closestTeammate.transform.position;
                return;
            }
            else
            {
                // AI is going to run into a cornet and die.
                targetPos = _agent.AgentSenses.GetFleeVectorFromTarget(averageOfEnemiesPositions);
            }
        }
        else
        {
            // got to base
            targetPos = _agent.AgentData.FriendlyBase.transform.position;
        }

        if (enemies.Count <= 0 || _agent.transform.position == _agent.AgentData.FriendlyBase.transform.position)
        {
            _agent.Fear = 0;
            _complete = true;
        }
    }

    public override string ToString()
    {
        return "Run Away / panic";
    }
}
