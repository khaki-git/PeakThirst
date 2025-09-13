using UnityEngine;
using System.Collections.Generic;

public static class DehydrationApi
{
    private static readonly Dictionary<string, float> restorations = new Dictionary<string, float>
    {
        { "Apple Berry",   0.10f },   // All Crispberries
        { "Berrynana",     0.20f },   // All Berrynanas
        { "Bugfix",        0.15f },   // Ticks
        { "Clusterberry",  0.35f },   // All Clusterberries
        { "Kingberry",     0.15f },   // All Kingberries
        { "Marshmallow",   0.45f },   // Marshmallows
        { "Mushroom",      0.075f },  // All Mushrooms
        { "Sports Drink",  1.00f },   // Sports Drinks
        { "Winterberry",   0.30f },   // All Winterberries
        { "Honeycomb",     0.15f },   // Honeycombs
        { "Coconut_half",  0.60f },   // Coconut Halves
        { "AloeVera",      1.50f },   // Aloe Vera
        { "Turkey",        0.70f },   // Chickens
        { "Napberry",      1.00f },   // Napberries
        { "Prickleberry",  0.50f },   // All Prickleberries
        { "Cure-All",      0.90f },   // Cure-All Potions
        { "MedicinalRoot", 0.20f },   // Medicinal Roots
        { "Energy Drink",  0.50f }    // Energy Drinks
    };

    /// <summary>
    /// Add or replace a restoration mapping at runtime.
    /// </summary>
    public static void RegisterRestoration(string itemName, float restorationAmount)
    {
        restorations[itemName] = restorationAmount;
    }

    /// <summary>
    /// Calculate thirst restoration for a GameObject by inspecting its name.
    /// Sums all matching keywords from the restoration table.
    /// </summary>
    public static float CalculateThirstRestoration(GameObject item)
    {
        if (item == null)
        {
            Debug.LogWarning("DehydrationApi.CalculateThirstRestoration called with null item.");
            return 0f;
        }

        string itemName = item.name;
        Debug.Log($"[DehydrationApi] Calculating thirst for '{itemName}'");
        return CalculateThirstRestoration(itemName);
    }

    /// <summary>
    /// Calculate thirst restoration for a raw item name by summing all matching keywords.
    /// </summary>
    public static float CalculateThirstRestoration(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
            return 0f;

        float total = 0f;

        foreach (var kvp in restorations)
        {
            if (itemName.Contains(kvp.Key))
                total += kvp.Value;
        }

        return total;
    }

    /// <summary>
    /// Try to get the exact restoration amount for a specific keyword (no name matching).
    /// </summary>
    public static bool TryGetRestoration(string key, out float restorationAmount)
        => restorations.TryGetValue(key, out restorationAmount);
}
