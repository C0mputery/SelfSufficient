using HarmonyLib;
using Photon.Pun;
using SelfSufficient.Utilities;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(MainMenuHandler))]
    [HarmonyPriority(Priority.First + 1)]
    internal static class MainMenuHandlerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MainMenuHandler.ConnectToPhoton))]
        private static void ConnectToPhotonPatch()
        {
            if (PhotonAppIDUtilities.HasPersonalyOverriddenAppIDs)
            {
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.OverridePunAppID!, PhotonAppIDUtilities.OverrideVoiceAppID!, false);
            }
            else if (!PhotonAppIDUtilities.IsUsingDefaultAppIDs)
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Updating AppIDs back to default values");

                // Update the AppIDs back to the default values without reconnecting as the connection process is handled by the ConnectToPhoton method
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID, false);
            }
        }
    }
}