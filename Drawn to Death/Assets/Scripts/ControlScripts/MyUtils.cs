using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MyUtils
{
    public static void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic != null)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }

    public static void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = alpha;
            renderer.color = color;
        }
    }

    public static Quaternion LookAt2D(Vector2 origin, Vector2 destination)
    {
        // Calculate the direction vector from origin to destination
        Vector2 direction = destination - origin;

        // Calculate the angle in degrees from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a Quaternion with the calculated angle (2D rotation is around the Z-axis)
        return Quaternion.Euler(0, 0, angle);
    }
}
