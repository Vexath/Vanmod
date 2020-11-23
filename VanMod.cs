using MelonLoader;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Object = Il2CppSystem.Object;
using Boolean = Il2CppSystem.Boolean;
using Int32 = Il2CppSystem.Int32;
using System.Collections.Generic;



namespace PhasmoMelonMod
{
    class VanMod
    {
        // Need these to determine if hunting cycle completed
        public static string Current;
        public static string Previous;

        // Need this to 'redefine' outside after VanHunt
        public static LevelRoom Outside;
        public static LevelRoom GhostRoom;

        // Teleport the doors away for vanhunt

        public static void GetGhostState()
        {
            if (Main.levelController != null && Main.ghostAI != null)
            {
                switch (Main.ghostAI.field_Public_EnumNPublicSealedvaidwahufalidothfuapUnique_0)
                {
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.idle:
                        Main.ghostState = "Idle";
                        VanMod.SpawnInTheTruck("Idle");
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.wander:
                        Main.ghostState = "Wander";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.hunting:
                        Main.ghostState = "Hunting";
                        VanMod.SpawnInTheTruck("Hunting");
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.favouriteRoom:
                        Main.ghostState = "Favourite Room";
                        VanMod.SpawnInTheTruck("Favourite Room");
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.light:
                        Main.ghostState = "Light";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.door:
                        Main.ghostState = "Door";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.throwing:
                        Main.ghostState = "Throwing";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.fusebox:
                        Main.ghostState = "Fusebox";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.appear:
                        Main.ghostState = "Appear";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.doorKnock:
                        Main.ghostState = "Knock Door";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.windowKnock:
                        Main.ghostState = "Knock Window";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.carAlarm:
                        Main.ghostState = "Car Alarm";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.radio:
                        Main.ghostState = "Radio";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.flicker:
                        Main.ghostState = "Flicker";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.lockDoor:
                        Main.ghostState = "Lock Door";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.cctv:
                        Main.ghostState = "CCTV";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.randomEvent:
                        Main.ghostState = "Random Event";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.GhostAbility:
                        Main.ghostState = "Ghost Ability";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.killPlayer:
                        Main.ghostState = "Kill Player";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.sink:
                        Main.ghostState = "Sink";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.sound:
                        Main.ghostState = "Sound";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.painting:
                        Main.ghostState = "Painting";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.mannequin:
                        Main.ghostState = "Mennequin";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.teleportObject:
                        Main.ghostState = "Teleport Object";
                        break;
                    case GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.animationObject:
                        Main.ghostState = "Animate Object";
                        break;
                    default:
                        Main.ghostState = "Idle";
                        break;
                }
            }
        }
        public static void SpawnInTheTruck(string GhostState)
        {
            // Keep a record of GhostStates to use as our trigger
            Previous = Current;
            Current  = GhostState;

            // When a hunt ends restore outside
            if (Current == "Favourite Room" && Previous == "Hunting")
            {
                Main.levelController.field_Public_LevelRoom_2 = Outside;
            }

            // when a new hunt happens check to see if it should be a vanhunt
            if (Current == "Hunting" && Previous == "Idle")
            {
                // 1/5 chance to spawn in van
                int spawnChance = UnityEngine.Random.Range(0, 5);
                MelonLogger.Log("Spawn Chance: " + spawnChance);
                // delete me after debug
                spawnChance = 4;
                // delete me after debug

                if (spawnChance == 4)
                {
                    MelonLogger.Log("VanHunt Triggered");
                    
                    // temp store reference to outside room
                    // pretty sure field_Public_LevelRoom_0 is any room in house
                    // pretty sure field_Public_LevelRoom_1 is ghost room
                    // field_Public_LevelRoom_2 is outside

                    // store the actual 'outside' for later restoration
                    Outside = Main.levelController.field_Public_LevelRoom_2;

                    //Attempt to declare the outside to be the ghost room
                    Main.levelController.field_Public_LevelRoom_2 = Main.levelController.field_Public_LevelRoom_1;
                    
                    // warp ghost to player spawn
                    Main.ghostAI.field_Public_NavMeshAgent_0.Warp(Main.playerSpawnPosition);

                    // Open all doors to give van peepz a shot
                    foreach (Door door in Main.doors)
                    {
                        door.DisableOrEnableDoor(true);
                        door.DisableOrEnableCollider(true);
                        door.UnlockDoor();
                    }
                }

                else
                {
                return;
                }
            }
        }
    }
}
