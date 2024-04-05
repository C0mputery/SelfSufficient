using HarmonyLib;
using Photon.Pun;
using SelfSufficient.Utilities;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(MainMenuHandler))]
    [HarmonyPriority(Priority.First + 1)]
    internal static class MainMenuHandlerPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(MainMenuHandler.ConnectToPhoton))]
        private static void ConnectToPhotonPatch()
        {
            if (!PhotonAppIDUtilities.IsUsingDefaultAppIDs)
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs back to default values");

                // Update the AppIDs back to the default values without reconnecting as the connection process is handled by the ConnectToPhoton method
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID, true);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MainMenuHandler.Host))]
        private static bool HostPatch(ref MainMenuHandler __instance, int saveIndex)
        {
            if (PhotonAppIDUtilities.HasPersonalyOverriddenAppIDs && PhotonAppIDUtilities.CanUpdateAppID(PhotonAppIDUtilities.OverridePunAppID, PhotonAppIDUtilities.OverrideVoiceAppID))
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs to personal values");
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.OverridePunAppID!, PhotonAppIDUtilities.OverrideVoiceAppID!, true);
                PhotonCallbackUtility.RehostOnMasterServerConnected(__instance, saveIndex);
                return false;
            }
            return true;
        }
    }
}