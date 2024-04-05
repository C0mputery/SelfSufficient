using Photon.Pun;
using Steamworks;
using System;
using UnityEngine;

namespace SelfSufficient.Utilities
{
    internal class PhotonCallbackUtility : MonoBehaviourPunCallbacks
    {
        // Singleton, but not really
        private static PhotonCallbackUtility? m_Instance;

        // Event for when the master server is connected
        public static event Action? OnMasterServerConnected;

        // Stops the connection process if we're already connected
        public static bool TryingToConnectToMasterServer { get; private set; } = false;

        // Fake singleton pattern
        public static void CreateInstace()
        {
            if (m_Instance == null)
            {
                GameObject photonCallbackUtilityGameObject = new GameObject("PhotonCallbackUtility");
                DontDestroyOnLoad(photonCallbackUtilityGameObject);
                m_Instance = photonCallbackUtilityGameObject.AddComponent<PhotonCallbackUtility>();
            }
        }

        // Photon Callback
        public override void OnConnectedToMaster()
        {
            OnMasterServerConnected?.Invoke();
        }

        // This is the event that will be called when the master server is connected
        // Wee bit of a hack, but it works
        public static void RejoinLobbyOnMasterServerConnected(SteamLobbyHandler steamLobbyHandler, CSteamID lobbyID)
        {
            if (TryingToConnectToMasterServer) { return; }

            // Most jank code ever or smartest solution, who knows.
            Action? rejoinAction = null;
            rejoinAction = () => { HandleOnMasterServerConnected(steamLobbyHandler, lobbyID, rejoinAction); };
            OnMasterServerConnected += rejoinAction;
            TryingToConnectToMasterServer = true;
            SelfSufficient.SelfSufficientLogger?.LogInfo("Waiting for master server connection to rejoin lobby");
        }

        public static void HandleOnMasterServerConnected(SteamLobbyHandler steamLobbyHandler, CSteamID lobbyID, Action? rejoinAction)
        {
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Rejoining lobby {lobbyID} after AppID update");
            steamLobbyHandler.JoinLobby(lobbyID);
            OnMasterServerConnected -= rejoinAction;
            TryingToConnectToMasterServer = false;
        }
    }
}
