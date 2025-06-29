using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealEnemyFlag : StateBase
{
    /// <summary>
    /// The enemy flag in view to steal.
    /// </summary>
    private GameObject _enemyFlag;

    public StealEnemyFlag(FiniteStateMachine fsm) : base(fsm)
    {

    }

    public override void Enter(AI entity)
    {
        // see if we can get the flag in view.
        _enemyFlag = entity.AgentSenses.GetEnemyFlagInView();
    }

    public override void Execute(AI entity)
    {
        // if we cannot see the flag and its not in our inventory, then go back to base.
        if (_enemyFlag == null && !entity.AgentData.HasEnemyFlag)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
        // if an enemy doesn't have the flag, and we don't and the flag is not at our base, then go get it.
        else if (_enemyFlag.transform.parent == null && !entity.AgentData.HasEnemyFlag && Vector3.Distance(_enemyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
        {
            // go get the flag
            entity.AgentActions.MoveTo(_enemyFlag);
            entity.AgentActions.CollectItem(_enemyFlag);

        }
        // if an enemy has the flag go attack them. if not, go back to our base.
        else if (_enemyFlag.transform.parent != null && !entity.AgentData.HasEnemyFlag)
        {
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

        // Do we have the flag, then we go return it to base and defend.
        if (entity.AgentData.HasEnemyFlag)
        {
            entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase); // go home.

            if (Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) < AI.MinDistanceToBase)
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
