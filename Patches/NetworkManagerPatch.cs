// ----------------------------------------------------------------------
// Copyright (c) Tyzeron. All Rights Reserved.
// Licensed under the GNU Affero General Public License, Version 3
// ----------------------------------------------------------------------

using HarmonyLib;
using System.Linq;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

namespace LethalCompanyMinimap.Patches
{
    [HarmonyPatch(typeof(NetworkManager))]
    internal class NetworkManagerPatch
    {
        private static readonly FieldInfo globalObjectIdHashField = AccessTools.Field(typeof(NetworkObject), "GlobalObjectIdHash");

        [HarmonyPatch(nameof(NetworkManager.SetSingleton))]
        [HarmonyPostfix]
        static void RegisterModNetworkPrefabPatch()
        {
            // Register an empty network prefab unique to this mod, so the game's prefab check
            // only allows joining a lobby where everyone (host and clients) has this mod installed
            GameObject prefab = new GameObject(MinimapMod.modGUID + " Prefab");
            prefab.hideFlags |= HideFlags.HideAndDontSave;
            Object.DontDestroyOnLoad(prefab);
            NetworkObject networkObject = prefab.AddComponent<NetworkObject>();
            globalObjectIdHashField.SetValue(networkObject, GetHash(MinimapMod.modGUID));

            NetworkManager.Singleton.PrefabHandler.AddNetworkPrefab(prefab);
        }

        private static uint GetHash(string value)
        {
            return value?.Aggregate(17u, (current, c) => unchecked((current * 31) ^ c)) ?? 0u;
        }

    }
}
