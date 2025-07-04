using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : StateBase
{
    /// <summary>
    /// Target enemy to kill.
    /// </summary>
    private GameObject _targetEnemy;

    public AttackEnemy(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        // our target to kill.
        _targetEnemy = entity.AgentSenses.GetNearestEnemyInView();

        if (_targetEnemy == null)
        {
            if (Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) <
                    Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position))
            {
                _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
                return;
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.GoToBase);
                return;
            }
        }

        // check if the enemy has either flag and switch to that respected state.
        if (_targetEnemy.GetComponent<AgentData>().HasFriendlyFlag || _targetEnemy.GetComponent<AgentData>().HasEnemyFlag)
        {

            _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);

        }
    }

    public override void Execute(AI entity)
    {
        // if we are hurt, heal.
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        // if the enemy is gone, we did it, we won, we go back ro routine.
        if (_targetEnemy == null)
        {
            if (Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) <
                Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position))
            {
                _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
                return;
            }
            else
            {
                _owningFSM.ChangeState(_owningFSM.GoToBase);
                return;
            }
        }
        else
        {
            if (_targetEnemy.GetComponent<AgentData>().HasFriendlyFlag || _targetEnemy.GetComponent<AgentData>().HasEnemyFlag)
            {

                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;

            }

            // Use a power up if we have one.
            if (entity.AgentInventory.HasItem(Names.PowerUp))
            {
                entity.AgentActions.UseItem(entity.AgentInventory.GetItem(Names.PowerUp));
            }

            // go to enemy and attack.
            entity.AgentActions.MoveTo(_targetEnemy);
            entity.AgentActions.AttackEnemy(_targetEnemy);
        }
    }

    public override void Exit(AI entity)
    {

    }
}
