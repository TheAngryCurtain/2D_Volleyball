using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputReceiver : BaseInputReceiver
{
    // TODO
    // AI can determine their own course of action from here
    // if they are close enough to the ball, they should call for it so other players can take a support position
    // if the ball is up in the air, they can jump for it and try to make a play

    private bool _hasTarget = false;
    private float _targetX;
    private bool _isJumping = false;
    private float _jumpTime = 0f;
    private Vector2 _currentBallPos;
    private Vector2 _aimLocation;
    private Game.eGameState _currentGameState;

    private float _spinDirection = 0f;
    private Transform _opposingPlayerTransform;

    protected override void OnEnable()
    {
        VSEventManager.Instance.AddListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.AddListener<GameEvents.PlayerSpawnedEvent>(OnPlayerSpawned);
        VSEventManager.Instance.AddListener<GameEvents.BallTouchFloorEvent>(OnBallHitFloor);
        VSEventManager.Instance.AddListener<GameEvents.BallPositionUpdateEvent>(OnBallPositionUpdate);
        VSEventManager.Instance.AddListener<GameEvents.TrajectoryEndEvent>(OnTrajectoryEndFound);

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<GameEvents.GameStateChangedEvent>(OnGameStateChanged);
        VSEventManager.Instance.RemoveListener<GameEvents.PlayerSpawnedEvent>(OnPlayerSpawned);
        VSEventManager.Instance.RemoveListener<GameEvents.BallTouchFloorEvent>(OnBallHitFloor);
        VSEventManager.Instance.RemoveListener<GameEvents.BallPositionUpdateEvent>(OnBallPositionUpdate);
        VSEventManager.Instance.RemoveListener<GameEvents.TrajectoryEndEvent>(OnTrajectoryEndFound);

        base.OnDisable();
    }

    private void OnGameStateChanged(GameEvents.GameStateChangedEvent e)
    {
        _currentGameState = e.NewState;
    }

    private void OnPlayerSpawned(GameEvents.PlayerSpawnedEvent e)
    {
        _opposingPlayerTransform = e.PlayerTransform;
    }

    private void OnBallPositionUpdate(GameEvents.BallPositionUpdateEvent e)
    {
        _currentBallPos = e.CurrentPosition;
    }

    private void OnTrajectoryEndFound(GameEvents.TrajectoryEndEvent e)
    {
        // TODO
        // check to see if you're the closet to the ball
        //  > create a strategy class, and assign a strategy based on number of players per team, and position (front/back court, if 2 players)
        //  > create a difficulty class, they they can pull some values from
        // if so, fire an event to let the team know and then set the targetX and hasTarget

        //determine if the ball was thrown by someone on your team
        // NOTE: this may not need to be checked? if the ball was launched and it's coming to your side, you should get it anyway
        int currentPlayerID = PlayerManager.Instance.GetPlayerID(this.gameObject);
        int currentTeamID = PlayerManager.Instance.GetTeamID(currentPlayerID);

        //int launchingTeamID = PlayerManager.Instance.GetTeamID(e.PlayerID);

        // determine if it's coming to your side of the court
        // TODO this needs to be moved out of here eventually
        float netXPos = LevelManager.Instance.GetNetPosition().x;
        bool onTeamSide = (currentTeamID == (int)Game.eTeam.Away && e.TrajectoryEndPosition.x > netXPos) || (currentTeamID == (int)Game.eTeam.Home && e.TrajectoryEndPosition.x < netXPos);

        // TODO this value should change with difficulty, so that easy players will try for OOB shots, and hard players won't
        bool isOOB = e.TrajectoryEndPosition.x > LevelManager.Instance.GetOOBXForSide(Game.eTeam.Away); // TEST for away team only! will need to be changed

        //if (launchingTeamID != currentTeamID)
        if (onTeamSide && !isOOB)
        {
            _targetX = e.TrajectoryEndPosition.x - 0.5f; // TODO account for side of court you're on. If you're on the right, it's -0.5, else +0.5
            _hasTarget = true;
        }
    }

    private void OnBallHitFloor(GameEvents.BallTouchFloorEvent e)
    {
        // for now, just stop chasing if the ball hits the floor
        _hasTarget = false;
        _targetX = transform.position.x;
    }

    private void Update()
    {
        UpdateShotPlacement();
        UpdateAimLocation();
        UpdateMovement();
    }

    private void UpdateShotPlacement()
    {
        _spinDirection = 0f;
        switch (_currentGameState)
        {
            case Game.eGameState.Serve:
                _spinDirection = 0.75f; // TODO take into account the side of the court
                break;

            case Game.eGameState.Play:
                if (_opposingPlayerTransform != null)
                {
                    // TODO
                    // get the position of the player and determine whether the shot should...
                    // have backspin (+) if the player is near the net (make sure it's not oob?)
                    // have forward spin (-) if the player is away from the net
                }
                break;
        }

        OnAxisInput(_playerID, Axis.LTrigger, new Vector2(_spinDirection, 0f));
    }

    private void UpdateAimLocation()
    {
        switch (_currentGameState)
        {
            case Game.eGameState.Serve:
                //_aimLocation = Vector2.up;
                //break;

            case Game.eGameState.Play:
                // TODO when strategies are figured out, adjust the length of the aimLocation to vary the power on AI shots

                // test aim, of slightly above the net
                Vector2 aboveNet = LevelManager.Instance.GetNetPosition() + Vector2.up * 8f;
                _aimLocation = (aboveNet - Utils.PositionXY(transform.position)).normalized;
                break;
        }

        OnAxisInput(_playerID, Axis.RStick, _aimLocation);
    }

    private void UpdateMovement()
    {
        switch (_currentGameState)
        {
            case Game.eGameState.Play:
                if (_hasTarget)
                {
                    float distToTarget = _targetX - transform.position.x;
                    float absoluteDistToTarget = Mathf.Abs(distToTarget);

                    if (absoluteDistToTarget > 0.05f)
                    {
                        float direction = Mathf.Sign(distToTarget);
                        float speedModifier = Mathf.Clamp(absoluteDistToTarget, 0f, 1f);

                        OnAxisInput(_playerID, Axis.LStick, new Vector2(direction * speedModifier, 0f));

                        // jump at the ball if you're closer to it than the target
                        float absoluteDistToBall = Mathf.Abs(_currentBallPos.x - transform.position.x);
                        if (absoluteDistToBall < absoluteDistToTarget && transform.position.x < _targetX && !_isJumping)
                        {
                            Debug.Log("AI JUMP");
                            OnButtonPress(_playerID, Button.RBumper); // jump. set up a map for common buttons across players
                            _isJumping = true;
                            _jumpTime = Time.time;
                        }

                        if (_isJumping)
                        {
                            OnButtonHeld(_playerID, Button.RBumper, (Time.time - _jumpTime));

                            if(_jumpTime > 1f) // set up a map for common button/action variables
                            {
                                Debug.Log("AI STOP JUMP");
                                _isJumping = false;
                                _jumpTime = 0f;
                                OnButtonRelease(_playerID, Button.RBumper);
                            }
                        }
                    }
                    else
                    {
                        _hasTarget = false;
                        _targetX = transform.position.x;
                    }
                }

                // move back to center if you're too close to the edges
                // TODO this will later depend on positions of the player (front court/back court)
                else if (transform.position.x < LevelManager.Instance.GetNetPosition().x + 1f || transform.position.x > LevelManager.Instance.GetOOBXForSide(Game.eTeam.Away) - 1f)
                {
                    _targetX = 3f;
                    _hasTarget = true;

                    // just release it here I guess?
                    OnButtonRelease(_playerID, Button.A);
                }
                break;
        }
    }
}
