using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public class PlayerEvent : VSGameEvent
    {
        public int PlayerID;

        public PlayerEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }

    public class BallVolliedEvent : PlayerEvent
    {
        public Vector2 Direction;
        public float HoldTime;
        public float Power;
        public Vector2 Spins;
        public float SpinModifier;

        public BallVolliedEvent(int playerid, Vector2 dir, Vector2 spins, float spinModifier, float holdTime, float power) : base (playerid)
        {
            Direction = dir;
            HoldTime = holdTime;
            Power = power;
            Spins = spins;
            SpinModifier = spinModifier;
        }
    }

    public class BallHeldEvent : PlayerEvent
    {
        public Vector3 CurrentPosition;

        public BallHeldEvent(int playerid, Vector3 pos) : base (playerid)
        {
            CurrentPosition = pos;
        }
    }

    public class BallHoldUpdateEvent : PlayerEvent
    {
        public float CurrentAmount;
        public float MaxAmount;

        public BallHoldUpdateEvent(int playerid, float current, float max) : base(playerid)
        {
            CurrentAmount = current;
            MaxAmount = max;
        }
    }

    public class BallDroppedEvent : PlayerEvent
    {
        public BallDroppedEvent(int playerid) : base(playerid) { }
    }

    public class BallInRangeEvent : PlayerEvent
    {
        public BallInRangeEvent(int playerid) : base(playerid) { }
    }

    public class AimUpdateEvent : PlayerEvent
    {
        public Vector2 Direction;

        public AimUpdateEvent(int playerid, Vector2 direction) : base(playerid)
        {
            Direction = direction;
        }
    }

    public class TouchGroundEvent : VSGameEvent
    {
        public bool Touching;

        public TouchGroundEvent(bool touch)
        {
            Touching = touch;
        }
    }

    public class SpinUpdateEvent : VSGameEvent
    {
        public float SpinDirection;

        public SpinUpdateEvent(float direction)
        {
            SpinDirection = direction;
        }
    }
}
