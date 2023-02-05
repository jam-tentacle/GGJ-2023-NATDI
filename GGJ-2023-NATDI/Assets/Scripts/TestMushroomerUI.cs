using UnityEngine;
using UnityEngine.UI;

public class TestMushroomerUI : MonoBehaviour
{
    [SerializeField] private Damageable _mushroomer;

    [SerializeField] private Slider _slider;

    private void Update()
    {
        _slider.value = GetValue();
    }

    private float GetValue() => _mushroomer.Percentage;
}
