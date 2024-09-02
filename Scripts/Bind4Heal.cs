using PugMod;
using UnityEngine;
using CoreLib;
using CoreLib.RewiredExtension;
using Rewired;
using CoreLib.Util;

public class Bind4Heal : IMod
{
    public static Player player;
    public static string KEYBIND_USE_POTION = "Use Potion";
    public int slotHealIndex = 9;
    public int previousSlotIndex = 0;

    public void EarlyInit()
    {
        CoreLibMod.LoadModules(typeof(RewiredExtensionModule));
        RewiredExtensionModule.rewiredStart += () => {
            player = ReInput.players.GetPlayer(0);
        };
        RewiredExtensionModule.AddKeybind(KEYBIND_USE_POTION, "Use Potion", KeyboardKeyCode.F);

    }

    public void Init()
    {
        UnityEngine.Debug.Log("[BIND 4 HEAL INIT]");
    }

    public void ModObjectLoaded(UnityEngine.Object obj)
    {
    }

    public void Shutdown()
    {
    }

    public void Update()
    {
        handleHealingBehavior();
        
    }
    public void handleHealingBehavior(){
        if (player != null) {
            PlayerController pl = GameManagers.GetMainManager().player;
            // Check if there is already a healing potion or greater healing potion in the slot
            bool potionFound = false || pl.playerInventoryHandler.GetObjectData(slotHealIndex).objectID == ObjectID.HealingPotion 
                || pl.playerInventoryHandler.GetObjectData(slotHealIndex).objectID == ObjectID.GreaterHealingPotion;

            if (player.GetButtonDown(KEYBIND_USE_POTION)) {
                
                // If there isn't a valid healing potion in the slot, search the inventory
                if (!potionFound) {
                    int playerInvSize = pl.playerInventoryHandler.size;

                    // First, try to find a Greater Healing Potion
                    for (int i = 0; i < playerInvSize; i++) {
                        if (pl.playerInventoryHandler.GetObjectData(i).objectID == ObjectID.GreaterHealingPotion) {
                            UnityEngine.Debug.Log($"[NEW HEAL ITEM FOUND] at InvIndex {i}: {pl.playerInventoryHandler.GetObjectData(i).objectID}");
                            pl.playerInventoryHandler.Swap(pl, i, pl.playerInventoryHandler, slotHealIndex);
                            potionFound = true;
                            break;
                        }
                    }

                    // If no Greater Healing Potion is found, search for a regular Healing Potion
                    if (!potionFound) {
                        for (int i = 0; i < playerInvSize; i++) {
                            if (pl.playerInventoryHandler.GetObjectData(i).objectID == ObjectID.HealingPotion) {
                                UnityEngine.Debug.Log($"[NEW HEAL ITEM FOUND] at InvIndex {i}: {pl.playerInventoryHandler.GetObjectData(i).objectID}");
                                pl.playerInventoryHandler.Swap(pl, i, pl.playerInventoryHandler, slotHealIndex);
                                potionFound = true;
                                break;
                            }
                        }
                    }
                }

                // If a potion was found and equipped, use it
                if (potionFound) {
                    previousSlotIndex = pl.equippedSlotIndex;
                    UnityEngine.Debug.Log($"Previous slot: {previousSlotIndex}");
                    Manager.ui.OnEquipmentSlotActivated(slotHealIndex);
                    pl.EquipSlot(slotHealIndex);

                    // Trigger potion usage logic
                    ClientInputHistoryCD componentData = EntityUtility.GetComponentData<ClientInputHistoryCD>(pl.entity, pl.world);
                    componentData.secondInteractUITriggered = true;
                    EntityUtility.SetComponentData<ClientInputHistoryCD>(pl.entity, pl.world, componentData);

                    UnityEngine.Debug.Log("Potion used!");
                } else {
                    UnityEngine.Debug.Log("Potion not found!");
                }
            }

            // Handle releasing the USE POTION keybind
            if (player.GetButtonUp(KEYBIND_USE_POTION)) {
                if (potionFound) {
                    Manager.ui.OnEquipmentSlotActivated(previousSlotIndex);
                    pl.EquipSlot(previousSlotIndex);
                }
            }
        }
    }
}
