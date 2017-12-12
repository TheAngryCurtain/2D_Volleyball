using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volley : MonoBehaviour, IInputReceiver
{
    [SerializeField] private float _volleyBasePower;
    [SerializeField] private GameInput.Button _volleyButton;
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

    public void OnAxisInput(GameInput.Axis axis, Vector2 data)
    {
        if (axis == GameInput.Axis.RStick)
        {
            _direction = data.normalized;
            if (_direction == Vector2.zero)
            {
                _direction = Vector2.up;
            }

            VSEventManager.Instance.TriggerEvent(new PlayerEvents.AimUpdateEvent(_playerID, _direction));
        }
        else if (axis == GameInput.Axis.LTrigger)
        {
            _spinDirection.x = data.x;
        }
        else if (axis == GameInput.Axis.RTrigger)
        {
            _spinDirection.y = -data.x;
        }
    }

    public void OnButtonPressed(GameInput.Button b) { }
    public void OnButtonHeld(GameInput.Button button, float duration) { }
    public void OnButtonReleased(GameInput.Button b) { }

    private void OnPlayerTouchedGround(PlayerEvents.TouchGroundEvent e)
    {
        _inAir = !e.Touching;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            // so by changing the length of the direction vector, this adjusts the power/range already
            // this can be left for now
            float holdTime = 0.75f;
            VSEventManager.Instance.TriggerEvent(new PlayerEvents.BallVolliedEvent(_playerID, _direction, _spinDirection, _spinModifier, holdTime, _volleyBasePower));
        }
    }
}
