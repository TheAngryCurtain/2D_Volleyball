using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectPanel : MonoBehaviour
{
    public enum eSpriteSlot { Head, Body };

    [SerializeField] private Image _colorBar;
    [SerializeField] private Text _nameLabel;
    [SerializeField] private GameObject _playerContainer;
    [SerializeField] private GameObject _arrowObject;
    [SerializeField] private GameObject _confirmContainer;
    [SerializeField] private GameObject _readyContainer;
    [SerializeField] private Image _headImage;
    [SerializeField] private Image _bodyImage;

    private int _panelIndex;
    private int _headSpriteIndex;
    private int _bodySpriteIndex;
    private eSpriteSlot _activeSlot;
    private bool _isAi;

    private bool _confirmed = false;
    public bool HasConfirmed { get { return _confirmed; } }

    private bool _active = false;
    public bool IsActive { get { return _active; } }

    public void Init(int index)
    {
        _panelIndex = index;
        ActivatePanel(true, index);

        _headSpriteIndex = 0;
        _bodySpriteIndex = 0;

        SetArrowPositionFromSlot(eSpriteSlot.Head);

        UpdateHeadSprite();
        UpdateBodySprite();
    }

    public void InitSide(int index, bool isAi)
    {
        if (index % 2 != 0)
        {
            // make right side (away team) face the middle
            Vector3 currentScale = _playerContainer.transform.localScale;
            currentScale.x *= -1f;
            _playerContainer.transform.localScale = currentScale;
        }

        _isAi = isAi;
        if (isAi)
        {
            SetAI(index);
        }
    }

    private void SetAI(int index)
    {
        Init(index);

        // randomize head and body for ai
        _headSpriteIndex = UnityEngine.Random.Range(0, Settings.Instance.PlayerHeadSprites.Length - 1);
        UpdateHeadSprite();

        _bodySpriteIndex = UnityEngine.Random.Range(0, Settings.Instance.PlayerBodySprites.Length - 1);
        UpdateBodySprite();

        _headImage.color = Settings.Instance.AiPlayerBodyColor;
        _bodyImage.color = Settings.Instance.AiPlayerBodyColor;

        _colorBar.color = Settings.Instance.DefaultPlayerPanelBarColor;

        // auto confirm them
        SetConfirm(true);
    }

    public void Navigate(Vector2 axisData)
    {
        // vertical
        if (axisData.y < 0f)
        {
            SetArrowPositionFromSlot(eSpriteSlot.Body);
            return;
        }
        else if (axisData.y > 0f)
        {
            SetArrowPositionFromSlot(eSpriteSlot.Head);
            return;
        }

        // horizontal
        int horDir = (axisData.x < 0f ? -1 : 1);
        if (_activeSlot == eSpriteSlot.Head)
        {
            _headSpriteIndex = (_headSpriteIndex + horDir) % Settings.Instance.PlayerHeadSprites.Length;
            if (_headSpriteIndex < 0)
            {
                _headSpriteIndex = Settings.Instance.PlayerHeadSprites.Length - 1;
            }

            UpdateHeadSprite();
        }
        else if (_activeSlot == eSpriteSlot.Body)
        {
            _bodySpriteIndex = (_bodySpriteIndex + horDir) % Settings.Instance.PlayerBodySprites.Length;
            if (_bodySpriteIndex < 0)
            {
                _bodySpriteIndex = Settings.Instance.PlayerBodySprites.Length - 1;
            }

            UpdateBodySprite();
        }
    }

    public void Clear()
    {
        ActivatePanel(false, -1);
    }

    public void SetConfirm(bool confirm)
    {
        _confirmed = confirm;
        _arrowObject.SetActive(!confirm);
        _confirmContainer.SetActive(!confirm);
        _readyContainer.SetActive(confirm);
    }

    public Settings.CostumeData GenerateSpriteData()
    {
        return new Settings.CostumeData(_headSpriteIndex, _bodySpriteIndex, !_isAi);
    }

    private void SetArrowPositionFromSlot(eSpriteSlot slot)
    {
        switch (slot)
        {
            case eSpriteSlot.Head:
                _arrowObject.transform.position = _headImage.transform.position;
                break;

            case eSpriteSlot.Body:
                _arrowObject.transform.position = _bodyImage.transform.position;
                break;
        }

        _activeSlot = slot;
    }

    private void UpdateHeadSprite()
    {
        _headImage.sprite = Settings.Instance.PlayerHeadSprites[_headSpriteIndex];
    }

    private void UpdateBodySprite()
    {
        _bodyImage.sprite = Settings.Instance.PlayerBodySprites[_bodySpriteIndex];
    }

    private void ActivatePanel(bool activate, int index)
    {
        if (activate)
        {
            _nameLabel.text = string.Format("Player {0}", index + 1);
            _colorBar.color = Settings.Instance.GetPlayerColor(index);
        }
        else
        {
            _nameLabel.text = Settings.Instance.DefaultPlayerPanelJoinMessage;
            _colorBar.color = Settings.Instance.DefaultPlayerPanelBarColor;
        }

        _playerContainer.SetActive(activate);
        _confirmContainer.SetActive(activate);
        _arrowObject.SetActive(activate);
        _readyContainer.SetActive(false);
        _active = activate;
    }
}
