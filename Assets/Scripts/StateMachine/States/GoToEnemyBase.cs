using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToEnemyBase : StateBase
{
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
            _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
            return;
        }

        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            _owningFSM.ChangeState(_owningFSM.AttackEnemy);
            return;
        }

        if (entity.AgentSenses.GetCollectablesInView().Count > 0)
        {
            _owningFSM.ChangeState(_owningFSM.PickUpItem);
            return;
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
            _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
            return;
        }

        if (entity.HasReachedDestination())
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }
}
