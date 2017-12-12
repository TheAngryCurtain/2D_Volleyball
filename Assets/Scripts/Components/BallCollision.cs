using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO
        // change this to a switch statement so that it looks cleaner

        Vector2 touchPosition = Utils.PositionXY(transform.position);
        Vector2 velocity = this.GetComponent<Rigidbody2D>().velocity;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Court"))
        {
            VSEventManager.Instance.TriggerEvent(new GameEvents.BallTouchFloorEvent(touchPosition, velocity));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Net"))
        {
            VSEventManager.Instance.TriggerEvent(new GameEvents.BallTouchNetEvent(touchPosition, velocity));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            VSEventManager.Instance.TriggerEvent(new GameEvents.BallTouchPlayerEvent(p, touchPosition, velocity));
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Net"))
        {
            VSEventManager.Instance.TriggerEvent(new GameEvents.BallUnderNetEvent());
        }
    }
}
