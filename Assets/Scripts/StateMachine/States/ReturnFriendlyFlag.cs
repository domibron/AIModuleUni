using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnFriendlyFlag : StateBase
{
    private GameObject _friendlyFlag;

    private bool _checkedBase = false;


    public override void Enter(AI entity)
    {
        _friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();
        _checkedBase = false;

        entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
    }

    public override void Execute(AI entity)
    {

        // not sure about these to checks. maybe just return to base.
        // if (entity.AgentData.CurrentHitPoints < entity.AgentData.MaxHitPoints / 4f)
        // {
        //     _owningFSM.ChangeState(_owningFSM.Heal);
        //     return;
        // }

        // if (entity.AgentSenses.GetEnemiesInView().Count > 0)
        // {
        //     _owningFSM.ChangeState(_owningFSM.AttackEnemy);
        //     return;
        // }


        if (!_checkedBase)
        {
            if (entity.HasReachedDestination())
            {
                // we are at the base. lets check for the flag.
                _friendlyFlag = entity.AgentSenses.GetFriendlyFlagInView();
                _checkedBase = true;
            }

            return;
        }

        if (_friendlyFlag != null && !entity.AgentData.HasFriendlyFlag)
        {
            if (_friendlyFlag.transform.parent != null)
            {
                _owningFSM.ChangeState(_owningFSM.AttackFlagCarrier);
                return;
            }

            entity.AgentActions.MoveTo(_friendlyFlag);
            entity.AgentActions.CollectItem(_friendlyFlag);
        }
        else if (entity.AgentData.HasFriendlyFlag && Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) > AI.MinDistanceToBaseToDrop) // magic number
        {
            entity.AgentActions.MoveTo(entity.AgentData.FriendlyBase);
        }
        else if (entity.AgentData.HasFriendlyFlag && Vector3.Distance(entity.transform.position, entity.AgentData.FriendlyBase.transform.position) <= AI.MinDistanceToBaseToDrop)
        {
            _owningFSM.ChangeState(_owningFSM.GoToEnemyBase);
        }

        // we checked the 
    }

    public override void Exit(AI entity)
    {
        entity.AgentActions.DropItem(entity.AgentInventory.GetItem(entity.AgentData.FriendlyFlagName));
    }
}
