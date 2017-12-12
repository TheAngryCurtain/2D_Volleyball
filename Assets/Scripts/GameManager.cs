using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _userPlayerPrefab;
    [SerializeField] private GameObject _aiPlayerPrefab;

    private Game _currentGame;

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
    }

    public void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<FlowEvents.OnSceneLoadedEvent>(OnSceneLoaded);
    }

    private void OnSceneLoaded(FlowEvents.OnSceneLoadedEvent e)
    {
        switch (e.SceneID)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                InitGameScene();
                break;

            default:
                Debug.LogErrorFormat("Invalid scene id");
                break;
        }
    }

    private void InitGameScene()
    {
        // TODO
        // Get match/game setup data from somewhere
        // set up volleyball game (rules, etc)
        // set up teams
        // instantiate players
        // instantiate ball
        // start

        float playerSpawnX = LevelManager.Instance.GetOOBXForSide(Game.eTeam.Away) - 0.5f;
        float posModifier = 1f;

        Settings.CostumeData[] playerData = Settings.Instance.SpriteData;
        for (int i = 0; i < playerData.Length; i++)
        {
            // front/back row position modifier
            // back row gets spawned first, then front row
            if (i > 1)
            {
                posModifier = 0.5f;
            }

            // spawn players
            if (playerData[i] != null)
            {
                // home team on left, away on right
                float teamModifier = (i % 2 == 0 ? -1f : 1f);

                GameObject currentPlayer;
                if (playerData[i].IsHuman)
                {
                    currentPlayer = Instantiate(_userPlayerPrefab, new Vector3(playerSpawnX * posModifier * teamModifier, 0.0575f, 0f), Quaternion.identity);
                }
                else
                {
                    currentPlayer = Instantiate(_aiPlayerPrefab, new Vector3(playerSpawnX * posModifier * teamModifier, 0.0575f, 0f), Quaternion.identity);
                }

                Costume c = currentPlayer.GetComponent<Costume>();
                c.Set(playerData[i]);
            }
        }

        Instantiate(_ballPrefab, new Vector3(-playerSpawnX + 0.85f, 0.95f, 0f), Quaternion.identity);

        int bestOfSets = 1;
        int maxPointsPerSet = 25; // -1 for free play
        _currentGame = new Game(bestOfSets, maxPointsPerSet); // test number of sets
    }
}
