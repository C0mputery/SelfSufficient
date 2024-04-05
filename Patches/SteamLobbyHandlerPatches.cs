using HarmonyLib;
using Photon.Pun;
using SelfSufficient.Utilities;
using Steamworks;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(SteamLobbyHandler))]
    [HarmonyPriority(Priority.First + 1)] // Virality uses Priority.First and we want to run before it, very hacky lol.
    internal static class SteamLobbyHandlerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SteamLobbyHandler.JoinLobby))]
        private static bool JoinLobbyPatch(ref SteamLobbyHandler __instance, CSteamID lobbyID)
        {
            if (PhotonCallbackUtility.TryingToConnectToMasterServer)
            {
                return false; // Skip the original method if we're already trying to connect to the master server
            }

            // Get the AppIDs from the lobby
            string? punAppId = SteamMatchmaking.GetLobbyData(lobbyID, PhotonAppIDUtilities.PUN_APP_ID_KEY);
            string? voiceAppId = SteamMatchmaking.GetLobbyData(lobbyID, PhotonAppIDUtilities.VOICE_APP_ID_KEY);

            // If the AppIDs are different, update them
            if (PhotonAppIDUtilities.CanUpdateAppID(punAppId, voiceAppId))
            {
                // Update the AppIDs
                PhotonAppIDUtilities.UpdateAppIDs(punAppId, voiceAppId, true);
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs to {punAppId} and {voiceAppId}");

                // Prevent starting the connection process if we're already somehow connected
                if (PhotonNetwork.IsConnectedAndReady)
                {
                    SelfSufficient.SelfSufficientLogger?.LogInfo($"Already connected to the master server, joining lobby {lobbyID}");
                    return true;
                }

                // Wait for the master server to connect before rejoining the lobby
                PhotonCallbackUtility.RejoinLobbyOnMasterServerConnected(__instance, lobbyID);
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Rejoining lobby {lobbyID} after AppID update");
                return false; // Skip the original method since we're waiting for the master server to connect
            }

            SelfSufficient.SelfSufficientLogger?.LogInfo($"AppIDs are already set to {punAppId} and {voiceAppId}, joining lobby {lobbyID}");
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamLobbyHandler.OnLobbyCreatedCallback))]
        private static void OnLobbyCreatedCallbackPatch(ref SteamLobbyHandler __instance)
        {
            // Override the AppIDs if we have them from the config
            if (PhotonAppIDUtilities.HasPersonalyOverriddenAppIDs)
            {
                CSteamID lobbyId = __instance.m_CurrentLobby;
                SteamMatchmaking.SetLobbyData(lobbyId, PhotonAppIDUtilities.PUN_APP_ID_KEY, PhotonAppIDUtilities.OverridePunAppID);
                SteamMatchmaking.SetLobbyData(lobbyId, PhotonAppIDUtilities.VOICE_APP_ID_KEY, PhotonAppIDUtilities.OverrideVoiceAppID);
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Set steam lobby AppIDs to {PhotonAppIDUtilities.CurrentPunAppID} and {PhotonAppIDUtilities.CurrentVoiceAppID}");
            }
        }
    }
}
