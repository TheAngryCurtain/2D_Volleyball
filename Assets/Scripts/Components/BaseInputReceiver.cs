using System;
using System.Collections.Generic;
using GameInput;
using UnityEngine;

public class BaseInputReceiver : MonoBehaviour
{
    protected IInputReceiver[] _receivers;
    protected bool _inputEnabled = true;
    protected int _playerID;

    protected virtual void Awake()
    {
        _receivers = GetComponentsInChildren<IInputReceiver>();
    }

    protected virtual void Start()
    {
        _playerID = PlayerManager.Instance.GetPlayerID(this.gameObject);
    }

    protected virtual void OnEnable()
    {
        VSEventManager.Instance.AddListener<GameEvents.EnableInputEvent>(OnEnableInput);
    }

    protected virtual void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<GameEvents.EnableInputEvent>(OnEnableInput);
    }

    protected virtual void OnEnableInput(GameEvents.EnableInputEvent e)
    {
        if (e.ControllerID == _playerID)
        {
            _inputEnabled = e.Enable;
        }
    }

    protected virtual void OnAxisInput(int playerIndex, Axis axis, Vector2 data)
    {
        if (!_inputEnabled) return;

        for (int i = 0; i < _receivers.Length; i++)
        {
            if (_playerID == playerIndex)
            {
                _receivers[i].OnAxisInput(axis, data);
            }
        }
    }

    protected virtual void OnButtonPress(int playerIndex, Button button)
    {
        if (!_inputEnabled) return;

        for (int i = 0; i < _receivers.Length; i++)
        {
            if (_playerID == playerIndex)
            {
                _receivers[i].OnButtonPressed(button);
            }
        }
    }

    protected virtual void OnButtonHeld(int playerIndex, Button button, float duration)
    {
        if (!_inputEnabled) return;

        for (int i = 0; i < _receivers.Length; i++)
        {
            if (_playerID == playerIndex)
            {
                _receivers[i].OnButtonHeld(button, duration);
            }
        }
    }

    protected virtual void OnButtonRelease(int playerIndex, Button button)
    {
        if (!_inputEnabled) return;

        for (int i = 0; i < _receivers.Length; i++)
        {
            if (_playerID == playerIndex)
            {
                _receivers[i].OnButtonReleased(button);
            }
        }
    }
}
