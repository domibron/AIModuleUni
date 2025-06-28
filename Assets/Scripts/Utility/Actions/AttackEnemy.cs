using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : Action
{
    public AttackEnemy(AI agent) : base(agent)
    {

    }

    public override void Execute(float deltaTime)
    {
        List<GameObject> enemies = _agent.AgentSenses.GetEnemiesInView();

        if (enemies.Count <= 0)
        {
            // aggro down
            _agent.AgentActions.MoveToRandomLocation();


            return;
        }

        GameObject targetEnemy = null;
        float distance = 0f;

        foreach (GameObject enemy in enemies)
        {
            if (targetEnemy == null)
            {
                targetEnemy = enemy;
                distance = Vector3.Distance(enemy.transform.position, _agent.transform.position);
                continue;
            }

            if (Vector3.Distance(enemy.transform.position, _agent.transform.position) < distance)
            {
                targetEnemy = enemy;
                distance = Vector3.Distance(enemy.transform.position, _agent.transform.position);
                continue;
            }
        }

        if (targetEnemy == null)
        {
            _complete = true;

            _agent.Boredom += Time.deltaTime;

            _agent.Aggression -= Time.deltaTime;

            return;
        }

        _agent.AgentActions.MoveTo(targetEnemy.transform.position);


        _agent.AgentActions.AttackEnemy(targetEnemy); // does distance check internally.

        _agent.Boredom -= Time.deltaTime;

    }

    public override string ToString()
    {
        return "Attacking enemy";
    }
}
