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

        // Event for when the master server is connected
        public static event Action? OnMasterServerConnected;

        // Stops the connection process if we're already connected
        public static bool TryingToConnectToMasterServer { get; private set; } = false;

        // I honestly hate this
        // Rejoin the lobby after the master server is connected
        public static void RejoinLobbyOnMasterServerConnected(SteamLobbyHandler steamLobbyHandler, CSteamID lobbyID)
        {
            if (TryingToConnectToMasterServer) { return; }

            void rejoinAction() { HandleRejoinOnMasterServerConnected(steamLobbyHandler, lobbyID, rejoinAction); }
            OnMasterServerConnected += rejoinAction;
            TryingToConnectToMasterServer = true;
            SelfSufficient.SelfSufficientLogger?.LogInfo("Waiting for master server connection to rejoin lobby");
        }
        public static void HandleRejoinOnMasterServerConnected(SteamLobbyHandler steamLobbyHandler, CSteamID lobbyID, Action? rejoinAction)
        {
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Rejoining lobby {lobbyID} after AppID update");
            steamLobbyHandler.JoinLobby(lobbyID);
            OnMasterServerConnected -= rejoinAction;
            TryingToConnectToMasterServer = false;
        }

        // Rehost the save after the master server is connected
        public static void RehostOnMasterServerConnected(MainMenuHandler mainMenuHandler, int saveIndex)
        {
            if (TryingToConnectToMasterServer) { return; }

            void rehostAction() { HandleOnMasterServerConnected(mainMenuHandler, saveIndex, rehostAction); }
            OnMasterServerConnected += rehostAction;
            TryingToConnectToMasterServer = true;
            SelfSufficient.SelfSufficientLogger?.LogInfo("Waiting for master server connection to rejoin lobby");
        }
        public static void HandleOnMasterServerConnected(MainMenuHandler mainMenuHandler, int saveIndex, Action? rehostAction)
        {
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Rehosting save {saveIndex} after AppID update");
            mainMenuHandler.Host(saveIndex);
            OnMasterServerConnected -= rehostAction;
            TryingToConnectToMasterServer = false;
        }
    }
}
