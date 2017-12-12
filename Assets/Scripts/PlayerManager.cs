using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private List<GameObject> _playerObjects; // have to use GOs because it could be checked from any component

    // TODO
    // keep track of a currently-controlled player index
    // then, along with checking against player ID, check for currently selected player ID (if in single player) for allowing input
    // -> will need a button for switching between active players

    public override void Awake()
    {
        base.Awake();

        _playerObjects = new List<GameObject>();
    }

    public void RegisterPlayer(GameObject p)
    {
        if (!_playerObjects.Contains(p))
        {
            _playerObjects.Add(p);
        }
        else
        {
            Debug.LogErrorFormat("Player Manager already has registration for {0}", p.name);
        }
    }

    public void UnregisterPlayer(GameObject p)
    {
        bool removed = _playerObjects.Remove(p);
        if (!removed)
        {
            Debug.LogErrorFormat("Player Manager failed to remove {0}", p.name);
        }
    }

    public int GetPlayerID(GameObject p)
    {
        int index = _playerObjects.IndexOf(p);
        if (index < 0)
        {
            Debug.LogErrorFormat("Could not get player ID for {0}", p.name);
        }

        return index;
    }

    public int GetTeamID(GameObject p)
    {
        return GetTeamID(GetPlayerID(p));
    }

    public int GetTeamID(int playerID)
    {
        // TODO change this later when real teams are set up
        if (playerID % 2 == 0)
        {
            return (int)Game.eTeam.Home;
        }

        return (int)Game.eTeam.Away;
    }
}
