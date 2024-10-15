using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Player_Scripts;

public class AttackPurchaseButton : MonoBehaviour
{
    public TextMeshProUGUI buttonTextPrice;
    public TextMeshProUGUI buttonTextAttackMultiplier;
    public Image buttonImage;
    public AttackData attackData;
    public Shop shop;
    public PlayerCreature player; // Ссылка на игрока

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        buttonTextPrice.text = attackData.IsPurchased ? "0" : attackData.cost.ToString();
        buttonTextAttackMultiplier.text = attackData.attackMultiplier.ToString();
        buttonImage.sprite = attackData.attackSprite;
    }

    public void OnButtonClick()
    {
        Debug.Log($"Attempting to buy attack with ID {attackData.attackId}");
        shop.BuyAttack(attackData.attackId, player);
        UpdateUI(); // Обновляем UI после покупки
    }
}