using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    private Transform _cachedTransform; // TODO this should be done in lots of other scripts as well
    private GameObject _parentPlayer;
    private int _playerID;
    private Vector2 _basePos;
    private float _headOffset;

    private void Awake()
    {
        _cachedTransform = this.transform;
        _parentPlayer = this.transform.parent.gameObject;
        _playerID = PlayerManager.Instance.GetPlayerID(_parentPlayer);

        _basePos = Utils.PositionXY(_cachedTransform.position);
        _headOffset = _basePos.y;
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<PlayerEvents.AimUpdateEvent>(OnAimUpdated);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<PlayerEvents.AimUpdateEvent>(OnAimUpdated);
    }

    private void OnAimUpdated(PlayerEvents.AimUpdateEvent e)
    {
        _basePos = _parentPlayer.transform.position;
        _basePos.y += _headOffset;

        if (_playerID == e.PlayerID)
        {
            Vector2 dir = _basePos + e.Direction * 0.5f;
            _cachedTransform.position = dir;
            _cachedTransform.rotation = Utils.LookAt2D(dir - _basePos);
        }
    }
}
