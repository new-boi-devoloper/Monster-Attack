using System;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Player_Scripts;

namespace Managers
{
    public class Shop : MonoBehaviour
    {
        [Serializable]
        public class AttackGroup
        {
            public string groupName; // Название группы, например "Level 1 - Type A"
            public List<AttackData> attacks;
        }
    
        public List<AttackGroup> attackGroups;
        private Dictionary<int, AttackData> _attackDictionary;

        private void Start()
        {
            _attackDictionary = new Dictionary<int, AttackData>();
            foreach (var group in attackGroups)
            {
                foreach (var attack in group.attacks)
                {
                    _attackDictionary[attack.attackId] = attack;
                }
            }

            // Загрузка состояния покупок атак
            UserInfo.Instance.LoadPurchasedAttacks(_attackDictionary);
        }

        public void BuyAttack(int attackId, PlayerCreature player)
        {
            Debug.Log($"Trying to buy attack with ID {attackId}");
            if (_attackDictionary.TryGetValue(attackId, out var attackToBuy))
            {
                if (player == null)
                {
                    Debug.LogError("Player reference is null in Shop.BuyAttack.");
                    return;
                }

                // Проверка на купленность атаки
                if (attackToBuy.IsPurchased)
                {
                    Debug.Log($"Attack with ID {attackId} is already purchased. Assigning to player.");
                    AssignAttackToPlayer(attackToBuy, player);
                    return;
                }

                // Проверка на наличие денег
                if (UserInfo.Instance.CoinCount >= attackToBuy.cost)
                {
                    Debug.Log($"Buying attack with ID {attackId} for {attackToBuy.cost} coins.");
                    UserInfo.Instance.ChangeCoin(-attackToBuy.cost);
                    UserInfo.Instance.PurchasedAttacks.Add(attackId);
                    attackToBuy.IsPurchased = true;
                    AssignAttackToPlayer(attackToBuy, player);
                }
                else
                {
                    Debug.Log($"Not enough money to buy attack - {attackToBuy.attackId}");
                }
            }
            else
            {
                Debug.LogError("Attack with ID " + attackId + " not found in the shop.");
            }
        }

        private void AssignAttackToPlayer(AttackData attack, PlayerCreature player)
        {
            if (attack.attackType == AttackType.Attack1)
            {
                Debug.Log($"Assigning attack with ID {attack.attackId} to currentAttack1.");
                player.currentAttack1 = attack;
            }
            else if (attack.attackType == AttackType.Attack2)
            {
                Debug.Log($"Assigning attack with ID {attack.attackId} to currentAttack2.");
                player.currentAttack2 = attack;
            }
            else
            {
                Debug.LogError($"Unknown attack type: {attack.attackType}");
            }
        }
    }
}