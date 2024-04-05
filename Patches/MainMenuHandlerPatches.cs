using HarmonyLib;
using SelfSufficient.Utilities;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(MainMenuHandler))]
    [HarmonyPriority(Priority.First + 1)]
    internal static class MainMenuHandlerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MainMenuHandler.ConnectToPhoton))]
        private static bool ConnectToPhotonPatch()
        {
            string? punAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for PUN", "", "The PUN AppID (Only needed for the host)").Value;
            string? voiceAppID = SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for VOICE", "", "The VOICE AppID (Defaults to the PUN AppID)").Value;
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Trying to updated AppIDs from config.");
            if (PhotonAppIDUtilities.AttemptAppIDUpdate(punAppID, voiceAppID))
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs from config");
                PhotonAppIDUtilities.PersonalyOverriddenAppIDs = true;
            }

            if (!PhotonAppIDUtilities.PersonalyOverriddenAppIDs && !PhotonAppIDUtilities.IsUsingDefaultAppIDs())
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs back to default values as they were not overridden by this user");
                PhotonAppIDUtilities.AttemptAppIDUpdate(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID);
            }
            return PhotonAppIDUtilities.IsUsingDefaultAppIDs() ? true : false; // We need to skip the original method if we are updating the AppIDs to our own since we don't got steam auth
        }
    }
}