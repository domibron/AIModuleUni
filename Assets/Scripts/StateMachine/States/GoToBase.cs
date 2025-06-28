using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToBase : StateBase
{
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
        if (enemyFlag != null && enemyFlag.transform.parent == null && Vector3.Distance(enemyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBaseToDrop)
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
            _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }
}
