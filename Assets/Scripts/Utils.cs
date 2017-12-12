using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector2 PositionXY(Vector3 position)
    {
        return new Vector2(position.x, position.y);
    }

    public static void DebugPlaceObject(PrimitiveType type, Vector2 position, float scale = 0.25f)
    {
        GameObject go = GameObject.CreatePrimitive(type);
        go.transform.localScale = new Vector2(scale, scale);
        go.transform.position = position;
    }

    public static Quaternion LookAt2D(Vector3 direction)
    {
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public static Vector3 FindTrajectoryEndPostion(Vector3 initialPosition, Vector3 initialVelocity, Color color, bool debug = false)
    {
        List<Vector3> positions = new List<Vector3>();
        
        float timeDelta = Time.deltaTime;
        Vector3 gravity = Physics.gravity;

        bool positionFound = false;
        int count = 0;

        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        while (!positionFound)
        {
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;
            count += 1;

            if (debug)
            {
                positions.Add(position);
            }

            // this assumes the ground is at 0!
            if (position.y < 0f)
            {
                positionFound = true;
            }
        }

        if (debug)
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                Debug.DrawLine(positions[i], positions[i + 1], color, 10f);
            }
        }

        return position;
    }
}
