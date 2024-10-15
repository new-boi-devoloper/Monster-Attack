using UnityEngine;
using UnityEngine.UI;

public class PlatformDependentImage : MonoBehaviour
{
    public Sprite mobileSprite;
    public Sprite pcSprite;

    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogError("Image component not found on this GameObject.");
            return;
        }

        // Определяем платформу и меняем картинку
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            imageComponent.sprite = mobileSprite;
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor ||
                 Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor ||
                 Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor)
        {
            imageComponent.sprite = pcSprite;
        }
    }
}