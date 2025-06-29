using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : StateBase
{
    private GameObject _targetEnemy;

    public AttackEnemy(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        _targetEnemy = entity.AgentSenses.GetNearestEnemyInView();


        // hope this works. check if any enemy has flag lose-ly. 
        if (entity.AgentSenses.GetFriendlyFlagInView()?.transform.parent != null)
        {
            if (entity.AgentSenses.GetFriendlyFlagInView()?.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
            }
        }
        else if (entity.AgentSenses.GetEnemyFlagInView()?.transform.parent != null)
        {
            if (entity.AgentSenses.GetEnemyFlagInView()?.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
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
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }
        else
        {
            if (Vector3.Distance(_targetEnemy.transform.position, entity.transform.position) > AI.MaxRangeToAttackEnemy)
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
