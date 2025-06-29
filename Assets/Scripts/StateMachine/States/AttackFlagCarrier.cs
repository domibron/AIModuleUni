using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFlagCarrier : StateBase
{
    /// <summary>
    /// The target enemy with the flag.
    /// </summary>
    private GameObject _targetEnemy;

    /// <summary>
    /// If the flag was one of ours.
    /// </summary>
    private bool _wasFriendlyFlag = false;

    public AttackFlagCarrier(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        // get the flags we can see.
        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();
        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // Friendly flag - then target the carrier.
        if (friendlyFlag != null && friendlyFlag?.transform.parent != null)
        {
            if (friendlyFlag.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _targetEnemy = friendlyFlag.transform.parent.gameObject;
                _wasFriendlyFlag = true;
                return;
            }
        }
        // Enemy flag - then target the carrier.
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
        // If we are hurt then heal.
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        // if we killed our enemy, return or steal the flag depending on the flag.
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
            // use power up if we have one of those.
            if (entity.AgentInventory.HasItem(Names.PowerUp))
            {
                entity.AgentActions.UseItem(entity.AgentInventory.GetItem(Names.PowerUp));
            }

            // move to the enemy and attack them.
            entity.AgentActions.MoveTo(_targetEnemy);
            entity.AgentActions.AttackEnemy(_targetEnemy);
        }
    }

    public override void Exit(AI entity)
    {

    }
}
