using JetBrains.Annotations;
using System;
using UnityEngine;

namespace NATDI
{
    public class MainMenuUI : MonoBehaviour
    {
        private UIService _uiService;

        private void Start()
        {
            _uiService = Services.Get<UIService>();
        }

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

        [UsedImplicitly]
        public void ClickTutorial()
        {
            _uiService.Tutorial.SetActive(true);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
