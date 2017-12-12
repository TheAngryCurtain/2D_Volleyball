using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private Text _timeText;
    [SerializeField] private Text[] _scoresText;
    [SerializeField] private Text[] _setsText;

    [SerializeField] private SpriteRenderer[] _serveIndicators;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _serveColor;

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<UIEvents.ScoreEvent>(OnScoreUpdated);
        VSEventManager.Instance.AddListener<UIEvents.SetEvent>(OnSetUpdated);
        VSEventManager.Instance.AddListener<UIEvents.ServingEvent>(OnServeUpdated);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<UIEvents.ScoreEvent>(OnScoreUpdated);
        VSEventManager.Instance.RemoveListener<UIEvents.SetEvent>(OnSetUpdated);
        VSEventManager.Instance.RemoveListener<UIEvents.ServingEvent>(OnServeUpdated);
    }

    private void OnScoreUpdated(UIEvents.ScoreEvent e)
    {
        _scoresText[0].text = e.Scores[0].ToString();
        _scoresText[1].text = e.Scores[1].ToString();
    }

    private void OnSetUpdated(UIEvents.SetEvent e)
    {
        _setsText[0].text = e.Sets[0].ToString();
        _setsText[1].text = e.Sets[1].ToString();
    }

    private void OnServeUpdated(UIEvents.ServingEvent e)
    {
        _serveIndicators[0].color = _defaultColor;
        _serveIndicators[1].color = _defaultColor;

        _serveIndicators[e.Side].color = _serveColor;
    }

    private void Update()
    {
        _timeText.text = string.Format("{0:00}:{1:00}", DateTime.Now.Hour % 12, DateTime.Now.Minute);
    }
}
