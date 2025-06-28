using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : StateBase
{
    private GameObject targetItem;

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

        if (entity.AgentSenses.GetFriendlyFlagInView()?.transform.parent != null)
        {
            _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
            return;
        }

        GameObject enemyFlag = entity.AgentSenses.GetEnemyFlagInView();

        // if the flag is 
        if (enemyFlag != null && enemyFlag.transform.parent == null)
        {
            _owningFSM.ChangeState(_owningFSM.StealEnemyFlag);
            return;
        }
        else if (enemyFlag != null && enemyFlag.transform.parent != null)
        {
            _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
            return;
        }


        entity.AgentActions.CollectItem(targetItem);


        if (targetItem == null)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
        }

    }

    public override void Exit(AI entity)
    {

    }
}
