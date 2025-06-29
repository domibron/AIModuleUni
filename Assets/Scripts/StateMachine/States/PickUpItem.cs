using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : StateBase
{
    /// <summary>
    /// The target item we are seeking.
    /// </summary>
    private GameObject targetItem;

    public PickUpItem(FiniteStateMachine fsm) : base(fsm)
    {
    }

    public override void Enter(AI entity)
    {
        // Get the item we want to get.
        targetItem = entity.AgentSenses.GetNearestCollectableInView();

        // if there are no items, then we can exit this state.
        if (targetItem == null)
        {
            _owningFSM.ChangeState(_owningFSM.GoToBase);
            return;
        }

        // go to the item.
        entity.AgentActions.MoveTo(targetItem);
    }

    public override void Execute(AI entity)
    {

        // if there are enemies in view, see if we are closer or they are. If the enemy are closer, we attack.
        // TODO check to see if we are closer to the enemy compared to the item.
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

        // If there are friendlies in view, see if we are closer otherwise we let them get the item.
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

        // once we can get the item or to the location, collect the item and go back to routine.
        if (entity.AgentSenses.IsItemInReach(targetItem) || entity.HasReachedDestination())
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
