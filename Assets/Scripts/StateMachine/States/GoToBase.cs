using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBase : StateBase
{
    public GoToBase(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        // go to our base.
        entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
    }

    public override void Execute(AI entity)
    {
        // if we are hurt, then heal.
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        // do we have any flag, then we need to be in that state.
        if (entity.AgentData.HasFriendlyFlag)
        {
            _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
            return;
        }
        else if (entity.AgentData.HasEnemyFlag)
        {
            _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
            return;
        }

        // see if we can see our flag.
        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();

        // if we cannot see our flag and we can see our base, then our flag is gone, go get it back.
        if (friendlyFlag == null)
        {
            if (!Physics.Linecast(entity.transform.position, entity.AgentData.FriendlyBase.transform.position, entity.AgentSenses.WallsLayer, QueryTriggerInteraction.Ignore))
            {
                // we can see the base, but no flag?
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
            }
        }
        // does an enemy have our flag, then get it back.
        else if (friendlyFlag?.transform.parent != null)
        {
            if (friendlyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }
        // if the flag is there but not at our base, then move it back.
        else if (Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
        {
            _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
        }

        // see if we can see the enemy flag.
        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // if the flag is there and not in our base, go get it.
        if (enemyFlag != null && enemyFlag.transform.parent == null && Vector3.Distance(enemyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
        {
            _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
            return;
        }
        // if someone has the flag, go attack them.
        else if (enemyFlag != null && enemyFlag.transform.parent != null)
        {
            if (enemyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }

        // are the enemies, then go attack them.
        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            if (Vector3.Distance(entity.AgentSenses.GetNearestEnemyInView().transform.position, entity.transform.position) <= AI.MaxRangeToAttackEnemy)
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
                return;
            }
        }

        // if there are items, go get them.
        if (entity.AgentSenses.GetCollectablesInView().Count > 0)
        {
            _owningFSM.ChangeState(_owningFSM.PickUpItem);
            return;
        }


        // if we reached our base, then go to the enemy base or defend ours if we have both flags.
        if (entity.HasReachedDestination() || Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) < AI.MinDistanceToBase)
        {
            if (entity.AgentSenses.GetFriendlyFlagInView() != null && entity.AgentSenses.GetEnemyFlagInView() != null)
            {
                _owningFSM.ChangeState(_owningFSM.DefendBase);
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
            }
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }
}
