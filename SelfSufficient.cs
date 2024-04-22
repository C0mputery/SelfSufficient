using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SelfSufficient.Utilities;
using System.Reflection;

namespace SelfSufficient
{
    [ContentWarningPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_VERSION, true)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class SelfSufficient : BaseUnityPlugin
    {
        static internal ManualLogSource? SelfSufficientLogger;
        static internal ConfigFile? SelfSufficientConfigFile;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "SHUT UP VISUAL STUDIO")]
        private void Awake()
        {
            // Logger
            SelfSufficientLogger = base.Logger;
            SelfSufficientLogger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // Config
            SelfSufficientConfigFile = base.Config;
            SelfSufficientConfigFile.Bind("Settings", "AppID for PUN", "", "The PUN AppID (Only needed for the host)"); // This creates the config file if it doesn't exist
            SelfSufficientConfigFile.Bind("Settings", "AppID for VOICE", "", "The VOICE AppID (Defaults to the PUN AppID)"); // This creates the config file if it doesn't exist
            SelfSufficientLogger.LogInfo("Config file is loaded!");

            // Photon Callback Utility
            PhotonCallbackUtility.CreateInstace();
            SelfSufficientLogger.LogInfo("Created PhotonCallbackUtility instance");

            // Harmony
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            SelfSufficientLogger.LogInfo("Patched all Harmony patches");

            SelfSufficientLogger.LogInfo(PhotonAppIDUtilities.HasPersonalyOverriddenAppIDs ? "Using custom Photon AppIds for hosting." : "Using default Photon AppIds for hosting");
        }
    }
}