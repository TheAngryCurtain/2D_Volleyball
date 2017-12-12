using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private bool _inAir = true;
    private Vector3 _startPos;

    private void Awake()
    {
        PlayerManager.Instance.RegisterPlayer(this.gameObject);

        _rigidbody = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.AddListener<GameEvents.BallTouchFloorEvent>(OnBallTouchFloor);

        VSEventManager.Instance.AddListener<PlayerEvents.TouchGroundEvent>(OnTouchedGround);
        VSEventManager.Instance.AddListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.AddListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.AddListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.RemoveListener<GameEvents.BallTouchFloorEvent>(OnBallTouchFloor);

        VSEventManager.Instance.RemoveListener<PlayerEvents.TouchGroundEvent>(OnTouchedGround);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent e)
    {
        if (e.NewState == Game.eGameState.Serve)
        {
            // reset to starting position
            transform.position = _startPos;
        }
    }

    private void OnTouchedGround(PlayerEvents.TouchGroundEvent e)
    {
        _inAir = !e.Touching;
    }

    private void OnBallHeld(PlayerEvents.BallHeldEvent e)
    {
        _rigidbody.velocity = Vector2.zero;

        if (_inAir)
        {
            SetIsKinematic(true);
        }
    }

    private void OnBallVollied(PlayerEvents.BallVolliedEvent e)
    {
        SetIsKinematic(false);
    }

    private void OnBallDropped(PlayerEvents.BallDroppedEvent e)
    {
        SetIsKinematic(false);
    }

    private void OnBallTouchFloor(GameEvents.BallTouchFloorEvent e)
    {
        //Debug.LogFormat("Player {0} Saw ball touch floor!", PlayerManager.Instance.GetPlayerID(this.gameObject));
    }

    private void SetIsKinematic(bool enable)
    {
        _rigidbody.isKinematic = enable;
    }
}
