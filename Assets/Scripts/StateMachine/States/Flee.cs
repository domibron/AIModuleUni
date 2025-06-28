using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : StateBase
{

    public override void Enter(AI entity)
    {

    }

    public override void Execute(AI entity)
    {
        bool isEnemyInSight = FleeFromEnemies(entity);

        if (!isEnemyInSight)
        {
            if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
            {
                _owningFSM.ChangeState(_owningFSM.Heal);
                return;
            }


            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }

    private bool FleeFromEnemies(AI entity)
    {
        GameObject enemy = entity.AgentSenses.GetNearestEnemyInView();

        if (enemy != null)
        {
            entity.AgentActions.Flee(enemy);
            return true;
        }

        return false;
    }
}
