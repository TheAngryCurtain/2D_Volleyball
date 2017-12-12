using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Singleton<Settings>
{
    public enum ePlayerMode { Single, Multi }
    public enum eTeamSize { Singles, Doubles }

    public class CostumeData
    {
        public int HeadSpriteIndex;
        public int BodySpriteIndex;
        public bool IsHuman;

        public CostumeData(int hIndex, int bIndex, bool isHuman)
        {
            HeadSpriteIndex = hIndex;
            BodySpriteIndex = bIndex;
            IsHuman = isHuman;
        }
    }

    [System.Serializable]
    public class VenueData
    {
        public Sprite VenueThumbnail;
        public string VenueTitle;
        public string VenueDesc;
    }

    [SerializeField] private Sprite[] _playerHeads;
    [SerializeField] private Sprite[] _playerBodies;

    [SerializeField] private Color[] _playerColors;
    [SerializeField] private Color _defaultPlayerPanelBarColor;
    [SerializeField] private string _defaultPlayerPanelJoinMessage;

    [SerializeField] private VenueData[] _venueInfo;
    public VenueData[] VenueInfo { get { return _venueInfo; } }

    [SerializeField] private Color _aiPlayerBodyColor;
    public Color AiPlayerBodyColor { get { return _aiPlayerBodyColor; } }

    public Sprite[] PlayerHeadSprites { get { return _playerHeads; } }
    public Sprite[] PlayerBodySprites { get { return _playerBodies; } }
    public Color DefaultPlayerPanelBarColor { get { return _defaultPlayerPanelBarColor; } }
    public string DefaultPlayerPanelJoinMessage { get { return _defaultPlayerPanelJoinMessage; } }

    private ePlayerMode _playerMode;
    public ePlayerMode PlayerMode { get { return _playerMode; } }

    private eTeamSize _teamSize;
    public eTeamSize TeamSize { get { return _teamSize; } }

    private CostumeData[] _playerSpriteData = new CostumeData[4];
    public CostumeData[] SpriteData { get { return _playerSpriteData; } }

    public void SetPlayerMode(ePlayerMode mode)
    {
        _playerMode = mode;
    }

    public void SetTeamSize(eTeamSize size)
    {
        _teamSize = size;
    }

    public Color GetPlayerColor(int index)
    {
        return _playerColors[index].linear;
    }

    public void SetPlayerSpriteData(int playerIndex, CostumeData data)
    {
        _playerSpriteData[playerIndex] = data;
    }
}
