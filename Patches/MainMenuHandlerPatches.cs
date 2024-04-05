﻿using HarmonyLib;
using SelfSufficient.Utilities;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(MainMenuHandler))]
    [HarmonyPriority(Priority.First + 1)]
    internal static class MainMenuHandlerPatches
    {

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MainMenuUIHandler.Start))]
        private static void StartPatch()
        {
            string? punAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for PUN", "", "The PUN AppID (Only needed for the host)").Value;
            string? voiceAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for VOICE", "", "The VOICE AppID (Defaults to the PUN AppID)").Value;
            if (PhotonAppIDUtilities.AttemptAppIDUpdate(punAppID, voiceAppID))
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs to {punAppID} and {voiceAppID}");
                PhotonAppIDUtilities.PersonalyOverriddenAppIDs = true;
            }

            if (!PhotonAppIDUtilities.PersonalyOverriddenAppIDs && PhotonAppIDUtilities.IsUsingDefaultAppIDs())
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs to default values as they were not overridden");
                PhotonAppIDUtilities.AttemptAppIDUpdate(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID);
            }
        }

    }
}