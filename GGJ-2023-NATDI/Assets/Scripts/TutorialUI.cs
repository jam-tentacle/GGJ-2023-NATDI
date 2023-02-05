using JetBrains.Annotations;
using System;
using UnityEngine;

namespace NATDI
{
    public class TutorialUI : MonoBehaviour
    {
        private bool _showOnStart = true;

        [UsedImplicitly]
        public void ClickOk()
        {
            Close();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }

        private void Close()
        {
            if (_showOnStart)
            {
                _showOnStart = false;
                Services.Get<FlowService>().Resume();
            }
            SetActive(false);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
