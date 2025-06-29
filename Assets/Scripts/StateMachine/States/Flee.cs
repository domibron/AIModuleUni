using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : StateBase
{
    public Flee(FiniteStateMachine fsm) : base(fsm)
    {
    }


    public override void Enter(AI entity)
    {
        entity.AgentActions.MoveToRandomLocation();
    }

    public override void Execute(AI entity)
    {
        // presuming this is being called in a update method. aware that there is a possibility its not, but I know that it is.

        if (entity.HasReachedDestination())
        {
            entity.AgentActions.MoveToRandomLocation(); // TODO, use dot to make sure its away from enemy and not make the AI go into the enemy
        }

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
        // I believe there is something wrong with the sensing, it can spot enemies and items through walls some how.
        // TODO investigate if i come back here.
        GameObject enemy = entity.AgentSenses.GetNearestEnemyInView();

        if (enemy != null)
        {
            //entity.AgentActions.Flee(enemy);
            return true;
        }

        return false;
    }
}
