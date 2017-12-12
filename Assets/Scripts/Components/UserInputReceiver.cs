using System;
using System.Collections.Generic;
using GameInput;
using UnityEngine;

public class UserInputReceiver : BaseInputReceiver
{
    protected override void OnEnable()
    {
        InputController.Instance.AddAxisInputListener(OnAxisInput);
        InputController.Instance.AddButtonPressInputListener(OnButtonPress);
        InputController.Instance.AddButtonHoldInputListener(OnButtonHeld);
        InputController.Instance.AddButtonReleaseInputListener(OnButtonRelease);

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        InputController.Instance.RemoveAxisInputListener(OnAxisInput);
        InputController.Instance.RemoveButtonPressInputListener(OnButtonPress);
        InputController.Instance.RemoveButtonHoldInputListener(OnButtonHeld);
        InputController.Instance.RemoveButtonReleaseInputListener(OnButtonRelease);

        base.OnDisable();
    }
}
