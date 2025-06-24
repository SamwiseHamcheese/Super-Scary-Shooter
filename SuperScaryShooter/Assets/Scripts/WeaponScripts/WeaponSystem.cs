using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public Transform origin;
    public LayerMask enemies;
    public AnimationCurve valueModifier;
    public int levelUpModValue;
    public List<WeaponScriptableObject> equippedWeapons = new List<WeaponScriptableObject>();
    public List<WeaponScriptableObject> availableWeapons = new List<WeaponScriptableObject>();
    private List<WeaponScriptableObject> commonWeapons = new List<WeaponScriptableObject>();
    private List<WeaponScriptableObject> rareWeapons = new List<WeaponScriptableObject>();
    private MobileJoystick joystick;
    // Start is called before the first frame update
    void Start()
    {
        joystick = GameObject.FindObjectOfType<MobileJoystick>();
        foreach (var weapon in equippedWeapons)
        {
            var hitbox = weapon.Effects.GetComponent<Hitbox>();
            hitbox.weapon = weapon;
            weapon.CanAttack = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(equippedWeapons.Count > 0)
        {
            FireWeapons();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddNewWeapon();
        }
    }
    public void FireWeapons()
    {
        foreach (var weapon in equippedWeapons)
        {
            weapon.Fire(origin, joystick, enemies);
        }
    }
    public void AddNewWeapon()
    {
        foreach(var weapon in availableWeapons)
        {
            if(weapon.Rarity == WeaponRarity.Common)
            {
                commonWeapons.Add(weapon);
            }
            if(weapon.Rarity == WeaponRarity.Rare)
            {
                commonWeapons.Add(weapon);
            }
        }
        int probability = 75;
        int randomNumber = Random.Range(0, 100);
        if(randomNumber > probability)
        {
            var randomRare = Random.Range(0, rareWeapons.Count);
            equippedWeapons.Add(rareWeapons[randomRare]);
        }
        if(randomNumber <= probability)
        {
            var randomCommon = Random.Range(0, commonWeapons.Count);
            equippedWeapons.Add(commonWeapons[randomCommon]);
        }
        commonWeapons.Clear();
        rareWeapons.Clear();
    }
}
