using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] _selectableObjects;
    protected GameObject[] SelectedObjects { get { return _selectableObjects; } }

    [SerializeField] private bool _isVertical = true;

    protected int _selectedIndex;

    public virtual void Init()
    {
        _selectedIndex = 0;
        SetSelected(_selectableObjects[_selectedIndex]);
    }

    public virtual void Shutdown()
    {
        //
    }

    public virtual void ProcessAxisInput(int playerIndex, Vector2 axisInput)
    {
        int direction;
        if (_isVertical)
        {
            direction = (axisInput.y < 0 ? 1 : -1);
        }
        else
        {
            direction = (axisInput.x < 0 ? -1 : 1);
        }

        _selectedIndex = Mathf.Clamp(_selectedIndex + direction, 0, _selectableObjects.Length - 1);
        SetSelected(_selectableObjects[_selectedIndex]);
    }

    public virtual void ProcessButtonInput(int playerIndex, GameInput.Button b)
    {
        // override
    }

    protected void SetSelected(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
    }
}
