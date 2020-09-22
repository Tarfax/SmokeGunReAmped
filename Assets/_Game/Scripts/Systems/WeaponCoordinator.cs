using MC_Utility;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCoordinator : MonoBehaviour {
    private static WeaponCoordinator instance;

    [SerializeField] private List<WeaponData> listOfWeapons = default;
    private Dictionary<WeaponType, WeaponData> weapons;

    private void Start() {
        if (instance == null) {
            instance = this;
            EventSystem<PickupLootEvent>.RegisterListener<LootData_Weapon>(OnPickUpLoot_Weapon);

            weapons = new Dictionary<WeaponType, WeaponData>();
            for (int i = 0; i < listOfWeapons.Count; i++) {
                WeaponData weaponData = listOfWeapons[i];
                weapons.Add(weaponData.WeaponType, weaponData);
            }
        }
    }

    private void OnPickUpLoot_Weapon(PickupLootEvent pickupItemEvent) {
        LootData_Weapon itemDropData = pickupItemEvent.LootData as LootData_Weapon;
        if (weapons.ContainsKey(itemDropData.WeaponData.WeaponType) == false) {
            weapons.Add(itemDropData.WeaponData.WeaponType, itemDropData.WeaponData);
            IWeapon weapon = Weapon.GetWeaponOfType(Weapon.GetType(itemDropData.WeaponData.WeaponType));
            EventSystem<WeaponPickupEvent>.FireEvent(GetWeaponPickupEvent(weapon, itemDropData.WeaponData));
        }
    }

    private WeaponPickupEvent GetWeaponPickupEvent(IWeapon weapon, WeaponData weaponData) {
        return new WeaponPickupEvent() {
            NewWeapon = weapon,
            WeaponData = weaponData,
        };
    }

    public static WeaponData GetWeaponData(WeaponType type) {
        if (instance.weapons.ContainsKey(type) == true) {
            return instance.weapons[type];
        }
        return default;
    }

    private void OnDisable() {
        EventSystem<PickupLootEvent>.UnregisterListener<LootData_Weapon>(OnPickUpLoot_Weapon);
        instance = null;
    }

}