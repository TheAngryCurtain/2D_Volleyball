using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeter : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;

    [SerializeField] private Color _fillColor;

    private void Awake()
    {
        _sliderFill.color = _fillColor.linear;

        Reset();
    }

    private void OnEnable()
    {
        VSEventManager.Instance.AddListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.AddListener<PlayerEvents.BallHoldUpdateEvent>(OnBallHoldUpdate);
        VSEventManager.Instance.AddListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.AddListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnDisable()
    {
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallHeldEvent>(OnBallHeld);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallHoldUpdateEvent>(OnBallHoldUpdate);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallVolliedEvent>(OnBallVollied);
        VSEventManager.Instance.RemoveListener<PlayerEvents.BallDroppedEvent>(OnBallDropped);
    }

    private void OnBallHeld(PlayerEvents.BallHeldEvent e)
    {
        transform.position = e.CurrentPosition;
    }

    private void OnBallHoldUpdate(PlayerEvents.BallHoldUpdateEvent e)
    {
        _slider.value = e.CurrentAmount / e.MaxAmount;
    }

    private void OnBallVollied(PlayerEvents.BallVolliedEvent e)
    {
        Reset();
    }

    private void OnBallDropped(PlayerEvents.BallDroppedEvent e)
    {
        Reset();
    }

    private void Reset()
    {
        _slider.value = 0f;

        // move offscreen
        transform.position = new Vector2(500f, 500f);
    }
}
