using System.Collections;
using Player_Scripts;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFeedback : MonoBehaviour
{
    public Button attack1Button;
    public Button attack2Button;
    [SerializeField] private float pressDuration = 0.2f; // Длительность нажатия кнопки
    [SerializeField] private Color pressedColor = Color.gray; // Цвет кнопки при нажатии

    private Color originalColor1;
    private Color originalColor2;

    private void Start()
    {
        // Сохраняем оригинальные цвета кнопок
        originalColor1 = attack1Button.colors.normalColor;
        originalColor2 = attack2Button.colors.normalColor;

        // Подписываемся на события атаки
        PlayerCreature.OnAttack1 += OnAttack1Pressed;
        PlayerCreature.OnAttack2 += OnAttack2Pressed;
    }

    private void OnDestroy()
    {
        // Отписываемся от событий атаки
        PlayerCreature.OnAttack1 -= OnAttack1Pressed;
        PlayerCreature.OnAttack2 -= OnAttack2Pressed;
    }

    private void OnAttack1Pressed()
    {
        // Имитируем нажатие кнопки attack1Button
        StartCoroutine(PressButton(attack1Button, originalColor1));
    }

    private void OnAttack2Pressed()
    {
        // Имитируем нажатие кнопки attack2Button
        StartCoroutine(PressButton(attack2Button, originalColor2));
    }

    private IEnumerator PressButton(Button button, Color originalColor)
    {
        // Изменяем цвет кнопки при нажатии
        ChangeButtonColor(button, pressedColor);
        Debug.Log("1");

        // Ждем заданное время
        yield return new WaitForSeconds(pressDuration);

        // Возвращаем кнопку в исходное состояние
        ChangeButtonColor(button, originalColor);
        Debug.Log("2");
    }

    private void ChangeButtonColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}