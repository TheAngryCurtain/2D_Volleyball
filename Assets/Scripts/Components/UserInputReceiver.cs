using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class UserInputReceiver : BaseInputReceiver
{
    private Vector2 aim = Vector2.zero;

    protected override void OnEnable()
    {
        InputManager.Instance.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);

        base.OnEnable();
    }

    // TODO
    // this needs serious refactoring to remove all traces of XInput (axis, button, etc)
    protected virtual void OnInputUpdate(InputActionEventData data)
    {
        float value = 0f;
        switch (data.actionId)
        {
            case RewiredConsts.Action.Move_Horizontal:
                    OnAxisInput(data.playerId, Axis.LStick, new Vector2(data.GetAxis(), 0f));
                break;

            case RewiredConsts.Action.Aim_Horizontal:
                aim.x = data.GetAxis();
                OnAxisInput(data.playerId, Axis.RStick, aim);
                break;

            case RewiredConsts.Action.Aim_Vertical:
                aim.y = data.GetAxis();
                OnAxisInput(data.playerId, Axis.RStick, aim);
                break;

            case RewiredConsts.Action.Jump:
                if (data.GetButtonDown())
                {
                    OnButtonPress(data.playerId, Button.RBumper);
                }
                else if (data.GetButtonUp())
                {
                    OnButtonRelease(data.playerId, Button.RBumper);
                }
                break;

            case RewiredConsts.Action.Spin_Left:
                value = data.GetAxis();
                OnAxisInput(data.playerId, Axis.LTrigger, new Vector2(value, 0f));
                break;

            case RewiredConsts.Action.Spin_Right:
                value = data.GetAxis();
                OnAxisInput(data.playerId, Axis.RTrigger, new Vector2(value, 0f));
                break;
        }
    }
}
