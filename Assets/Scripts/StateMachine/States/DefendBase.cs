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
        entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
    }

    public override void Execute(AI entity)
    {

        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();

        if (friendlyFlag == null)
        {
            if (!Physics.Linecast(entity.transform.position, entity.AgentData.FriendlyBase.transform.position, entity.AgentSenses.WallsLayer, QueryTriggerInteraction.Ignore))
            {
                // we can see the base, but no flag?
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
                return;
            }
        }
        else if (friendlyFlag?.transform.parent != null) // does some one have our flag
        {
            if (friendlyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }
        else if (Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBaseToDrop)
        {
            _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
            return;
        }

        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // if the flag is 
        if (enemyFlag != null && enemyFlag.transform.parent != null)
        {
            if (enemyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }

        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            if (Vector3.Distance(entity.AgentSenses.GetNearestEnemyInView().transform.position, entity.transform.position) <= AI.MaxRangeToAttackEnemy)
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
                return;
            }
        }

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
