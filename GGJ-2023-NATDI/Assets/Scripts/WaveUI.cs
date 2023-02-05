using TMPro;
using UnityEngine;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private GameObject _leftTimeGO;
    [SerializeField] private string _leftTimePrefix;
    [SerializeField] private TMP_Text _leftTimeText;

    [SerializeField] private string _leftWavesPrefix;
    [SerializeField] private TMP_Text _leftWavesText;

    [SerializeField] private string _leftEnemiesPrefix = "Enemies: ";
    [SerializeField] private TMP_Text _leftEnemiesText;

    public void SetContent(bool hasLeftTime, float leftTime, int leftWaves, int enemies)
    {
        if (hasLeftTime)
        {
            _leftTimeGO.SetActive(true);
            _leftTimeText.text = $"{_leftTimePrefix}{leftTime}s";
        }
        else
        {
            _leftTimeGO.SetActive(false);
        }

        _leftWavesText.text = $"{_leftWavesPrefix}{leftWaves}";

        _leftEnemiesText.text = $"{_leftEnemiesPrefix}{enemies}";
    }
}
