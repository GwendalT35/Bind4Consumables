using PugMod;
using UnityEngine;
using CoreLib;
using CoreLib.RewiredExtension;
using Rewired;
using CoreLib.Util;
using System.Collections.Generic;

// Pug.Base/ConditionID enum state
//FOOD 9500 - 9513
//9550 - 9563
//9575 - 9588
public class Bind4Food : IMod
{
    public static Player player;
    public static string KEYBIND_USE_FOOD = "Use Food";
    public int slotFoodIndex = 8;
    public int previousSlotIndex = 0;

    Dictionary<string, bool> dictFood = new Dictionary<string, bool>{
        {"CookedSoup", true},
        {"CookedPudding", true},
        {"CookedSalad", true},
        {"CookedWrap", true},
        {"CookedSteak", true},
        {"CookedCheese", true},
        {"CookedDipSnack", true},
        {"CookedSushi", true},
        {"CookedFishBalls", true},
        {"CookedFillet", true},
        {"CookedCereal", true},
        {"CookedSmoothie", true},
        {"CookedSandwich", true},
        {"CookedPanCurry", true},
        {"CookedCake", true},
        {"CookedSoupRare", true},
        {"CookedPuddingRare", true},
        {"CookedSaladRare", true},
        {"CookedWrapRare", true},
        {"CookedSteakRare", true},
        {"CookedCheeseRare", true},
        {"CookedDipSnackRare", true},
        {"CookedSushiRare", true},
        {"CookedFishBallsRare", true},
        {"CookedFilletRare", true},
        {"CookedCerealRare", true},
        {"CookedSmoothieRare", true},
        {"CookedSandwichRare", true},
        {"CookedPanCurryRare", true},
        {"CookedCakeRare", true},
        {"CookedSoupEpic", true},
        {"CookedPuddingEpic", true},
        {"CookedSaladEpic", true},
        {"CookedWrapEpic", true},
        {"CookedSteakEpic", true},
        {"CookedCheeseEpic", true},
        {"CookedDipSnackEpic", true},
        {"CookedSushiEpic", true},
        {"CookedFishBallsEpic", true},
        {"CookedFilletEpic", true},
        {"CookedCerealEpic", true},
        {"CookedSmoothieEpic", true},
        {"CookedSandwichEpic", true},
        {"CookedPanCurryEpic", true},
        {"CookedCakeEpic", true},
        {"HeartBerry", true},
        {"GlowingTulipFlower", true},
        {"BombPepper", true},
        {"Carrock", true},
        {"Puffungi", true},
        {"BloatOat", true},
        {"Pewpaya", true},
        {"Pinegrapple", true},
        {"Grumpkin", true},
        {"Sunrice", true},
        {"Lunacorn", true},
        {"HeartBerryRare", true},
        {"GlowingTulipFlowerRare", true},
        {"BombPepperRare", true},
        {"CarrockRare", true},
        {"PuffungiRare", true},
        {"BloatOatRare", true},
        {"PewpayaRare", true},
        {"PinegrappleRare", true},
        {"GrumpkinRare", true},
        {"SunriceRare", true},
        {"LunacornRare", true}
    };

    public void EarlyInit()
    {
        CoreLibMod.LoadModules(typeof(RewiredExtensionModule));
        RewiredExtensionModule.rewiredStart += () => {
            player = ReInput.players.GetPlayer(0);
        };
        RewiredExtensionModule.AddKeybind(KEYBIND_USE_FOOD, "Use Food", KeyboardKeyCode.G);

    }

    public void Init()
    {
        UnityEngine.Debug.Log("[BIND 4 FOOD INIT]");
    }

    public void ModObjectLoaded(UnityEngine.Object obj)
    {
    }

    public void Shutdown()
    {
    }

    public void Update()
    {
        handleEatingBehavior();
        
    }
    public void handleEatingBehavior(){
        if (player != null) {
            PlayerController pl = GameManagers.GetMainManager().player;
            // Check if there is already food in the slot
            bool foodFound = false || dictFood.ContainsKey(pl.playerInventoryHandler.GetObjectData(slotFoodIndex).objectID.ToString());

            if (player.GetButtonDown(KEYBIND_USE_FOOD)) {
                // If there isn't a valid food in the slot, search the inventory
                if (!foodFound) {
                    int playerInvSize = pl.playerInventoryHandler.size;
                    for (int i = 0; i < playerInvSize; i++){
                        if (dictFood.ContainsKey(pl.playerInventoryHandler.GetContainedObjectData(i).objectID.ToString()))
                        {
                            UnityEngine.Debug.Log($"[FOOD FOUND DEBUG] index {i} : {pl.playerInventoryHandler.GetObjectData(i)}");
                            pl.playerInventoryHandler.Swap(pl, i, pl.playerInventoryHandler, slotFoodIndex);
                            foodFound = true;
                            break;
                        }
                    }
                    
                    
                }

                // If a Food was found and equipped, use it
                if (foodFound) {
                    previousSlotIndex = pl.equippedSlotIndex;
                    UnityEngine.Debug.Log($"Previous slot: {previousSlotIndex}");
                    Manager.ui.OnEquipmentSlotActivated(slotFoodIndex);
                    pl.EquipSlot(slotFoodIndex);

                    // Trigger Food usage logic
                    ClientInputHistoryCD componentData = EntityUtility.GetComponentData<ClientInputHistoryCD>(pl.entity, pl.world);
                    componentData.secondInteractUITriggered = true;
                    EntityUtility.SetComponentData<ClientInputHistoryCD>(pl.entity, pl.world, componentData);

                    UnityEngine.Debug.Log("Food used!");
                } else {
                    UnityEngine.Debug.Log("Food not found!");
                    
                }
            }

            // Handle releasing the USE FOOD keybind
            if (player.GetButtonUp(KEYBIND_USE_FOOD)) {
                if (foodFound) {
                    Manager.ui.OnEquipmentSlotActivated(previousSlotIndex);
                    pl.EquipSlot(previousSlotIndex);
                }
            }
        }
    }
}
