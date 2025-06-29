using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : StateBase
{
    private bool _hasHealItem = false;

    public Heal(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        _hasHealItem = entity.AgentInventory.HasItem(Names.HealthKit);

        if (!_hasHealItem)
        {
            entity.AgentActions.MoveToRandomLocation();
        }
    }

    public override void Execute(AI entity)
    {
        if (_hasHealItem)
        {
            entity.AgentActions.UseItem(entity.AgentInventory.GetItem(Names.HealthKit));
        }
        else
        {
            if (entity.AgentSenses.GetCollectablesInView().Count > 0)
            {
                _owningFSM.ChangeState(_owningFSM.PickUpItem);
                return;
            }

            if (entity.AgentSenses.GetEnemiesInView().Count > 0)
            {
                _owningFSM.ChangeState(_owningFSM.Flee);
                return;
            }

            if (entity.HasReachedDestination())
            {
                entity.AgentActions.MoveToRandomLocation();
                return;
            }
        }

        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            if (_hasHealItem)
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.Flee);
            }

            return;
        }

        _owningFSM.ChangeState(_owningFSM.GoToBase);

    }

    public override void Exit(AI entity)
    {

    }
}
