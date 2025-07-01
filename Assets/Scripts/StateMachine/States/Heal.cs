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
        // see if we have a heal item.
        _hasHealItem = entity.AgentInventory.HasItem(Names.HealthKit);

        // if not, go to a random location.
        if (!_hasHealItem)
        {
            entity.AgentActions.MoveToRandomLocation();
        }
    }

    public override void Execute(AI entity)
    {
        if (_hasHealItem)
        {
            // Use the health item if we have one.
            entity.AgentActions.UseItem(entity.AgentInventory.GetItem(Names.HealthKit));
        }
        else
        {
            // Are the any items, go get them.
            if (entity.AgentSenses.GetCollectablesInView().Count > 0)
            {
                _owningFSM.ChangeState(_owningFSM.PickUpItem);
                return;
            }

            // we know we are low and have no heal items, we must run.
            if (entity.AgentSenses.GetEnemiesInView().Count > 0)
            {
                _owningFSM.ChangeState(_owningFSM.Flee);
                return;
            }

            // search another location, go!
            if (entity.HasReachedDestination())
            {
                entity.AgentActions.MoveToRandomLocation();
                return;
            }
        }

        // if there are enemies and we have health we can attack or run away if not.
        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {

            _owningFSM.ChangeState(_owningFSM.AttackEnemy);
            return;
        }

        // go back to what we were doing
        if (Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) <
                Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position))
        {
            _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
            return;
        }
        else
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }

        // hmmm.
        //_owningFSM.ChangeState(_owningFSM.GoToBase);

    }

    public override void Exit(AI entity)
    {

    }
}
