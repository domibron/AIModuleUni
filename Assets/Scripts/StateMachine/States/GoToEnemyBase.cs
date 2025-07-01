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
        // go to the enemy's base.
        entity.AgentActions.MoveTo(entity.AgentData.EnemyBase);
    }

    public override void Execute(AI entity)
    {
        // if we are hurt then go heal.
        if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        {
            _owningFSM.ChangeState(_owningFSM.Heal);
            return;
        }

        // if we have either flag in our inventory, then we need to be in the respected state.
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

        // see if we can see the friendly flag.
        GameObject friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();

        // if we can spot our flag and its not at our base, go return it.
        if (friendlyFlag != null)
        {
            if (Vector3.Distance(friendlyFlag.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBase)
            {
                _owningFSM.ChangeState(_owningFSM.ReturnFriendlyFlag);
                return;
            }
        }

        // see if we can see our flag.
        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // can we see the enemy flag and no one has it, go steal it.
        if (enemyFlag != null)
        {
            _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
            return;
        }
        // if an enemy has the flag, go attack them.
        else if (enemyFlag != null)
        {
            if (enemyFlag.transform.parent.gameObject.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }

        // if there are enemies, go attack them.
        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            // do any of them have a flag. if so we attack the flag carrier.
            foreach (var enemy in entity.AgentSenses.GetEnemiesInView())
            {
                if (enemy.GetComponent<AgentData>().HasFriendlyFlag || enemy.GetComponent<AgentData>().HasEnemyFlag)
                {
                    _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                    return;
                }
            }

            // otherwise we just attack the closest one.
            if (Vector3.Distance(entity.AgentSenses.GetNearestEnemyInView().transform.position, entity.transform.position) <= AI.MaxRangeToAttackEnemy)
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
                return;
            }
        }

        // if there are items, then go get them.
        if (entity.AgentSenses.GetCollectablesInView().Count > 0)
        {
            _owningFSM.ChangeState(_owningFSM.PickUpItem);
            return;
        }


        // if we are at the enemy's base, there's no flag or enemies or anything, then go back to our base.
        if (entity.HasReachedDestination() || Vector3.Distance(entity.transform.position, entity.AgentData.EnemyBase.transform.position) < AI.MinDistanceToBase)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
    }

    public override void Exit(AI entity)
    {

    }
}
