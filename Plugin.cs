using BepInEx;
using Photon.Pun;

namespace SelfSufficient
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            string PunAppID = Config.Bind("Settings", "PUN AppID", "", "The Photon AppID").Value;
            if (!PunAppID.IsNullOrWhiteSpace()) { PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = PunAppID; }

            string VoiceAppID = Config.Bind("Settings", "VOICE AppID", "", "The Voice AppID").Value;
            if (!VoiceAppID.IsNullOrWhiteSpace()) { PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice = VoiceAppID; }
        }
    }
}
