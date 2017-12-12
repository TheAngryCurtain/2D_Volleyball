using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    void OnAxisInput(Axis a, Vector2 data);
    void OnButtonPressed(Button b);
    void OnButtonHeld(Button b, float duration);
    void OnButtonReleased(Button b);
}
