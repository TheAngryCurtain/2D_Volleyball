using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectScreen : UIScreen
{
    [SerializeField] private PlayerSelectPanel[] _panels; 
    [SerializeField] private GameObject _readyPrompt;

    private int _headIndex = 0;
    private int _bodyIndex = 0;
    private int _numPlayers = 0;
    private int _playersConfirmed = 0;
    private bool _allConfirmed = false;

    public override void Init()
    {
        for (int i = 0; i < _panels.Length; i++)
        {
            ShutdownPlayerPanel(i);
        }

        _readyPrompt.SetActive(false);

        // player 1 should always be there
        InitPlayerPanel(0);
        _numPlayers = 1;

        int possibleOtherPlayers = 3;
        int aiOtherPlayers = 3;
        if (Settings.Instance.PlayerMode == Settings.ePlayerMode.Single)
        {
            // no other human players
            aiOtherPlayers = 1;

            if (Settings.Instance.TeamSize == Settings.eTeamSize.Singles)
            {
                // only one ai player
                possibleOtherPlayers = 1;
            }
        }
        else
        {
            // potentially 3 other humans, so default no Ai players until other have confirmed
            possibleOtherPlayers = 0;
        }

        for (int i = 1; i <= possibleOtherPlayers; i++)
        {
            _panels[i].InitSide(i, aiOtherPlayers <= i);
        }

        _numPlayers += possibleOtherPlayers;
        _playersConfirmed += possibleOtherPlayers;
    }

    private void InitPlayerPanel(int index)
    {
        _panels[index].Init(index);
    }

    private void ShutdownPlayerPanel(int index)
    {
        _panels[index].Clear();
    }

    public override void ProcessAxisInput(int playerIndex, Vector2 axisInput)
    {
        if (_panels[playerIndex].IsActive)
        {
            _panels[playerIndex].Navigate(axisInput);
        }
    }

    public override void ProcessButtonInput(int playerIndex, Button b)
    {
        switch (b)
        {
            case Button.Start:
                if (_allConfirmed)
                {
                    bool isMultiplayer = (Settings.Instance.PlayerMode == Settings.ePlayerMode.Multi);
                    for (int i = 0; i < _numPlayers; i++)
                    {
                        if (isMultiplayer)
                        {
                            // if everyone is confirmed and you advance, generate AI players to fill the teams
                            if (!_panels[i].IsActive)
                            {
                                _panels[i].InitSide(i, true);
                            }
                        }

                        Settings.Instance.SetPlayerSpriteData(i, _panels[i].GenerateSpriteData());
                    }

                    // test to back end
                    VSEventManager.Instance.TriggerEvent(new FlowEvents.OnSceneChangeRequestEvent(FlowManager.eScene.Game));
                }
                else
                {
                    HandlePlayerEnter(playerIndex);
                }
                break;

            case Button.A:
                HandlePlayerConfirm(playerIndex);
                break;

            case Button.B:
                HandlePlayerExit(playerIndex);
                break;
        }
    }

    private void HandlePlayerEnter(int index)
    {
        if (!_panels[index].IsActive)
        {
            _numPlayers += 1;
            _panels[index].Init(index);
        }
    }

    private void HandlePlayerExit(int index)
    {
        if (_panels[index].IsActive)
        {
            if (_panels[index].HasConfirmed)
            {
                if (_allConfirmed)
                {
                    _allConfirmed = false;
                    _readyPrompt.SetActive(false);
                }

                _playersConfirmed -= 1;
                _panels[index].SetConfirm(false);
            }
            else
            {
                // player 1 can't back out
                if (index > 0)
                {
                    _numPlayers -= 1;
                    _panels[index].Clear();
                }
            }
        }
    }

    private void HandlePlayerConfirm(int index)
    {
        if (_panels[index].IsActive)
        {
            _playersConfirmed += 1;
            _panels[index].SetConfirm(true);
        }

        // check if everyone is ready
        if (_playersConfirmed == _numPlayers)
        {
            _allConfirmed = true;
            _readyPrompt.SetActive(true);
        }
    }
}
