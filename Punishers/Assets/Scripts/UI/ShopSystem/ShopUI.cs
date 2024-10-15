using System.Collections.Generic;
using Managers;
using Player_Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Shop shop;
    public GameObject buttonPrefab; // Префаб кнопки покупки
    public Transform contentTransform; // Transform контейнера в ScrollView
    public PlayerCreature player; // Ссылка на игрока
    public string groupName; // Название группы атак для этого ScrollView

    private void Start()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        Debug.Log($"Updating Shop UI for group: {groupName}");

        // Очищаем существующие кнопки
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // Находим группу атак по названию
        var group = shop.attackGroups.Find(g => g.groupName == groupName);
        if (group != null)
        {
            Debug.Log($"Found group: {group.groupName} with {group.attacks.Count} attacks");

            // Создаем новые кнопки на основе данных из группы
            foreach (var attack in group.attacks)
            {
                GameObject buttonObj = Instantiate(buttonPrefab, contentTransform);
                AttackPurchaseButton purchaseButton = buttonObj.GetComponent<AttackPurchaseButton>();
                if (purchaseButton != null)
                {
                    purchaseButton.attackData = attack;
                    purchaseButton.shop = shop;
                    purchaseButton.player = player; // Назначаем ссылку на игрока
                    purchaseButton.UpdateUI();
                    Debug.Log($"Created button for attack: {attack.attackId}");
                }
                else
                {
                    Debug.LogError("AttackPurchaseButton component is missing on the button prefab.");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Group with name {groupName} not found in the shop.");
        }
    }
}