﻿using HarmonyLib;
using UnityEngine;

namespace EpicLoot
{
    //public void OnDeath()
    [HarmonyPatch(typeof(CharacterDrop), "OnDeath")]
    public static class CharacterDrop_OnDeath_Patch
    {
        public static void Postfix(CharacterDrop __instance)
        {
            if (__instance.m_dropsEnabled)
            {
                EpicLoot.OnCharacterDeath(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(Ragdoll), "Setup")]
    public static class Ragdoll_Setup_Patch
    {
        public static void Postfix(Ragdoll __instance, CharacterDrop characterDrop)
        {
            var characterName = EpicLoot.GetCharacterCleanName(characterDrop.m_character);
            var level = characterDrop.m_character.GetLevel();

            __instance.m_nview.m_zdo.Set("characterName", characterName);
            __instance.m_nview.m_zdo.Set("level", level);
        }
    }

    [HarmonyPatch(typeof(Ragdoll), "SpawnLoot")]
    public static class Ragdoll_SpawnLoot_Patch
    {
        public static void Postfix(Ragdoll __instance, Vector3 center)
        {
            var characterName = __instance.m_nview.m_zdo.GetString("characterName");
            var level = __instance.m_nview.m_zdo.GetInt("level");
            if (!string.IsNullOrEmpty(characterName))
            {
                EpicLoot.OnCharacterDeath(characterName, level, center + Vector3.up * 0.75f);
            }
        }
    }

}
