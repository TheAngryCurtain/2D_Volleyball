using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public enum Button
{
    A, B, X, Y, Back, Start, LBumper, RBumper, Guide, LStick, RStick, DPadL, DPadUp, DPadR, DPadDown
}

public enum Axis
{
    LStick, RStick, LTrigger, RTrigger
}

public class ScreenManager : Singleton<ScreenManager>
{
    public enum eScreen { MainMenu, TeamSize, PlayerSelect, /*MatchSetup*/ };

    [SerializeField] private UIScreen[] _screens;

    private UIScreen _currentScreen;
    private float _inputDelayTime = 0.25f;
    private float _currentTime = 0f;
    private const float _deadzone = 0.25f;

    private bool _isPaused = false;

    public override void Awake()
    {
        base.Awake();

        //GameInput.InputController.Instance.AddAxisInputListener(OnAxisInput);
        //GameInput.InputController.Instance.AddButtonPressInputListener(OnButtonPress);

        InputManager.Instance.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);

        VSEventManager.Instance.AddListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
        VSEventManager.Instance.AddListener<FlowEvents.OnScreenChangeRequestEvent>(OnScreenChangeRequested);

        LoadScreen(eScreen.MainMenu);
    }

    public override void OnDestroy()
    {
        InputManager.Instance.RemoveInputEventDelegate(OnInputUpdate);

        base.OnDestroy();

        VSEventManager.Instance.RemoveListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
        VSEventManager.Instance.RemoveListener<FlowEvents.OnScreenChangeRequestEvent>(OnScreenChangeRequested);
    }

    protected virtual void OnInputUpdate(InputActionEventData data)
    {
        float value = 0f;
        Vector2 axis = Vector2.zero;
        Button b = Button.Guide; // not used
        switch (data.actionId)
        {
            case RewiredConsts.Action.Nav_Horizontal:
                value = data.GetAxis();
                if (Mathf.Abs(value) > _deadzone)
                {
                    axis.x = data.GetAxis();
                }
                break;

            case RewiredConsts.Action.Nav_Vertical:
                value = data.GetAxis();
                if (Mathf.Abs(value) > _deadzone)
                {
                    axis.y = data.GetAxis();
                }
                break;

            case RewiredConsts.Action.Select:
                if (data.GetButtonDown())
                {
                    b = Button.A;
                }
                break;

            case RewiredConsts.Action.Cancel:
                if (data.GetButtonDown())
                {
                    b = Button.B;
                }
                break;

            case RewiredConsts.Action.Confirm:
                if (data.GetButtonDown())
                {
                    b = Button.Start;
                }
                break;
        }

        if (axis != Vector2.zero)
        {
            if (_currentScreen != null && Time.time > _currentTime + _inputDelayTime)
            {
                _currentScreen.ProcessAxisInput(data.playerId, axis);
                _currentTime = Time.time;
            }
        }

        if (b != Button.Guide && _currentScreen != null)
        {
            _currentScreen.ProcessButtonInput(data.playerId, b);
        }
    }

    //private void OnAxisInput(int playerIndex, GameInput.Axis axis, Vector2 data)
    //{
    //    if (axis == GameInput.Axis.LStick)
    //    {
    //        if (data != Vector2.zero)
    //        {
    //            if (_currentScreen != null && !_inputProcessed)
    //            {
    //                _currentScreen.ProcessAxisInput(playerIndex, data);
    //                _inputProcessed = true;
    //            }
    //        }
    //        else
    //        {
    //            _inputProcessed = false;
    //        }
    //    }
    //}

    //private void OnButtonPress(int playerIndex, GameInput.Button button)
    //{
    //    if (_currentScreen != null)
    //    {
    //        _currentScreen.ProcessButtonInput(playerIndex, button);
    //    }
    //}

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
