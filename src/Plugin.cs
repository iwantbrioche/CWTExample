global using System;
global using System.Linq;
global using System.Text.RegularExpressions;
global using System.Collections.Generic;
global using System.Reflection;
global using BepInEx;
global using MoreSlugcats;
global using Mono.Cecil.Cil;
global using MonoMod.Cil;
global using UnityEngine;
global using RWCustom;
global using Random = UnityEngine.Random;
global using Vector2 = UnityEngine.Vector2;
global using Color = UnityEngine.Color;
global using Custom = RWCustom.Custom;
using System.Security;
using System.Security.Permissions;
using BepInEx.Logging;
using System.Runtime.CompilerServices;

#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace CWTExample
{
    [BepInPlugin(MOD_ID, MOD_NAME, MOD_VER)]
    public class CWTExample : BaseUnityPlugin
    {
        public const string MOD_ID = "iwantbread.cwtexample";
        public const string MOD_NAME = "CWTExample";
        public const string MOD_VER = "1.0";
        public static new ManualLogSource Logger { get; private set; }
        private void OnEnable()
        {
            On.RainWorld.OnModsInit += RainWorld_OnModsInit;
            Logger = base.Logger;
        }

        private bool IsInit;

        private void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
        {
            orig(self);
            try
            {
                if (IsInit) return;

                Hooks.Hooks.PatchHooks();
                /*
                 * See the `CWT` and `Hooks` class for usage and how a ConditionalWeakTable works
                 */

                IsInit = true;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{MOD_NAME} failed to load!");
                Logger.LogError(ex);
                throw;
            }
        }

    }
}
