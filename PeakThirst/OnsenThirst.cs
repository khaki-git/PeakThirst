using System.Collections.Generic;
using UnityEngine;

namespace PeakThirst
{
    public static class OnsenThirst
    {
        // doesn't work rn, it creates the game object but it and sets the status type but it does nothing still?
        // may need to create a new emitter monobehaviour :(
        public static void AddThirstToOnsens(bool includeInactive = false)
        {
            GameObject[] thirstZones = FindAllByName("heat zone", includeInactive);
            foreach (GameObject src in thirstZones)
            {
                GameObject clone = Object.Instantiate(src);
                clone.transform.SetParent(src.transform.parent, true);

                StatusEmitter emitter = clone.GetComponent<StatusEmitter>();
                if (emitter == null) emitter = clone.AddComponent<StatusEmitter>();
                emitter.statusType = ThirstAffliction.DehydrationType;
            }
        }

        public static GameObject[] FindAllByName(string targetName, bool includeInactive = false)
        {
            if (string.IsNullOrEmpty(targetName))
                return System.Array.Empty<GameObject>();

            var inactiveFlag = includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            Transform[] transforms = Object.FindObjectsByType<Transform>(inactiveFlag, FindObjectsSortMode.None);

            List<GameObject> results = new List<GameObject>(transforms.Length);
            foreach (var t in transforms)
            {
                if (t != null && t.name == targetName)
                    results.Add(t.gameObject);
            }
            return results.ToArray();
        }
    }
}
