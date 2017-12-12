using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _netHitDampenFactor;
    [SerializeField] private float _playerHitDampenFactor;

    [SerializeField] private TrailRenderer _trail;

    private const float BaseTrajectoryModifier = 15.25f;
    private const float BackwardSpinTrajectoryModifier = 19.6f;
    private const float ForwardSpinTrajectoryModifier = 12.85f;

    private Rigidbody2D _rigidbody;
    private Vector3 _startPos;
    private Vector3 _servePos;
    private int _playerHoldingID;
    private Vector2 _spinValues;
    private float _spin;
    private float _spinModifier;

    private GameEvents.BallPositionUpdateEvent _ballPosUpdateEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _startPos = transform.position;
        _ballPosUpdateEvent = new GameEvents.BallPositionUpdateEvent();

        VSEventManager.Instance.TriggerEvent(new GameEvents.BallSpawnedEvent(this.transform));
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        //VSEventManager.Instance.AddListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);

        VSEventManager.Instance.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.AddListener<GameEvents.BallTouchNetEvent>(OnNetHit);
        //VSEventManager.Instance.AddListener<GameEvents.BallTouchPlayerEvent>(OnPlayerHit);
    }

    private void OnDestroy()
    {
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        //VSEventManager.Instance.RemoveListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);

        VSEventManager.Instance.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.RemoveListener<GameEvents.BallTouchNetEvent>(OnNetHit);
        //VSEventManager.Instance.RemoveListener<GameEvents.BallTouchPlayerEvent>(OnPlayerHit);
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent e)
    {
        if (e.NewState == Game.eGameState.Serve)
        {
            _trail.enabled = false;

            int serveSide = e.ParamData[0];
            _servePos = _startPos;
            _servePos.x *= (serveSide == (int)Game.eTeam.Away ? -1 : 1);

            transform.position = _servePos;

            //HoldBall(true);

            SetTrailColor(Color.white);
        }
    }

    private void OnNetHit(GameEvents.BallTouchNetEvent e)
    {
        _rigidbody.velocity *= _netHitDampenFactor;

        // update trajectory
        UpdateTrajectory(e.RelativeVelocity.normalized, _rigidbody.velocity.magnitude, 1f, Color.blue);
    }

    //private void OnPlayerHit(GameEvents.BallTouchPlayerEvent e)
    //{
    //    _rigidbody.velocity *= _playerHitDampenFactor;

    //    // update trajectory
    //    UpdateTrajectory(e.RelativeVelocity.normalized, _rigidbody.velocity.magnitude, 1f, Color.green);
    //}

    private void OnBallVollied(PlayerEvents.BallVolliedEvent e)
    {
        // only allow the player who is holding it to volley it

        // NOTE: currently, the holdingID doesn't get set for AI, so they won't shoot at all
        //if (_playerHoldingID == e.PlayerID)
        //{

            // stop all momentum, so that the shots are accurate to their aim locations
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;

            // fun with trail colors
            SetTrailColor(Settings.Instance.GetPlayerColor(e.PlayerID));

            //HoldBall(false);
            _rigidbody.AddForce(e.Direction * e.HoldTime * e.Power);

            // spin
            _spinValues = e.Spins;
            _spinModifier = e.SpinModifier;
            _spin = _spinValues.x + _spinValues.y;
            _rigidbody.AddTorque(_spin * _spinModifier);

            // predict trajectory
            float ballTravelDirection = Mathf.Sign(_rigidbody.velocity.x);
            float magicalTrajectoryValue = BaseTrajectoryModifier;
            Color debugColor = Color.red;
            if (_spin * ballTravelDirection > 0f)
            {
                float delta = BackwardSpinTrajectoryModifier - BaseTrajectoryModifier;
                magicalTrajectoryValue = BaseTrajectoryModifier + delta * _spin;
                debugColor = Color.green;
            }
            else if (_spin * ballTravelDirection < 0f)
            {
                float delta = ForwardSpinTrajectoryModifier - BaseTrajectoryModifier;
                magicalTrajectoryValue = BaseTrajectoryModifier - delta * _spin;
                debugColor = Color.blue;
            }

            UpdateTrajectory(e.Direction, e.HoldTime, magicalTrajectoryValue, debugColor);
        //}

        _trail.enabled = true;
    }

    //private void OnBallDropped(PlayerEvents.BallDroppedEvent e)
    //{
    //    HoldBall(false);
    //}

    private void SetTrailColor(Color color)
    {
        color.a = 0.25f;
        _trail.material.SetColor("_Color", color);
    }

    //private void HoldBall(bool hold)
    //{
    //    if (hold)
    //    {
    //        _rigidbody.bodyType = RigidbodyType2D.Kinematic;
    //        _rigidbody.velocity = Vector2.zero;
    //        _rigidbody.angularVelocity = 0f;
    //    }
    //    else
    //    {
    //        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    //    }
    //}

    private void UpdateTrajectory(Vector3 direction, float holdTime, float modifier, Color color)
    {
        Vector2 endPos = Utils.FindTrajectoryEndPostion(transform.position, direction * (holdTime * modifier), color, true);

        VSEventManager.Instance.TriggerEvent(new GameEvents.TrajectoryEndEvent(endPos));
    }

    private void Update()
    {
        _ballPosUpdateEvent.Update(Utils.PositionXY(transform.position));
        VSEventManager.Instance.TriggerEvent(_ballPosUpdateEvent);
    }

    private void FixedUpdate()
    {
        if (_spin != 0f)
        {
            _rigidbody.AddForce(Vector2.up * _spin, ForceMode2D.Force);
        }
    }
}
