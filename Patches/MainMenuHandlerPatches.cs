using HarmonyLib;
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
            string? PunAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for PUN", "", "The PUN AppID (Only needed for the host)").Value;
            string? VoiceAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for VOICE", "", "The VOICE AppID (Defaults to the PUN AppID)").Value;
            if (PhotonAppIDUtilities.AttemptAppIDUpdate(PunAppID, VoiceAppID))
            {
                PhotonAppIDUtilities.PersonalyOverriddenAppIDs = true;
            }

            if (!PhotonAppIDUtilities.PersonalyOverriddenAppIDs)
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs to default values as they were not overridden");
                PhotonAppIDUtilities.AttemptAppIDUpdate(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID);
            }
        }

    }
}