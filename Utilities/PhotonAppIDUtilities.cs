using Photon.Pun;
using Photon.Realtime;

namespace SelfSufficient.Utilities
{
    internal static class PhotonAppIDUtilities
    {
        // Steam Lobby Data Keys
        static internal readonly string PUN_APP_ID_KEY = "PUN_APP_ID";
        static internal readonly string VOICE_APP_ID_KEY = "VOICE_APP_ID";

        // AppID Overrides
        internal static bool HasPersonalyOverriddenAppIDs
        {
            get
            {
                return !string.IsNullOrWhiteSpace(OverridePunAppID) && !string.IsNullOrWhiteSpace(OverrideVoiceAppID);
            }
        }
        internal static string? OverridePunAppID
        {
            get { return SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for PUN", "", "The PUN AppID (Only needed for the host)").Value; }
        }
        internal static string? OverrideVoiceAppID
        {
            get { return SelfSufficient.SelfSufficientConfigFile?.Bind("Settings", "AppID for VOICE", "", "The VOICE AppID (Defaults to the PUN AppID)").Value; }
        }

        // Default AppIDs
        internal static string DefaultPunAppID { get; private set; } = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        internal static string DefaultVoiceAppID { get; private set; } = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;
        internal static bool IsUsingDefaultAppIDs
        {
            get { return CurrentPunAppID == DefaultPunAppID && CurrentVoiceAppID == DefaultVoiceAppID; }
        }

        // Current AppIDs
        internal static string CurrentPunAppID
        {
            get { return PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime; }
            private set { PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = value; }
        }
        internal static string CurrentVoiceAppID
        {
            get { return PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice; }
            private set { PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = value; }
        }

        // True if the AppIDs are different from the current ones and are not empty
        internal static bool CanUpdateAppID(string? PunAppID, string? VoiceAppID)
        {
            if (string.IsNullOrWhiteSpace(PunAppID) || (PunAppID == CurrentPunAppID && VoiceAppID == CurrentVoiceAppID))
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("AppIDs are already set to the provided values or are empty");
                return false;
            }
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Can update AppIDs to {PunAppID} and {VoiceAppID}");
            return true;
        }

        // Updates the AppIDs and reconnects if needed
        internal static void UpdateAppIDs(string PunAppID, string VoiceAppID, bool forceReconnect)
        {
            SelfSufficient.SelfSufficientLogger?.LogInfo($"Updating AppIDs to {PunAppID} and {VoiceAppID}");

            CurrentPunAppID = PunAppID;
            CurrentVoiceAppID = string.IsNullOrWhiteSpace(VoiceAppID) ? PunAppID : VoiceAppID;

            // Prevents auth issues when switching AppIDs
            PhotonNetwork.AuthValues.AuthType = IsUsingDefaultAppIDs ? CustomAuthenticationType.Steam : CustomAuthenticationType.None;

            SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs to {PunAppID} and {VoiceAppID}");

            if (forceReconnect)
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo("Forcing a reconnect");
                PhotonNetwork.Disconnect();
                if (!PhotonNetwork.ConnectUsingSettings())
                {
                    SelfSufficient.SelfSufficientLogger?.LogError("Failed to reconnect to the master server???");
                }
            }
        }
    }
}
