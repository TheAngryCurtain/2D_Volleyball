using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : UIScreen
{
    public override void ProcessButtonInput(int playerIndex, GameInput.Button b)
    {
        if (b == GameInput.Button.A)
        {
            switch (_selectedIndex)
            {
                case 0:
                    Settings.Instance.SetPlayerMode(Settings.ePlayerMode.Single);
                    break;

                case 1:
                    Settings.Instance.SetPlayerMode(Settings.ePlayerMode.Multi);
                    break;

                case -1:
                default:
                    Debug.LogFormat("Invalid button index for MainMenu Screen");
                    break;
            }

            VSEventManager.Instance.TriggerEvent(new FlowEvents.OnScreenChangeRequestEvent(ScreenManager.eScreen.TeamSize));
        }
    }
}
