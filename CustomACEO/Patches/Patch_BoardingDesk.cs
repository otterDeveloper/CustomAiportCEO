using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using Nodes;

namespace MyCustomACEO.Patches
{
    [HarmonyPatch(typeof(BoardingDeskController))]
    static class Patch_BoardingDesk
    {
        //1. Change CLASSTOPATCH to the class you are modifying/overwriting.
        //2. Change FUNCTIONTOPATCH to the function/method you are modifying/overwriting.
        //3. Rename the PURPOSEOFPATCH.cs file and class to the purpose of your patch.
        //4. Add your harmony postfix, prefix or transpiler here.
        [HarmonyPostfix]
        [HarmonyPatch(nameof(BoardingDeskController.CanBeRelocated))]
        static void Patch_CanBeRelocated(BoardingDeskController __instance, ref bool __result)
        {
            CanBeRelocatedOrDemolished(__instance, ref __result);
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(BoardingDeskController.CanDemolish))]
        static void Patch_CanBeDemolished(BoardingDeskController __instance, ref bool __result)
        {
            CanBeRelocatedOrDemolished(__instance, ref __result);
        }

        private static void CanBeRelocatedOrDemolished(BoardingDeskController __instance, ref bool __result)
        {
            if (__result) return;
            if (__instance.ConnectedStand == null || __instance.ConnectedStand.BoardingDesks == null)
            {
                __result = true;
                return;
            }

            if (__instance.ConnectedStand.BoardingDesks.Count <= 1)
                return;

            if (__instance.CurrentFlight == null)
            {
                __result = true;
            }
            else if (!__instance.CurrentFlight.IsBoarding && __instance.CurrentFlight.boardingClosed)
            {
                __result = true;
            }
        }

       
    }
}