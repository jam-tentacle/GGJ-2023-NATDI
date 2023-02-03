using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NATDI
{
    public class FlowService : Service
    {
        public FlowState CurrentFlowState { get; private set; } = FlowState.Playing;

        public enum FlowState
        {
            Playing,
            Paused
        }

        private void Start()
        {
            Services.Get<UIService>().MainMenu.SetActive(false);
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
            Services.Get<UIService>().MainMenu.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (CurrentFlowState == FlowState.Paused)
                {
                    Resume();
                }
                else if (CurrentFlowState == FlowState.Playing)
                {
                    Pause();
                }
            }
        }
    }
}
