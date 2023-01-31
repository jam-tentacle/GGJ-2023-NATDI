using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    public void SetValue(float current, float max)
    {
        _fillImage.fillAmount = current / max;
    }
}
