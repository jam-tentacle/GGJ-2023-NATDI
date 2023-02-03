using TMPro;
using UnityEngine;

public class TestMushroomerUI : MonoBehaviour
{
    [SerializeField] private Damageable _mushroomer;
    [SerializeField] private TMP_Text _text;

    private void Update()
    {
        _text.text = GetText();
    }

    private string GetText()
    {
        return $"Hp = {_mushroomer.Health}";
    }
}
