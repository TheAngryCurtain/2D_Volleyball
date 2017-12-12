using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSizeScreen : UIScreen
{
    public override void ProcessButtonInput(int playerIndex, Button b)
    {
        if (b == Button.A)
        {
            switch (_selectedIndex)
            {
                case 0:
                    Settings.Instance.SetTeamSize(Settings.eTeamSize.Singles);
                    break;

                case 1:
                    Settings.Instance.SetTeamSize(Settings.eTeamSize.Doubles);
                    break;

                case -1:
                default:
                    Debug.LogFormat("Invalid button index for Team Size Screen");
                    break;
            }

            VSEventManager.Instance.TriggerEvent(new FlowEvents.OnScreenChangeRequestEvent(ScreenManager.eScreen.PlayerSelect));
        }
    }
}
