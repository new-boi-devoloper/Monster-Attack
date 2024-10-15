using UnityEngine;

public class PlayAttackAnim : MonoBehaviour
{
    public void PlayParticleSystem(GameObject particleSystemPrefab, Vector3 position, bool isFacingRight, float scaleMultiplier = 1.0f)
    {
        if (particleSystemPrefab != null)
        {
            var instantiatedParticle = Instantiate(particleSystemPrefab, position, Quaternion.identity);
            var particleSystem = instantiatedParticle.GetComponent<ParticleSystem>();
            
            if (particleSystem != null)
            {
                // Изменение размера частиц
                var main = particleSystem.main;
                main.startSizeMultiplier *= scaleMultiplier;
                
                // Изменение направления частиц
                if (!isFacingRight)
                {
                    var currentScale = instantiatedParticle.transform.localScale;
                    currentScale.x *= -1;
                    instantiatedParticle.transform.localScale = currentScale;
                }
                
                particleSystem.Play();
                
                // Уничтожить частицы после их завершения
                Destroy(instantiatedParticle, particleSystem.main.duration);
            }
            else
            {
                Debug.LogError("ParticleSystem component not found on the prefab.");
                Destroy(instantiatedParticle);
            }
        }
    }
}