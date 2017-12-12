using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager : Singleton<FlowManager>
{
    #region GameStates
    public class GameState
    {
        private bool _changeRequested = false;
        private eScene _sceneRequest;

        public virtual void Enter(FlowManager flowManager) { }
        public virtual GameState Update()
        {
            if (_changeRequested)
            {
                _changeRequested = false;
                switch (_sceneRequest)
                {
                    case eScene.Boot:
                        return new BootState();

                    case eScene.FE:
                        return new FrontEndState();

                    case eScene.Game:
                        return new GamePlayState();

                    default:
                        return this;
                }
            }
            else
            {
                return this;
            }
        }
        public virtual void RequestChange(eScene scene)
        {
            _sceneRequest = scene;
            _changeRequested = true;
        }
        public virtual void Exit() { }
    }

    public class BootState : GameState
    {
        public override void Enter(FlowManager flowManager)
        {
            Debug.Log("Entering BootState");

            flowManager.LoadScene(eScene.Boot);

            // test
            RequestChange(eScene.FE);
        }

        public override void Exit()
        {
            Debug.Log("Exiting BootState");
        }
    }

    public class FrontEndState : GameState
    {
        public override void Enter(FlowManager flowManager)
        {
            Debug.Log("Entering FrontEndState");

            flowManager.LoadScene(eScene.FE);
        }

        public override void Exit()
        {
            Debug.Log("Exiting FrontEndState");
        }
    }

    public class GamePlayState : GameState
    {
        public override void Enter(FlowManager flowManager)
        {
            Debug.Log("Entering GameplayState");

            flowManager.LoadScene(eScene.Game);
        }

        public override void Exit()
        {
            Debug.Log("Exiting GameplayState");
        }
    }
    #endregion

    public enum eScene { Boot = 1, FE, Game }; // boostrap is 0

    private GameState _previousState;
    private GameState _currentState;

    private void Start()
    {
        VSEventManager.Instance.AddListener<FlowEvents.OnSceneChangeRequestEvent>(OnSceneChangeRequested);

        _previousState = null;
        _currentState = new BootState();
        //_currentState = new GamePlayState(); // for testing
        //LoadScene(eScene.Boot);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        VSEventManager.Instance.RemoveListener<FlowEvents.OnSceneChangeRequestEvent>(OnSceneChangeRequested);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneChangeRequested(FlowEvents.OnSceneChangeRequestEvent e)
    {
        _currentState.RequestChange(e.Scene);
    }

    private void Update()
    {
        if (_currentState != null && _currentState != _previousState)
        {
            if (_previousState != null)
            {
                _previousState.Exit();
            }
            _currentState.Enter(Instance);
        }

        _previousState = _currentState;
        _currentState = _currentState.Update();
    }

    private void LoadScene(eScene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;
        VSEventManager.Instance.TriggerEvent(new FlowEvents.OnSceneLoadedEvent(sceneIndex));
    }
}
