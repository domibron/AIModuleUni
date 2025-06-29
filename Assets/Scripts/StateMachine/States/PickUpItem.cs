using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : StateBase
{
    private GameObject targetItem;

    public PickUpItem(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        targetItem = entity.AgentSenses.GetNearestCollectableInView();

        if (targetItem == null)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }

        entity.AgentActions.MoveTo(targetItem);
    }

    public override void Execute(AI entity)
    {


        if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        {
            float enemyDistanceToItem = float.MaxValue;


            foreach (var enemy in entity.AgentSenses.GetEnemiesInView())
            {
                float distCalc = Vector3.Distance(enemy.transform.position, targetItem.transform.position);


                if (distCalc < enemyDistanceToItem)
                {
                    enemyDistanceToItem = distCalc;
                }
            }

            if (enemyDistanceToItem < Vector3.Distance(entity.transform.position, targetItem.transform.position))
            {
                _owningFSM.ChangeState(_owningFSM.AttackEnemy);
                return;
            }
        }

        if (entity.AgentSenses.GetFriendliesInView().Count > 0)
        {
            float enemyDistanceToItem = float.MaxValue;


            foreach (var enemy in entity.AgentSenses.GetFriendliesInView())
            {
                float distCalc = Vector3.Distance(enemy.transform.position, targetItem.transform.position);


                if (distCalc < enemyDistanceToItem)
                {
                    enemyDistanceToItem = distCalc;
                }
            }

            if (enemyDistanceToItem < Vector3.Distance(entity.transform.position, targetItem.transform.position))
            {
                _owningFSM.ChangeState(_owningFSM.GoToBase);
                return;
            }
        }

        if (entity.AgentSenses.GetFriendlyFlagInView()?.transform.parent != null)
        {
            if (entity.AgentSenses.GetFriendlyFlagInView()?.transform.parent.tag == entity.AgentData.EnemyTeamTag)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }
        }

        if (entity.AgentSenses.IsItemInReach(targetItem))
        {
            entity.AgentActions.CollectItem(targetItem);

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
    }

    public override void Exit(AI entity)
    {

    }
}
