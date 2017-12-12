using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public readonly int MAX_POINTS_PER_SET = 21;
    public readonly int MAX_POINTS_FINAL_SET = 15;

    public enum eTeam { Home, Away };
    public enum eGameState { Free, Serve, Play, Over };

    private int _currentMaxScore;

    private int _maxSets;
    private int _bestOfSets;

    private int[] _scores;
    private int[] _sets;

    private float _timeRemainingSeconds;
    private eGameState _currentState;
    private int _servingTeamIndex;
    private bool _ballUnderNet = false;

    public Game(int numSets, int maxScore = 25)
    {
        // TODO
        // score info (points, sets) (FIND OUT HOW THIS WORKS)
        // remaining time (FIND OUT HOW VOLLEYBALL TIMING WORKS)
        // serving team (FIND OUT HOW THIS WORKS)

        _maxSets = numSets;
        _scores = new int[2] { 0, 0 };
        _sets = new int[2] { 0, 0 };

        _currentMaxScore = maxScore;
        _bestOfSets = Mathf.CeilToInt(_maxSets / 2f);

        _servingTeamIndex = (int)eTeam.Home;

        SetState(eGameState.Serve);

        VSEventManager.Instance.AddListener<GameEvents.BallTouchFloorEvent>(OnBallTouchFloor);
        VSEventManager.Instance.AddListener<PlayerEvents.BallVolliedEvent>(OnBallLaunched);
        VSEventManager.Instance.AddListener<GameEvents.BallUnderNetEvent>(OnBallUnderNet);
        
        // UI
        VSEventManager.Instance.TriggerEvent(new UIEvents.ScoreEvent(_scores));
        VSEventManager.Instance.TriggerEvent(new UIEvents.SetEvent(_sets));
    }

    private void SetState(eGameState state)
    {
        _currentState = state;

        int[] paramData = null;
        if (_currentState == eGameState.Serve)
        {
            paramData = new int[] { _servingTeamIndex };
            VSEventManager.Instance.TriggerEvent(new UIEvents.ServingEvent(_servingTeamIndex));
        }

        VSEventManager.Instance.TriggerEvent(new GameEvents.GameStateChangedEvent(_currentState, paramData));
    }

    private void OnBallUnderNet(GameEvents.BallUnderNetEvent e)
    {
        if (_currentState == eGameState.Play)
        {
            _ballUnderNet = true;
        }
    }

    private void OnBallTouchFloor(GameEvents.BallTouchFloorEvent e)
    {
        if (_currentState == eGameState.Play)
        {
            float hitX = e.ContactPoint.x;
            bool isOOB = (Mathf.Abs(hitX) > LevelManager.Instance.GetOOBXForSide(eTeam.Away)); // we only use away here because it's positive

            eTeam awardedSide = (hitX < LevelManager.Instance.GetNetPosition().x ? eTeam.Away : eTeam.Home);
            if (isOOB || _ballUnderNet)
            {
                awardedSide = (awardedSide == eTeam.Home ? eTeam.Away : eTeam.Home);
            }

            bool isGameOver = UpdateScore(awardedSide);
            if (isGameOver)
            {
                SetState(eGameState.Over);
            }
            else
            {
                _servingTeamIndex = (int)awardedSide;
                SetState(eGameState.Serve);
            }

            _ballUnderNet = false;
        }
    }

    private bool UpdateScore(eTeam scoringSide)
    {
        bool gameOver = false;

        int side = (int)scoringSide;
        int otherSide = (scoringSide == eTeam.Home ? (int)eTeam.Away : (int)eTeam.Home);
        _scores[side] += 1;

        if (_scores[side] == _currentMaxScore)
        {
            // if the teams are only a point apart when one team reaches the max, they need to win by 2
            if (_scores[side] - _scores[otherSide] == 1)
            {
                _currentMaxScore += 1;
            }
            else
            {
                _sets[side] += 1;

                // reset scores
                _scores[side] = 0;
                _scores[otherSide] = 0;

                if (_sets[side] == _bestOfSets)
                {
                    gameOver = true;
                }

            }
        }

        // UI
        VSEventManager.Instance.TriggerEvent(new UIEvents.ScoreEvent(_scores));
        VSEventManager.Instance.TriggerEvent(new UIEvents.SetEvent(_sets));

        return gameOver;
    }

    private void OnBallLaunched(PlayerEvents.BallVolliedEvent e)
    {
        if (_currentState == eGameState.Serve)
        {
            SetState(eGameState.Play);
        }
    }
}
