using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestMushroomerUI : MonoBehaviour
{
    [SerializeField] private Damageable _mushroomer;
    // [SerializeField] private TMP_Text _text;
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _slider.value = GetValue();
        // _text.text = GetText();
    }

    private float GetValue() => _mushroomer.Percentage;

    private string GetText()
    {
        return $"Hp = {_mushroomer.Health}";
    }
}
