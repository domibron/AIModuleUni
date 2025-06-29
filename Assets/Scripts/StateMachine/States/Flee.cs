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
        // go to random location because flee vector makes ai go into corner.
        entity.AgentActions.MoveToRandomLocation();
    }

    public override void Execute(AI entity)
    {
        // have we reached the random point, then get a new point.
        if (entity.HasReachedDestination())
        {
            // TODO, use dot to make sure its away from enemy and not make the AI go into the enemy
            entity.AgentActions.MoveToRandomLocation();
        }

        // are there still enemies chasing.
        bool isEnemyInSight = entity.AgentSenses.GetEnemiesInView().Count > 0;

        // if there are no enemies, then we can go back to healing or base.
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
}
