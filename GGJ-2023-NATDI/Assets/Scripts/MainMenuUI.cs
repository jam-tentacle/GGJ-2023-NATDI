using JetBrains.Annotations;
using UnityEngine;

namespace NATDI
{
    public class MainMenuUI : MonoBehaviour
    {
        [UsedImplicitly]
        public void ClickRestart()
        {
            Services.Get<FlowService>().RestartGame();
        }

        [UsedImplicitly]
        public void ClickPlay()
        {
            Services.Get<FlowService>().Resume();
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
