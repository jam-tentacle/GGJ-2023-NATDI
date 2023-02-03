using System;
using System.Collections;
using UnityEngine;

public class MushroomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private float _passedTime;

    private void FixedUpdate()
    {
        _passedTime += Time.deltaTime;
        if (!(_passedTime >= 5f)) return;

        Instantiate(_gameObject, transform.position, Quaternion.identity);
        _passedTime = 0f;
    }
}
