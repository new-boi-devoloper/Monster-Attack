using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeScene(string sceneToTurnOn)
        {
            SceneManager.LoadScene(sceneToTurnOn);
        }
    }
}
