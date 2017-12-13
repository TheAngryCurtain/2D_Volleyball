using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public class GameStateChangedEvent : VSGameEvent
    {
        public Game.eGameState NewState;
        public int[] ParamData;

        public GameStateChangedEvent(Game.eGameState state, int[] data)
        {
            NewState = state;
            ParamData = data;
        }
    }

    public class TrajectoryEndEvent : VSGameEvent
    {
        public Vector2 TrajectoryEndPosition;
        
        public TrajectoryEndEvent(Vector2 endPos)
        {
            TrajectoryEndPosition = endPos;
        }
    }

    public class BallCollisionEvent : VSGameEvent
    {
        public Vector2 RelativeVelocity;
        public Vector2 ContactPoint;

        public BallCollisionEvent(Vector2 point, Vector2 relativeVel)
        {
            ContactPoint = point;
            RelativeVelocity = relativeVel;
        }
    }

    public class BallTouchFloorEvent : BallCollisionEvent
    {
        public BallTouchFloorEvent(Vector2 point, Vector2 relativeVel) : base(point, relativeVel) { }
    }

    public class BallTouchNetEvent : BallCollisionEvent
    {
        public BallTouchNetEvent(Vector2 point, Vector2 relativeVel) : base(point, relativeVel) { }
    }

    public class BallTouchPlayerEvent : BallCollisionEvent
    {
        public Player TouchedPlayer;

        public BallTouchPlayerEvent(Player p, Vector2 point, Vector2 relativeVel) : base(point, relativeVel)
        {
            TouchedPlayer = p;
        }
    }

    public class BallSpawnedEvent : VSGameEvent
    {
        public Transform BallTransform;

        public BallSpawnedEvent(Transform ballTransform)
        {
            BallTransform = ballTransform;
        }
    }

    public class BallPositionUpdateEvent : VSGameEvent
    {
        public Vector2 CurrentPosition;

        public BallPositionUpdateEvent() { }

        //instead of creating a new one in the update each time, just update it and fire it out
        public void Update(Vector2 pos)
        {
            CurrentPosition = pos;
        }
    }

    public class EnableInputEvent : VSGameEvent
    {
        public int ControllerID;
        public bool Enable;

        public EnableInputEvent(int controllerID, bool enable)
        {
            ControllerID = controllerID;
            Enable = enable;
        }
    }

    public class BallUnderNetEvent : VSGameEvent
    {
        public BallUnderNetEvent()
        {

        }
    }

    public class PlayerSpawnedEvent : VSGameEvent
    {
        public Transform PlayerTransform;

        public PlayerSpawnedEvent(Transform player)
        {
            PlayerTransform = player;
        }
    }
}
