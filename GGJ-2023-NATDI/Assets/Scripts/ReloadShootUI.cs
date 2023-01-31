using TMPro;
using UnityEngine;

public class ReloadShootUI : MonoBehaviour
{
    [SerializeField] private ProgressBarUI _progressBar;
    [SerializeField] private TextMeshProUGUI _reloadShootText;


    public void UpdateView(float leftReloadTime, float maxReloadTime)
    {
        _progressBar.SetValue(leftReloadTime, maxReloadTime);

        if (leftReloadTime <= 0)
        {
            _reloadShootText.gameObject.SetActive(false);
        }
        else
        {
            _reloadShootText.gameObject.SetActive(true);
            _reloadShootText.text = ((int)leftReloadTime).ToString();
        }
    }
}
