using UnityEngine;
using UnityEngine.UI;

public class UI_WeaponInventorySlot : MonoBehaviour {

    [SerializeField] private Text weaponText = default;
    [SerializeField] private Image weaponImage = default;

    private Color defaultColor;
    private Color activeColor = Color.cyan;

    public void Initialize(WeaponData weaponData) {
        defaultColor = gameObject.GetComponent<Image>().color;
        weaponText.text = weaponData.WeaponType.ToString();
        weaponImage.sprite = weaponData.UI_WeaponSprite;
    }

    public void SetActive(bool isActive) {
        if (isActive == true) {
            gameObject.GetComponent<Image>().color = activeColor;
        } else {
            gameObject.GetComponent<Image>().color = defaultColor;
        }
    }

}