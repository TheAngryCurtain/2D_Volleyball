using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offset;
    [SerializeField] private float _maxAngle;
    [SerializeField] private float _minAngle;

    private Transform _cachedTransform;

    private void Awake()
    {
        _cachedTransform = GetComponent<Transform>();
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<GameEvents.BallSpawnedEvent>(OnBallSpawned);
    }

    private void OnDestroy()
    {
        VSEventManager.Instance.RemoveListener<GameEvents.BallSpawnedEvent>(OnBallSpawned);
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            Vector3 des = _target.position - _cachedTransform.position;
            float rotZ = Mathf.Atan2(des.y, des.x) * Mathf.Rad2Deg;

            if (rotZ < _maxAngle && rotZ > _minAngle)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ - _offset);
            }
        }
    }

    private void OnBallSpawned(GameEvents.BallSpawnedEvent e)
    {
        _target = e.BallTransform;
    }
}
