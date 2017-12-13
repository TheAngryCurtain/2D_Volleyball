using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEvents
{
    public class AIPlayerEvent : VSGameEvent
    {
        public int PlayerID;

        public AIPlayerEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }
}