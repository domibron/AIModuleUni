using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToEnemyBase : StateBase
{
    public GoToEnemyBase(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        entity.AgentActions.MoveTo(entity.AgentData.EnemyBase);
    }

    public override void Execute(AI entity)
    {

        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

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

        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();


        if (friendlyFlag != null)
        {
            if (Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.EnemyBase.transform.position) <
                Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position))
            {
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
                return;
            }
        }
        else if (friendlyFlag?.transform.parent != null)
        {
            if (friendlyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }

        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // if the flag is 
        if (enemyFlag != null && enemyFlag.transform.parent == null)
        {
            _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
            return;
        }
        else if (enemyFlag != null && enemyFlag.transform.parent != null)
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



        if (entity.HasReachedDestination() || Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) < AI.MinDistanceToBaseToDrop)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }
}
