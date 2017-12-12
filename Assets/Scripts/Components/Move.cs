using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour, IInputReceiver
{
    [SerializeField] private float _moveSpeed;

    private Rigidbody2D _rigidBody;
    private bool _canMove = true;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.AddListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.AddListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnBallHeld(PlayerEvents.BallHeldEvent e)
    {
        // for now, remove these id checks. it makes for more tense gameplay when no players can move while someone is charging a shot

        //if (e.PlayerID == PlayerManager.Instance.GetPlayerID(this.gameObject))
        //{
            _canMove = false;
        //}
    }

    private void OnBallVollied(PlayerEvents.BallVolliedEvent e)
    {
        //if (e.PlayerID == PlayerManager.Instance.GetPlayerID(this.gameObject))
        //{
            _canMove = true;
        //}
    }

    private void OnBallDropped(PlayerEvents.BallDroppedEvent e)
    {
        //if (e.PlayerID == PlayerManager.Instance.GetPlayerID(this.gameObject))
        //{
            _canMove = true;
        //}
    }

    public void OnAxisInput(Axis axis, Vector2 data)
    {
        _rigidBody.bodyType = (_canMove ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic);

        if (_canMove && axis == Axis.LStick)
        {
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;

            Vector2 movement = data * _moveSpeed * Time.fixedDeltaTime;
            movement.y = _rigidBody.velocity.y;

            _rigidBody.velocity = movement;
        }
    }

    public void OnButtonPressed(Button b) { }
    public void OnButtonHeld(Button button, float duration) { }
    public void OnButtonReleased(Button b) { }

}
