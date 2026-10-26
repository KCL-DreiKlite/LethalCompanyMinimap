// ----------------------------------------------------------------------
// Copyright (c) Tyzeron. All Rights Reserved.
// Licensed under the GNU Affero General Public License, Version 3
// ----------------------------------------------------------------------

using HarmonyLib;

namespace LethalCompanyMinimap.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {

        [HarmonyPatch("OnPlayerConnectedClientRpc")]
        [HarmonyPostfix]
        static void RestoreMapAfterPlayerJoinedPatch(StartOfRound __instance)
        {
            // When a player joins mid-round (e.g. with late-join mods), non-host clients switch the map screen
            // to the moon info display and never switch it back, so restore the radar map like on landing
            if (!__instance.IsServer && !__instance.inShipPhase && __instance.mapScreen != null
                && __instance.mapScreen.overrideCameraForOtherUse)
            {
                __instance.SwitchMapMonitorPurpose();
            }
        }

    }
}
