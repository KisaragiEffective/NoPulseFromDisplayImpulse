using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FrooxEngine;
using FrooxEngine.LogiX.Display;
using FrooxEngine.UIX;
using HarmonyLib;
using JetBrains.Annotations;
using NeosModLoader;

namespace NoPulseFromDisplayImpulse
{
    [UsedImplicitly]
    public class NoPulseFromDisplayImpulse : NeosMod
    {
        public override string Name => "NoPulseFromDisplayImpulse";
        public override string Author => "kisaragi marine";
        public override string Version => "0.1.6";

        public override void OnEngineInit()
        {
            var harmony = new Harmony("com.github.kisaragieffective.neos.NoPulseFromDisplayImpulse");
            harmony.PatchAll();
            Msg("Injected");
        }
    }

    [HarmonyPatch(typeof(DisplayImpulse), nameof(DisplayImpulse.ImpulseTarget))]
    [UsedImplicitly]
    internal class Patch
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [UsedImplicitly]
        // Note that `____text` is `DisplayImpulse#_text`
        // return: false if cancel, true otherwise
        static bool Prefix(DisplayImpulse __instance, SyncRef<Text> ____text, ref Sync<int> ____impulseCount,
            ref int ___GlobalImpulseIndex)
        {
            if (____text.Target == null)
                return true;
            ++____impulseCount.Value;
            ____text.Target.Content.Value =
                $"Last pulse: {__instance.Time.WorldTime}\nFrom: {__instance.LocalUser.UserName}\nTotal: {____impulseCount.Value}\nGlobal Index: {Interlocked.Increment(ref ___GlobalImpulseIndex)}";
            return false;
        }
    }
}
