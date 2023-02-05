using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NATDI
{
    public class FlowService : Service
    {
        private UIService _uiService;
        public FlowState CurrentFlowState { get; private set; } = FlowState.Playing;

        public enum FlowState
        {
            Playing,
            Paused
        }

        private void Start()
        {
            Pause();
            _uiService = Services.Get<UIService>();
            _uiService.Tutorial.SetActive(true);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Gameplay");
        }

        public void Pause()
        {
            CurrentFlowState = FlowState.Paused;
            Services.Get<UIService>().MainMenu.SetActive(true);
        }

        public void Resume()
        {
            if (CurrentFlowState == FlowState.Playing)
            {
                return;
            }
            CurrentFlowState = FlowState.Playing;
            _uiService.MainMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentFlowState == FlowState.Paused)
                {
                    if (_uiService.Tutorial.gameObject.activeSelf)
                    {
                        _uiService.Tutorial.SetActive(false);
                    }
                    else
                    {
                        Resume();
                    }
                }
                else if (CurrentFlowState == FlowState.Playing)
                {
                    Pause();
                }
            }
        }
    }
}
