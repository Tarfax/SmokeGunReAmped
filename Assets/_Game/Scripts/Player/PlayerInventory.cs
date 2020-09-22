using MC_Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {
    [SerializeField] private PlayerController playerController = default;
    [SerializeField] private PlayerWeaponHand weaponHand = default;
    [SerializeField] private PlayerForceField forceField = default;
    [SerializeField] private Slider slider_forceShield = default;
    [SerializeField] private Text text_ForceFieldValue = default;
    [SerializeField] private Text text_HomingMissileValue = default;
    [SerializeField] private RectTransform UI_WeaponInventoryParent = default;
    [SerializeField] private GameObject WeaponInventorySlot_Prefab = default;

    private List<UI_WeaponInventorySlot> weaponSlots;

    [SerializeField] public static int   moneyBagsCollected = 0;
    [SerializeField] public static int   forceFieldsCollected = 0;
    [SerializeField] public static float ForceFieldSecondsCollected = 0;
    [SerializeField] public static int   CollectedMoney = 0;
    [SerializeField] public static int   HomingMissilesCollected = 0;

    private List<IWeapon> weaponInventory = default;
    private int usingWeaponCounter = 0;

    private bool isForceFieldActive = false;
    [SerializeField] private float forceFieldChargeTime = 5f;
    [SerializeField] private float forceFieldStartValue = 5f;

    private int homingMissiles = 0;
    private int HomingMissiles {
        get {
            return homingMissiles;
        }
        set {
            homingMissiles = value;
            text_HomingMissileValue.text = homingMissiles.ToString();
        }
    }

    private void Start() {
        weaponInventory = new List<IWeapon>();

        playerController.RegisterOnForceFieldPressedCallback(ActivateForceField);
        playerController.RegisterOnForceFieldReleasedCallback(DeactivateForceField);
        playerController.RegisterOnChangeWeaponCallback(OnChangeWeapon);
        playerController.RegisterOnFireHomingMissilePressedCallback(OnFireHomingMissle);

        EventSystem<WeaponPickupEvent>.RegisterListener(OnWeaponPickup);
        EventSystem<WeaponUpgradeEvent>.RegisterListener(OnWeaponUpgrade);

        EventSystem<PickupLootEvent>.RegisterListener<LootData_ForceField>(OnPickupLoot_ForceField);
        EventSystem<PickupLootEvent>.RegisterListener<LootData_Money>(OnPickupLoot_Money);
        EventSystem<PickupLootEvent>.RegisterListener<LootData_HomingMissile>(OnPickupLoot_HomingMissile);
        SetDefaultValues();
        DeactivateForceField();
    }

    private void SetDefaultValues() {
        moneyBagsCollected = 0;
        forceFieldsCollected = 0;
        ForceFieldSecondsCollected = 0;
        CollectedMoney = 0;
        HomingMissilesCollected = 0;
        usingWeaponCounter = 0;
        HomingMissiles = 1;
    }

    private void OnPickupLoot_Money(PickupLootEvent loot) {
        LootData_Money money = loot.LootData as LootData_Money;

        moneyBagsCollected++;
        CollectedMoney += money.GetMoneyInDrop();
    }

    private void OnPickupLoot_ForceField(PickupLootEvent loot) {
        LootData_ForceField forceField = loot.LootData as LootData_ForceField;

        forceFieldsCollected++;
        ForceFieldSecondsCollected += forceField.GetSecondsInDrop();
        forceFieldChargeTime += forceField.GetSecondsInDrop();
        text_ForceFieldValue.text = forceFieldChargeTime.ToString("0.00");
    }

    private void OnPickupLoot_HomingMissile(PickupLootEvent loot) {
        HomingMissiles++;
        HomingMissilesCollected++;
    }

    private void OnFireHomingMissle() {
        if (HomingMissiles > 0) {
            weaponHand.FireHomingMissile();
            HomingMissiles--;
        }
    }

    private void OnWeaponPickup(WeaponPickupEvent data) {
        AddNewWeapon(data.NewWeapon);
        AddWeaponToCanvas(data.WeaponData);
        SwitchToWeapon(data.NewWeapon);
    }


    public void AddNewWeapon(IWeapon newWeapon) {
        if (weaponInventory.Contains(newWeapon) == false) {
            weaponInventory.Add(newWeapon);
            newWeapon.Initialize(weaponHand.transform);
        }
    }

    private void AddWeaponToCanvas(WeaponData data) {
        GameObject weaponSlot = Instantiate(WeaponInventorySlot_Prefab, UI_WeaponInventoryParent);
        UI_WeaponInventorySlot inventorySlot = weaponSlot.GetComponent<UI_WeaponInventorySlot>();
        if (weaponSlots == null) {
            weaponSlots = new List<UI_WeaponInventorySlot>();
        }
        weaponSlots.Add(inventorySlot);
        inventorySlot.Initialize(data);
    }

    public void OnChangeWeapon() {
        if (weaponInventory.Count > 0) {
            weaponSlots[usingWeaponCounter].SetActive(false);
            usingWeaponCounter++;
            if (usingWeaponCounter >= weaponInventory.Count) {
                usingWeaponCounter = 0;
            }
            IWeapon weapon = weaponInventory[usingWeaponCounter];
            weaponSlots[usingWeaponCounter].SetActive(true);
            weaponHand.SwitchToWeapon(weapon);
        }
    }

    private void SwitchToWeapon(IWeapon weapon) {
        for (int i = 0; i < weaponInventory.Count; i++) {
            if (weaponInventory[i] == weapon) {
                weaponHand.SwitchToWeapon(weaponInventory[i]);
                weaponSlots[usingWeaponCounter].SetActive(false);
                weaponSlots[i].SetActive(true);
                usingWeaponCounter++;
                if (usingWeaponCounter >= weaponInventory.Count) {
                    usingWeaponCounter = 0;
                }
                return;
            }
        }
    }

    //This is for debug only.
    private void CreateFirstWeapon() {
        IWeapon weapon = Weapon.GetWeaponOfType<SingleBulletGun>();
        AddNewWeapon(weapon);
    }

    private void ActivateForceField() {
        text_ForceFieldValue.text = forceFieldChargeTime.ToString("0.00");
        if (forceFieldChargeTime <= 0f && isForceFieldActive == true) {
            DeactivateForceField();
        }
        else if (forceFieldChargeTime > 0f) {
            forceField.ActivateShield();
            slider_forceShield.gameObject.SetActive(true);

            if (isForceFieldActive == false) {
                forceFieldStartValue = forceFieldChargeTime;
                if (forceFieldStartValue >= 1f) {
                    slider_forceShield.maxValue = forceFieldStartValue;
                }
                else {
                    slider_forceShield.maxValue = 1f;
                }
                isForceFieldActive = true;
            }
            forceFieldChargeTime -= Time.deltaTime;
            slider_forceShield.value = forceFieldChargeTime;
        }
    }

    private void DeactivateForceField() {
        text_ForceFieldValue.text = forceFieldChargeTime.ToString("0.00");
        forceField.DeactivateShield();
        slider_forceShield.gameObject.SetActive(false);
        isForceFieldActive = false;
    }

    private void OnWeaponUpgrade(WeaponUpgradeEvent data) {
        //Not implemented
        Debug.Log("Picked up an upgrade, now what??");
    }

    private void OnDisable() {
        playerController.UnregisterOnForceFieldPressedCallback(ActivateForceField);
        playerController.UnregisterOnForceFieldReleasedCallback(DeactivateForceField);
        playerController.UnregisterOnFireHomingMissilePressedCallback(OnFireHomingMissle);

        EventSystem<WeaponPickupEvent>.UnregisterListener(OnWeaponPickup);
        EventSystem<WeaponUpgradeEvent>.UnregisterListener(OnWeaponUpgrade);

        EventSystem<PickupLootEvent>.UnregisterListener<LootData_ForceField>(OnPickupLoot_ForceField);
        EventSystem<PickupLootEvent>.UnregisterListener<LootData_Money>(OnPickupLoot_Money);
        EventSystem<PickupLootEvent>.UnregisterListener<LootData_HomingMissile>(OnPickupLoot_HomingMissile);
    }

}
