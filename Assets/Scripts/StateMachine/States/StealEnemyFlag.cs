using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealEnemyFlag : StateBase
{
    private GameObject _enemyFlag;

    public StealEnemyFlag(FiniteStateMachine fsm) : base(fsm)
    {

    }

    public override void Enter(AI entity)
    {
        _enemyFlag = entity.AgentSenses.GetEnemyFlagInView();
    }

    public override void Execute(AI entity)
    {
        if (_enemyFlag == null && !entity.AgentData.HasEnemyFlag)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
        else if (_enemyFlag.transform.parent == null && !entity.AgentData.HasEnemyFlag && Vector3.Distance(_enemyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBaseToDrop)
        {
            // go get the flag
            entity.AgentActions.MoveTo(_enemyFlag);
            entity.AgentActions.CollectItem(_enemyFlag);

        }
        else if (_enemyFlag.transform.parent != null && !entity.AgentData.HasEnemyFlag)
        {
            // kill the flag carrier
            if (_enemyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.GoToBase);
                return;
            }
        }

        if (entity.AgentData.HasEnemyFlag)
        {
            entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase); // go home.

            if (Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) < AI.MinDistanceToBaseToDrop)
            {
                entity.AgentActions.DropItem(entity.AgentInventory.GetItem(entity.AgentData.EnemyFlagName));
                _owningFSM.ChangeState(_owningFSM.DefendBase);
                return;
            }
        }
    }

    public override void Exit(AI entity)
    {

    }
}
