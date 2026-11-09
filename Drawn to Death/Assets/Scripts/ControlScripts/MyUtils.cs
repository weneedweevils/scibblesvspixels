using System;
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

    /// <summary>
    /// Selects a random element from a list based on weighted probabilities.
    /// </summary>
    public static T WeightedRandomChoice<T>(List<T> items) where T : IWeightedOption
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("Item list is null or empty.");

        float totalWeight = 0f;
        foreach (var item in items)
            totalWeight += item.weight;

        if (totalWeight <= 0f)
            throw new ArgumentException("Total weight must be greater than zero.");

        float randomPoint = UnityEngine.Random.value * totalWeight;
        float currentWeight = 0f;

        foreach (var item in items)
        {
            currentWeight += item.weight;
            if (randomPoint <= currentWeight)
                return item;
        }

        return default;
    }

    public interface IWeightedOption
    {
        float weight { get; }
    }
}
