using HarmonyLib;
using Photon.Pun;
using SelfSufficient.Utilities;
using Steamworks;
using UnityEngine;

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
            // Quick check so I don't have to do tech support for pirates, and more closely follow the Landfall EULA. https://store.steampowered.com//eula/2881650_eula_0
            // The mod was already seemingly broken for pirates, but this is a more explicit so I don't have to deal with them.
            // Additionally, pirates probably don't even need this mod. Pirated copies prosumably don't have a hard cap on the number of players per match on there own servers.
            // Prior to Landfall adding steam auth to the photon servers, pirates could play on the official servers, but now they can't.
            if (SteamUtils.GetAppID().m_AppId != 2881650) { SelfSufficient.SelfSufficientLogger?.LogWarning($"Using wong steam AppID {SteamUtils.GetAppID().m_AppId}?"); Application.Quit(); }

            if (!PhotonAppIDUtilities.IsUsingDefaultAppIDs)
            {
                SelfSufficient.SelfSufficientLogger!.LogInfo("Updating AppIDs back to default values");

                // Update the AppIDs back to the default values without reconnecting as the connection process is handled by the ConnectToPhoton method
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.DefaultPunAppID, PhotonAppIDUtilities.DefaultVoiceAppID, false);
                PhotonNetwork.Disconnect();
                // The connection process is handled by the ConnectToPhoton method
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(MainMenuHandler.Host))]
        private static bool HostPatch(ref MainMenuHandler __instance, int saveIndex)
        {
            if (PhotonCallbackUtility.TryingToConnectToMasterServer) { return false; }

            if (PhotonAppIDUtilities.HasPersonalyOverriddenAppIDs && PhotonAppIDUtilities.CanUpdateAppID(PhotonAppIDUtilities.OverridePunAppID, PhotonAppIDUtilities.OverrideVoiceAppID))
            {
                SelfSufficient.SelfSufficientLogger!.LogInfo("Updating AppIDs to personal values");
                PhotonAppIDUtilities.UpdateAppIDs(PhotonAppIDUtilities.OverridePunAppID!, PhotonAppIDUtilities.OverrideVoiceAppID!, true);
                PhotonCallbackUtility.RehostOnMasterServerConnected(__instance, saveIndex);
                return false;
            }
            return true;
        }
    }
}