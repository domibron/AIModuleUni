using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFlagCarrier : StateBase
{
    private GameObject _targetEnemy;
    private bool _wasFriendlyFlag = false;

    public AttackFlagCarrier(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();
        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // Friendly flag
        if (friendlyFlag != null && friendlyFlag?.transform.parent != null)
        {
            if (friendlyFlag.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _targetEnemy = friendlyFlag.transform.parent.gameObject;
                _wasFriendlyFlag = true;
                return;
            }
        }
        // Enemy flag
        else if (enemyFlag != null && enemyFlag?.transform.parent != null)
        {
            if (enemyFlag.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _targetEnemy = enemyFlag.transform.parent.gameObject;
                _wasFriendlyFlag = false;
                return;
            }
        }
    }

    public override void Execute(AI entity)
    {
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        if (_targetEnemy == null)
        {
            if (_wasFriendlyFlag)
            {
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
                return;
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
                return;
            }
        }
        else
        {
            if (entity.AgentInventory.HasItem(Names.PowerUp))
            {
                entity.AgentActions.UseItem(entity.AgentInventory.GetItem(Names.PowerUp));
            }

            entity.AgentActions.MoveTo(_targetEnemy);
            entity.AgentActions.AttackEnemy(_targetEnemy);
        }
    }

    public override void Exit(AI entity)
    {

    }
}
