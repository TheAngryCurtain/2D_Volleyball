using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Transform[] _OOBTransforms;
    [SerializeField] private Transform _netTransform;

    public float GetOOBXForSide(Game.eTeam side)
    {
        return _OOBTransforms[(int)side].position.x;
    }

    public Vector2 GetNetPosition()
    {
        return Utils.PositionXY(_netTransform.position);
    }
}
