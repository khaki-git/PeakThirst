using BepInEx;
using HarmonyLib;
using PEAKLib.Core;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeakThirst
{

    public class Action_MergerAction : ItemAction // unused because += exists
    {
        public ItemAction action1;
        public ItemAction action2;

        public override void RunAction()
        {
            action1.RunAction();
            action2.RunAction();
        }
    }

    [BepInPlugin("com.khakixd.thirst", "Thirst", "0.1.0")]
    public class Thirst : BaseUnityPlugin
    {
        public static ModDefinition Definition { get; set; } = null!;
        private Harmony _harmony;

        private void Awake()
        {
            Definition = ModDefinition.GetOrCreate(Info.Metadata);
            Logger.LogInfo("Thirst loaded!");
            _harmony = new Harmony("com.khakixd.thirst");
            ThirstAffliction.CreateThirstAffliction();
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            try
            {
                var patchedCount = Harmony.GetAllPatchedMethods().Count();
                Logger.LogInfo($"Harmony: total patched methods in process = {patchedCount}");
            }
            catch { Logger.LogInfo("oh what the fuck"); }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            try { _harmony?.UnpatchSelf(); }
            catch { Debug.Log("damn"); }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            OnsenThirst.AddThirstToOnsens();
        }
    }
}
