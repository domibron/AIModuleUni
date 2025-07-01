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
        // if we see any enemies and they have a flag, we attack them.    
        if (_enemyFlag == null && entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            foreach (var enemy in entity.AgentSenses.GetEnemiesInView())
            {
                if (enemy.GetComponent<AgentData>().HasFriendlyFlag || enemy.GetComponent<AgentData>().HasEnemyFlag)
                {
                    _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                    return;
                }
            }
        }

        // if we cannot see the flag and its not in our inventory, then go back to base.
        if (_enemyFlag == null && !entity.AgentData.HasEnemyFlag)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
        // if we don't have the flag and the enemy's flag is not at our base, then go get it.
        else if (!entity.AgentData.HasEnemyFlag && Vector3.Distance(_enemyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
        {
            // go get the flag
            entity.AgentActions.MoveTo(_enemyFlag);
            entity.AgentActions.CollectItem(_enemyFlag);

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
