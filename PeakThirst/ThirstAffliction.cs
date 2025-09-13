using BepInEx;
using HarmonyLib;
using PEAKLib.Stats;
using PeakThirst;
using System.Collections;
using UnityEngine;

public static class ThirstAffliction
{
    public const string StatusName = "Dehydration";
    public static CharacterAfflictions.STATUSTYPE DehydrationType;
    public static void CreateThirstAffliction()
    {
        var icon = SpriteLoader.FromEmbedded("PeakThirst.Sprites.Droplet.png");
        Status spikyStatus = new Status()
        {
            Name = StatusName,
            Color = new Color(0.075f, 0.286f, 1),
            MaxAmount = 2f,
            AllowClear = true,

            ReductionCooldown = 0f,
            ReductionPerSecond = 0f,

            Icon = icon
        };
        new StatusContent(spikyStatus).Register(Thirst.Definition);
        DehydrationType = spikyStatus.Type;
    }

}