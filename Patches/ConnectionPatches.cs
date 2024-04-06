using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using SelfSufficient.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SelfSufficient.Patches
{
    [HarmonyPatch(typeof(Connection))]
    [HarmonyPatch("CheckForErrors")]
    public static class ConnectionPatches
    {
        internal static void UpdateAuthType()
        {
            if (!PhotonAppIDUtilities.IsUsingDefaultAppIDs) { PhotonNetwork.AuthValues = null; }
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Connection.Start))]
        static IEnumerable<CodeInstruction> ConnectionStartPatch(IEnumerable<CodeInstruction> instructions)
        {
            SelfSufficient.SelfSufficientLogger!.LogInfo("Transpiler running, should be no more auth values on custom AppId.");
            return new CodeMatcher(instructions)
                .SearchForward(i => i.opcode == OpCodes.Call && i.OperandIs(AccessTools.Method(typeof(PhotonNetwork), nameof(PhotonNetwork.ConnectToRegion))))
                .Advance(-1)
                .ThrowIfInvalid("ConnectionStartPatch did not work!")
                .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ConnectionPatches), nameof(UpdateAuthType))))
                .InstructionEnumeration();
        }
    }
}
