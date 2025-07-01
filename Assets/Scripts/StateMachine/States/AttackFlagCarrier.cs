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
        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            foreach (var enemy in entity.AgentSenses.GetEnemiesInView())
            {
                if (enemy.GetComponent<AgentData>().HasFriendlyFlag || enemy.GetComponent<AgentData>().HasEnemyFlag)
                {
                    _wasFriendlyFlag = enemy.GetComponent<AgentData>().HasEnemyFlag;
                    _targetEnemy = enemy;
                    break;
                }
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
