using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BankGold : Action
//{
// public BankGold(AI agent):base(agent)
// {

// }

// public override void Execute(float deltaTime)
// {
//     if(GoToBank())
//     {
//         DepositGoldAtBank();
//     }
// }

// // Go to the bank
// public bool GoToBank()
// {
//     // If we're not trying to go to the bank, set bank as target location
//     if (_agent.TargetLocation != Locations.Bank)
//     {
//         _agent.ChangeLocation(Locations.Bank);
//     }

//     // Are we at the bank yet?
//     if (_agent.CurrentLocation != _agent.TargetLocation)
//     {
//         Debug.Log("Going to the bank...");
//         _agent.ChangeLocation(Locations.Bank);
//         return false;
//     }
//     return true;
// }

// // We have enough gold to go to the bank and deposit it
// public bool DepositGoldAtBank()
// {
//     // We're at the bank, deposit the gold
//     Debug.Log("Depositing " + _agent.MoneyInBank + " in the bank");
//     _agent.MoneyInBank += _agent.GoldCarried;
//     _agent.AI.UpdateGoals(GoalLabels.MineGold, _agent.MoneyInBank);

//     _agent.GoldCarried = 0;
//     _agent.AI.UpdateGoals(GoalLabels.BankGold, _agent.GoldCarried);

//     _complete = true;
//     return _complete;
// }

// public override string ToString()
// {
//     return "Bank Gold";
// }
//}
