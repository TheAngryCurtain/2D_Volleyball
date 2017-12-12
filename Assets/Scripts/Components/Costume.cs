using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Costume : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _headImage;
    [SerializeField] private SpriteRenderer _bodyImage;

    // TODO
    // could probably replace this with a PlayerSpawned event that listened and set this when it was ready
    public void Set(Settings.CostumeData data)
    {
        _headImage.sprite = Settings.Instance.PlayerHeadSprites[data.HeadSpriteIndex];
        _bodyImage.sprite = Settings.Instance.PlayerBodySprites[data.BodySpriteIndex];

        if (!data.IsHuman)
        {
            _headImage.color = Settings.Instance.AiPlayerBodyColor;
            _bodyImage.color = Settings.Instance.AiPlayerBodyColor;
        }
    }
}
