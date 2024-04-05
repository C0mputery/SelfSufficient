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
        private static bool JoinLobbyPrefix(ref SteamLobbyHandler __instance, CSteamID lobbyID)
        {
            string? realtimeAppId = SteamMatchmaking.GetLobbyData(lobbyID, SelfSufficient.PUN_APP_ID_KEY);
            string? voiceAppId = SteamMatchmaking.GetLobbyData(lobbyID, SelfSufficient.VOICE_APP_ID_KEY);
            if (PhotonAppIDUtilities.AttemptAppIDUpdate(realtimeAppId, voiceAppId) && !PhotonCallbackUtility.TryingToConnectToMasterServer)
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs to {realtimeAppId} and {voiceAppId}");
                // Prevent stopping the connection process if we're already connected
                if (!PhotonNetwork.IsConnectedAndReady)
                {
                    PhotonCallbackUtility.RejoinLobbyOnMasterServerConnected(__instance, lobbyID);
                    SelfSufficient.SelfSufficientLogger?.LogInfo($"Rejoining lobby {lobbyID} after AppID update");
                    return false; // Don't join the lobby yet
                }
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Already connected to the master server, joining lobby {lobbyID}");
            }
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Joining lobby {lobbyID}");
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(SteamLobbyHandler.OnLobbyCreatedCallback))]
        private static void OnLobbyCreatedCallbackPostfix(ref SteamLobbyHandler __instance)
        {
            if (PhotonAppIDUtilities.PersonalyOverriddenAppIDs)
            {
                CSteamID lobbyId = __instance.m_CurrentLobby;
                SteamMatchmaking.SetLobbyData(lobbyId, SelfSufficient.PUN_APP_ID_KEY, PhotonAppIDUtilities.CurrentPunAppID);
                SteamMatchmaking.SetLobbyData(lobbyId, SelfSufficient.VOICE_APP_ID_KEY, PhotonAppIDUtilities.CurrentVoiceAppID);
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Set AppIDs to {PhotonAppIDUtilities.CurrentPunAppID} and {PhotonAppIDUtilities.CurrentVoiceAppID}");
            }
        }
    }
}
