using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject[] _managers;

    private void OnEnable()
    {
        for (int i = 0; i < _managers.Length; i++)
        {
            Instantiate(_managers[i], Vector3.zero, Quaternion.identity);
        }
    }
}
