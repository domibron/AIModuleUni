using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBase : StateBase
{
    public DefendBase(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        // go to our base.
        entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
    }

    public override void Execute(AI entity)
    {
        // if we are hurt then heal.
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        // check to see if we can see our flag.
        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();

        // if we cannot see our flag at our base, we need to go and get it.
        if (friendlyFlag == null)
        {
            if (!Physics.Linecast(entity.transform.position, entity.AgentData.FriendlyBase.transform.position, entity.AgentSenses.WallsLayer, QueryTriggerInteraction.Ignore))
            {
                // we can see the base, but no flag?
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
                return;
            }
        }
        // If our flag is outside our base, then return it (in case flag was dropped outside).
        else if (Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
        {
            _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
            return;
        }

        // if there are enemies, then we attack.
        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            foreach (var enemy in entity.AgentSenses.GetEnemiesInView())
            {
                if (enemy.GetComponent<AgentData>().HasFriendlyFlag || enemy.GetComponent<AgentData>().HasEnemyFlag)
                {
                    _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                    return;
                }
            }

            if (Vector3.Distance(entity.AgentSenses.GetNearestEnemyInView().transform.position, entity.transform.position) <= AI.MaxRangeToAttackEnemy)
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
                return;
            }
        }

        // if we spot an item, go get it.
        if (entity.AgentSenses.GetCollectablesInView().Count > 0)
        {
            _owningFSM.ChangeState(_owningFSM.PickUpItem);
            return;
        }


    }

    public override void Exit(AI entity)
    {

    }
}
