using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBase : Action
{

    public DefendBase(AI agent) : base(agent)
    {

    }


    public override void Execute(float deltaTime)
    {
        // we need to seek our base and sit at it,


        if (Vector3.Distance(_agent.AgentData.FriendlyBase.transform.position, _agent.transform.position) < _agent.MaxDistanceAwayFromBase)
        {
            // TODO, where is flag null.
            GameObject flagObject = _agent.GetFlagInView(_agent.AgentData.FriendlyFlagName);


            // if (flagObject != null && Vector3.Distance(flagObject.transform.position, _agent.AgentData.FriendlyBase.transform.position) < 3f)
            // {

            // we are happy, we are at our base.
            _agent.AgentActions.MoveToRandomLocation();
            // }
        }





        // attack enemy if we see one. use aggression.

        List<GameObject> enemies = _agent.AgentSenses.GetEnemiesInView();


        if (_agent.AgentSenses.GetEnemiesInView().Count > 0 && Vector3.Distance(_agent.AgentData.FriendlyBase.transform.position, _agent.transform.position) <= _agent.MaxDistanceAwayFromBase)
        {
            _agent.UtilAI.UpdateGoals(GoalLabels.Aggression, 10);
            _complete = true;
        }
        else if (Vector3.Distance(_agent.AgentData.FriendlyBase.transform.position, _agent.transform.position) <= _agent.MaxDistanceAwayFromBase)
        {
            // _agent.DefendObjective -= deltaTime;
            _complete = true;

        }


        // 
    }

    // /// <summary>
    // /// Try and return a enemy that is closest. may be null!
    // /// </summary>
    // /// <param name="enemies">gameObject of enemy, may be null.</param>
    // /// <returns></returns>
    // private GameObject FindClosestEnemy(GameObject[] enemies)
    // {
    //     GameObject closestEnemy = null;
    //     float distance = 0;

    //     foreach (GameObject enemy in enemies)
    //     {
    //         if (Vector3.Distance(enemy.transform.position, _agent.transform.position) < distance || closestEnemy == null)
    //         {
    //             closestEnemy = enemy;
    //             distance = Vector3.Distance(enemy.transform.position, _agent.transform.position);
    //         }
    //     }

    //     return closestEnemy;
    // }

    public override string ToString()
    {
        return "Defending the base";
    }
}
