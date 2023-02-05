using JetBrains.Annotations;
using System;
using UnityEngine;

namespace NATDI
{
    public class TutorialUI : MonoBehaviour
    {
        [UsedImplicitly]
        public void ClickOk()
        {
            SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetActive(false);
            }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
