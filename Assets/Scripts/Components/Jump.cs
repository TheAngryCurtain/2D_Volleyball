using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour, IInputReceiver
{
    [SerializeField] private GameInput.Button _jumpButton;
    //[SerializeField] private float _maxHeight;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxJumpTime;

    private Rigidbody2D _rigidbody;
    private bool _canJump = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnButtonPressed(GameInput.Button button)
    {
        if (button == _jumpButton && _canJump)
        {
            //SetVelocityY(_maxHeight);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _canJump = false;
        }
    }

    public void OnButtonHeld(GameInput.Button button, float duration)
    {
        if (button == _jumpButton && duration < _maxJumpTime)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce * 0.5f, ForceMode2D.Force);
        }
    }

    public void OnButtonReleased(GameInput.Button button)
    {
        //don't cancel the jump if falling
        if (_rigidbody.velocity.y > 0f)
        {
            SetVelocityY(_rigidbody.velocity.y * 0.5f);
        }
    }

    public void OnAxisInput(GameInput.Axis axis, Vector2 data) { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Court"))
        {
            _canJump = true;

            VSEventManager.Instance.TriggerEvent(new PlayerEvents.TouchGroundEvent(true));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Court"))
        {
            VSEventManager.Instance.TriggerEvent(new PlayerEvents.TouchGroundEvent(false));
        }
    }

    private void SetVelocityY(float value)
    {
        Vector2 vel = _rigidbody.velocity;
        vel.y = value;

        _rigidbody.velocity = vel;
    }

    //private void FixedUpdate()
    //{
    //    CheckJump();
    //    CheckJumpCancel();
    //}

    //private void CheckJump()
    //{
    //    if (_canJump && _requestJump)
    //    {
    //        Vector2 velocity = _rigidbody.velocity;
    //        velocity.y = _maxJumpVelocity;

    //        _rigidbody.velocity = velocity;
    //        _requestJump = false;
    //    }
    //}

    //private void CheckJumpCancel()
    //{
    //    if (_cancelJump)
    //    {
    //        Vector2 velocity = _rigidbody.velocity;
    //        if (velocity.y > _minJumpVelocity)
    //        {
    //            velocity.y = _minJumpVelocity;

    //            _rigidbody.velocity = velocity;
    //        }
    //        _cancelJump = false;
    //    }
    //}
}
