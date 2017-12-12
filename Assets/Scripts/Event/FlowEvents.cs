using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowEvents
{
    public class OnSceneLoadedEvent : VSGameEvent
    {
        public int SceneID;

        public OnSceneLoadedEvent(int sceneID)
        {
            SceneID = sceneID;
        }
    }

    public class OnSceneChangeRequestEvent : VSGameEvent
    {
        public FlowManager.eScene Scene;

        public OnSceneChangeRequestEvent(FlowManager.eScene sceen)
        {
            Scene = sceen;
        }
    }

    public class OnScreenChangeRequestEvent : VSGameEvent
    {
        public ScreenManager.eScreen Screen;

        public OnScreenChangeRequestEvent(ScreenManager.eScreen screen)
        {
            Screen = screen;
        }
    }
}
