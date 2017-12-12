using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    public enum eScreen { MainMenu, TeamSize, PlayerSelect, /*MatchSetup*/ };

    [SerializeField] private UIScreen[] _screens;

    private UIScreen _currentScreen;
    private bool _inputProcessed = false;

    private bool _isPaused = false;

    public override void Awake()
    {
        base.Awake();

        GameInput.InputController.Instance.AddAxisInputListener(OnAxisInput);
        GameInput.InputController.Instance.AddButtonPressInputListener(OnButtonPress);

        VSEventManager.Instance.AddListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
        VSEventManager.Instance.AddListener<FlowEvents.OnScreenChangeRequestEvent>(OnScreenChangeRequested);

        LoadScreen(eScreen.MainMenu);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        VSEventManager.Instance.RemoveListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
        VSEventManager.Instance.RemoveListener<FlowEvents.OnScreenChangeRequestEvent>(OnScreenChangeRequested);
    }

    private void OnAxisInput(int playerIndex, GameInput.Axis axis, Vector2 data)
    {
        if (axis == GameInput.Axis.LStick)
        {
            if (data != Vector2.zero)
            {
                if (_currentScreen != null && !_inputProcessed)
                {
                    _currentScreen.ProcessAxisInput(playerIndex, data);
                    _inputProcessed = true;
                }
            }
            else
            {
                _inputProcessed = false;
            }
        }
    }

    private void OnButtonPress(int playerIndex, GameInput.Button button)
    {
        if (_currentScreen != null)
        {
            _currentScreen.ProcessButtonInput(playerIndex, button);
        }
    }

    private void OnSceneLoaded(FlowEvents.OnSceneLoadedEvent e)
    {
        // TODO? is this needed?
    }

    private void OnScreenChangeRequested(FlowEvents.OnScreenChangeRequestEvent e)
    {
        LoadScreen(e.Screen);
    }

    private void LoadScreen(eScreen screen)
    {
        if (_currentScreen != null)
        {
            _currentScreen.Shutdown();
        }

        for (int i = 0; i < _screens.Length; i++)
        {
            ShowScreen(_screens[i], false);
        }

        _currentScreen = _screens[(int)screen];
        ShowScreen(_currentScreen, true);

        _currentScreen.Init();
    }

    private void ShowScreen(UIScreen screen, bool show)
    {
        screen.gameObject.SetActive(show);
    }
}
