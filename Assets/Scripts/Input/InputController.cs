using UnityEngine;
using System;
using System.Collections;
using XInputDotNetPure;

namespace GameInput
{
    public enum Button
    {
        A, B, X, Y, Back, Start, LBumper, RBumper, Guide, LStick, RStick, DPadL, DPadUp, DPadR, DPadDown
    }

    public enum Axis
    {
        LStick, RStick, LTrigger, RTrigger
    }

    public class InputController : Singleton<InputController>
    {
        public delegate void OnAxisEventDelegate(int playerIndex, Axis axis, Vector2 data);
        private static OnAxisEventDelegate AxisEventDelegate;

        public delegate void OnButtonPressEventDelegate(int playerIndex, Button button);
        private static OnButtonPressEventDelegate ButtonPressDelegate;

        public delegate void OnButtonHoldEventDelegate(int playerIndex, Button button, float duration);
        private static OnButtonHoldEventDelegate ButtonHoldDelegate;

        public delegate void OnButtonReleaseEventDelegate(int playerIndex, Button button);
        private static OnButtonReleaseEventDelegate ButtonReleaseDelegate;

        public class Controller
        {
            public PlayerIndex PlayerIndex;
            public GamePadState CurrentState;
            public GamePadState PreviousState;

            public float HoldStartTime = 0f;
            public bool Enabled = true;

            public Controller(PlayerIndex index)
            {
                PlayerIndex = index;
            }
        }

        /// <summary>
        /// Controller connection update callback.
        /// @Params index of the controller, true if controller was connected
        /// </summary>
        public System.Action<int, bool> OnControllerConnectionChanged;

        private int MaxPlayers = 4;
        public int MaxNumberOfPlayers { get { return MaxPlayers; } }

        private Controller[] _controllers;
        private Controller _currentController;

        private Vector2 _axisData = Vector2.zero;

        public override void Awake()
        {
            base.Awake();

            _controllers = new Controller[MaxPlayers];
            for (int i = 0; i < MaxPlayers; ++i)
            {
                _controllers[i] = new Controller((PlayerIndex)i);
            }
        }

        private void Update()
        {
            for (int i = 0; i < MaxPlayers; ++i)
            {
                _currentController = _controllers[i];

                // check for controller state changes
                UpdateControllerStates(_currentController);

                // current controller is not connected or is disabled, skip it
                if (!_currentController.CurrentState.IsConnected || !_currentController.Enabled) continue;

                // poll for input
                ProcessInput(_currentController);
            }
        }

        // enable a given players controller
        public void EnablePlayerInput(int playerIndex, bool enable)
        {
            _controllers[playerIndex].Enabled = enable;
        }

        // vibrate a given controller with amounts for left and right trigger motors
        public void TriggerHapticPulse(int playerIndex, float duration, float left, float right)
        {
            GamePad.SetVibration((PlayerIndex)playerIndex, left, right);

            StartCoroutine(HapticPulse(playerIndex, duration));
        }

        // stop the pulse after t
        private IEnumerator HapticPulse(int index, float t)
        {
            yield return new WaitForSeconds(t);

            GamePad.SetVibration((PlayerIndex)index, 0f, 0f);
        }

        private void UpdateControllerStates(Controller controller)
        {
            controller.PreviousState = controller.CurrentState;
            controller.CurrentState = GamePad.GetState(controller.PlayerIndex);

            if (!controller.PreviousState.IsConnected && controller.CurrentState.IsConnected)
            {
                // new controller connected
                Debug.LogFormat("Controller {0} Connected", controller.PlayerIndex.ToString());
                if (OnControllerConnectionChanged != null)
                {
                    OnControllerConnectionChanged((int)controller.PlayerIndex, true);
                }
            }
            else if (controller.PreviousState.IsConnected && !controller.CurrentState.IsConnected)
            {
                // controller disconnected
                Debug.LogFormat("Controller {0} Disconnected", controller.PlayerIndex.ToString());
                if (OnControllerConnectionChanged != null)
                {
                    OnControllerConnectionChanged((int)controller.PlayerIndex, false);
                }
            }
        }

        private void ProcessInput(Controller controller)
        {
            UpdateButton(Button.A, controller);
            UpdateButton(Button.B, controller);
            UpdateButton(Button.X, controller);
            UpdateButton(Button.Y, controller);
            UpdateButton(Button.Back, controller);
            UpdateButton(Button.Start, controller);
            UpdateButton(Button.Guide, controller);
            UpdateButton(Button.LBumper, controller);
            UpdateButton(Button.RBumper, controller);
            UpdateButton(Button.LStick, controller);
            UpdateButton(Button.RStick, controller);

            UpdateButton(Button.DPadL, controller);
            UpdateButton(Button.DPadUp, controller);
            UpdateButton(Button.DPadR, controller);
            UpdateButton(Button.DPadDown, controller);

            UpdateAxis(Axis.LStick, controller);
            UpdateAxis(Axis.RStick, controller);
            UpdateAxis(Axis.LTrigger, controller);
            UpdateAxis(Axis.RTrigger, controller);
        }

        private void UpdateAxis(Axis a, Controller pc)
        {
            switch(a)
            {
                case Axis.LStick:
                    _axisData.x = pc.CurrentState.ThumbSticks.Left.X;
                    _axisData.y = pc.CurrentState.ThumbSticks.Left.Y;
                    break;

                case Axis.RStick:
                    _axisData.x = pc.CurrentState.ThumbSticks.Right.X;
                    _axisData.y = pc.CurrentState.ThumbSticks.Right.Y;
                    break;

                case Axis.RTrigger:
                    _axisData.x = pc.CurrentState.Triggers.Right;
                    _axisData.y = 0f;
                    break;

                case Axis.LTrigger:
                    _axisData.x = pc.CurrentState.Triggers.Left;
                    _axisData.y = 0f;
                    break;
            }

            InvokeAxisDelegate((int)pc.PlayerIndex, a, _axisData);
            //_currentControllable.HandleAxis(a, hValue, vValue);
        }

        private void UpdateButton(Button b, Controller pc)
        {
            ButtonState prev;
            ButtonState current;
            switch (b)
            {
                case Button.A:
                    prev = pc.PreviousState.Buttons.A;
                    current = pc.CurrentState.Buttons.A;
                    break;

                case Button.B:
                    prev = pc.PreviousState.Buttons.B;
                    current = pc.CurrentState.Buttons.B;
                    break;

                case Button.X:
                    prev = pc.PreviousState.Buttons.X;
                    current = pc.CurrentState.Buttons.X;
                    break;

                case Button.Y:
                    prev = pc.PreviousState.Buttons.Y;
                    current = pc.CurrentState.Buttons.Y;
                    break;

                case Button.Back:
                    prev = pc.PreviousState.Buttons.Back;
                    current = pc.CurrentState.Buttons.Back;
                    break;

                default:
                case Button.Start:
                    prev = pc.PreviousState.Buttons.Start;
                    current = pc.CurrentState.Buttons.Start;
                    break;

                case Button.Guide:
                    prev = pc.PreviousState.Buttons.Guide;
                    current = pc.CurrentState.Buttons.Guide;
                    break;

                case Button.LBumper:
                    prev = pc.PreviousState.Buttons.LeftShoulder;
                    current = pc.CurrentState.Buttons.LeftShoulder;
                    break;

                case Button.RBumper:
                    prev = pc.PreviousState.Buttons.RightShoulder;
                    current = pc.CurrentState.Buttons.RightShoulder;
                    break;

                case Button.LStick:
                    prev = pc.PreviousState.Buttons.LeftStick;
                    current = pc.CurrentState.Buttons.LeftStick;
                    break;

                case Button.RStick:
                    prev = pc.PreviousState.Buttons.RightStick;
                    current = pc.CurrentState.Buttons.RightStick;
                    break;

                case Button.DPadL:
                    prev = pc.PreviousState.DPad.Left;
                    current = pc.CurrentState.DPad.Left;
                    break;

                case Button.DPadUp:
                    prev = pc.PreviousState.DPad.Up;
                    current = pc.CurrentState.DPad.Up;
                    break;

                case Button.DPadR:
                    prev = pc.PreviousState.DPad.Right;
                    current = pc.CurrentState.DPad.Right;
                    break;

                case Button.DPadDown:
                    prev = pc.PreviousState.DPad.Down;
                    current = pc.CurrentState.DPad.Down;
                    break;

            }

            if (CheckButtonPressed(prev, current))
            {
                //_currentControllable.HandleButtonPress(b);
                InvokeButtonPressDelegate((int)pc.PlayerIndex, b);
                pc.HoldStartTime = Time.time;
            }
            else if (CheckButtonReleased(prev, current))
            {
                //_currentControllable.HandleButtonRelease(b);
                InvokeButtonReleaseDelegate((int)pc.PlayerIndex, b);
                pc.HoldStartTime = 0f;
            }
            else if (CheckButtonHeld(prev, current))
            {
                // _currentControllable.HandleButtonHeld(b, (Time.time - pc.HoldStartTime));
                InvokeButtonHoldDelegate((int)pc.PlayerIndex, b, (Time.time - pc.HoldStartTime));
            }
        }

        private bool CheckButtonPressed(ButtonState prev, ButtonState current)
        {
            return prev == ButtonState.Released && current == ButtonState.Pressed;
        }

        private bool CheckButtonReleased(ButtonState prev, ButtonState current)
        {
            return prev == ButtonState.Pressed && current == ButtonState.Released;
        }

        private bool CheckButtonHeld(ButtonState prev, ButtonState current)
        {
            return prev == ButtonState.Pressed && current == ButtonState.Pressed;
        }

        private void InvokeAxisDelegate(int playerIndex, Axis axis, Vector2 axisInput)
        {
            if (AxisEventDelegate != null)
            {
                AxisEventDelegate(playerIndex, axis, axisInput);
            }
        }

        private void InvokeButtonPressDelegate(int playerIndex, Button b)
        {
            if (ButtonPressDelegate != null)
            {
                ButtonPressDelegate(playerIndex, b);
            }
        }

        private void InvokeButtonHoldDelegate(int playerIndex, Button b, float duration)
        {
            if (ButtonHoldDelegate != null)
            {
                ButtonHoldDelegate(playerIndex, b, duration);
            }
        }

        private void InvokeButtonReleaseDelegate(int playerIndex, Button b)
        {
            if (ButtonReleaseDelegate != null)
            {
                ButtonReleaseDelegate(playerIndex, b);
            }
        }

        public void AddAxisInputListener(OnAxisEventDelegate listener)
        {
            if (listener != null)
            {
                RemoveAxisInputListener(listener);
                AxisEventDelegate += listener;
            }
        }

        public void AddButtonPressInputListener(OnButtonPressEventDelegate listener)
        {
            if (listener != null)
            {
                RemoveButtonPressInputListener(listener);
                ButtonPressDelegate += listener;
            }
        }

        public void AddButtonHoldInputListener(OnButtonHoldEventDelegate listener)
        {
            if (listener != null)
            {
                RemoveButtonHoldInputListener(listener);
                ButtonHoldDelegate += listener;
            }
        }

        public void AddButtonReleaseInputListener(OnButtonReleaseEventDelegate listener)
        {
            if (listener != null)
            {
                RemoveButtonReleaseInputListener(listener);
                ButtonReleaseDelegate += listener;
            }
        }

        public void RemoveAxisInputListener(OnAxisEventDelegate listener)
        {
            AxisEventDelegate -= listener;
        }

        public void RemoveButtonPressInputListener(OnButtonPressEventDelegate listener)
        {
            ButtonPressDelegate -= listener;
        }

        public void RemoveButtonHoldInputListener(OnButtonHoldEventDelegate listener)
        {
            ButtonHoldDelegate -= listener;
        }

        public void RemoveButtonReleaseInputListener(OnButtonReleaseEventDelegate listener)
        {
            ButtonReleaseDelegate -= listener;
        }
    }
}
