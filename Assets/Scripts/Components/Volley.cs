using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : MonoBehaviour, IInputReceiver
{
    [SerializeField] private float _volleyBasePower;
    [SerializeField] private float _spikeBoostAmount;
    [SerializeField] private Button _volleyButton;
    [SerializeField] private float _spinModifier = 5f;

    private bool _inAir = true;
    private Vector2 _direction = Vector2.up;
    private Vector2 _spinDirection = Vector2.zero;

    private int _playerID;

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<PlayerEvents.TouchGroundEvent>(OnPlayerTouchedGround);

        _playerID = PlayerManager.Instance.GetPlayerID(this.gameObject);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<PlayerEvents.TouchGroundEvent>(OnPlayerTouchedGround);
    }

    public void OnAxisInput(Axis axis, Vector2 data)
    {
        if (axis == Axis.RStick)
        {
            _direction = data.normalized;
            if (_direction == Vector2.zero)
            {
                _direction = Vector2.up;
            }

            VSEventManager.Instance.TriggerEvent(new PlayerEvents.AimUpdateEvent(_playerID, _direction));
        }
        else if (axis == Axis.LTrigger)
        {
            _spinDirection.x = data.x;
        }
        else if (axis == Axis.RTrigger)
        {
            _spinDirection.y = -data.x;
        }
    }

    public void OnButtonPressed(Button b) { }
    public void OnButtonHeld(Button button, float duration) { }
    public void OnButtonReleased(Button b) { }

    private void OnPlayerTouchedGround(PlayerEvents.TouchGroundEvent e)
    {
        _inAir = !e.Touching;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            float shotPower = _volleyBasePower;
            bool isSpike = (_direction.y < 0f && _inAir);
            if (isSpike)
            {
                shotPower += _spikeBoostAmount;
            }

            VSEventManager.Instance.TriggerEvent(new PlayerEvents.BallVolliedEvent(_playerID, _direction, _spinDirection, _spinModifier, shotPower, isSpike));
        }
    }
}
