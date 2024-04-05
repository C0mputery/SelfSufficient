using Photon.Pun;

namespace SelfSufficient.Utilities
{
    internal static class PhotonAppIDUtilities
    {
        // Steam Lobby Data Keys
        static internal readonly string PUN_APP_ID_KEY = "PUN_APP_ID";
        static internal readonly string VOICE_APP_ID_KEY = "VOICE_APP_ID";

        // Default AppIDs
        internal static bool PersonalyOverriddenAppIDs = false;
        internal static string DefaultPunAppID { get; private set; } = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        internal static string DefaultVoiceAppID { get; private set; } = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice;

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

        /// <summary>
        /// Attempts to update the AppIDs for PUN and Voice
        /// </summary>
        /// <param name="PunAppID"> The AppID for PUN </param>
        /// <param name="VoiceAppID"> The AppID for Voice </param>
        /// <returns>true if the AppIDs were updated, false otherwise</returns>
        internal static bool AttemptAppIDUpdate(string? PunAppID, string? VoiceAppID)
        {
            if (string.IsNullOrWhiteSpace(PunAppID) || (PunAppID == CurrentPunAppID && VoiceAppID == CurrentVoiceAppID))
            {
                SelfSufficient.SelfSufficientLogger?.LogInfo($"Failed to update AppIDs to {PunAppID} and {VoiceAppID}, current AppIDs are {CurrentPunAppID} and {CurrentVoiceAppID}");
                return false; // Cannot update the AppIDs
            }

            CurrentPunAppID = PunAppID;
            CurrentVoiceAppID = string.IsNullOrWhiteSpace(VoiceAppID) ? PunAppID : VoiceAppID;

            // Force a disconnect and reconnect to update the AppIDs
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectUsingSettings();

            SelfSufficient.SelfSufficientLogger?.LogInfo($"Updated AppIDs to {PunAppID} and {VoiceAppID}");

            return true;
        }

        internal static bool IsUsingDefaultAppIDs()
        {
            if (PersonalyOverriddenAppIDs) { return false; }
            return CurrentPunAppID == DefaultPunAppID && CurrentVoiceAppID == DefaultVoiceAppID;
        }
    }
}
