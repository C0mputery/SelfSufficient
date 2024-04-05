using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using SelfSufficient.Utilities;
using System.Reflection;

namespace SelfSufficient
{
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

            SelfSufficientConfigFile = base.Config;
            SelfSufficientLogger.LogInfo("Config file is loaded!");

            // Photon Callback Utility
            PhotonCallbackUtility.CreateInstace();
            SelfSufficientLogger.LogInfo("Created PhotonCallbackUtility instance");

            // Harmony
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            SelfSufficientLogger.LogInfo("Patched all Harmony patches");
        }
    }
}