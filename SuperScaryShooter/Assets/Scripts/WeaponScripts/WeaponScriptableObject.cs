using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObj", menuName = "ScriptableObj/Weapon", order = 0)]
public class WeaponScriptableObject : ScriptableObject
{
    [Header("General Customizable Options")]
    [Tooltip("Do I seriously need to explain what this is? Fine... its the weapon name")]
    public string Name;
    [Tooltip("How long it takes to reactivate")]
    public float Cooldown;
    [Tooltip("How long it stays active")]
    public float StayTime;
    [Tooltip("The multiplier that dictates the distance from the player")]
    public float Offset;
    [Tooltip("In what direction does the attack go")]
    public WeaponDirection Direction;
    public WeaponRarity Rarity;
    [Tooltip("Particle Effects + Hitbox")]
    public GameObject Effects;

    [Header("Damage Variables")]
    public bool CanAttack = true;
    [Tooltip("What is the Input value(damage, healing, etc...) without modifiers")]
    public float RawValue;
    [Tooltip("What the finalized damage is")]
    public float FinalValue;
    [Tooltip("Are there any additional effects")]
    public WeaponElement Element;
    [Tooltip("What is the element value(damage, healing, etc... ) without modifiers" + 
        " // Physical: null, Fire: burn damage, Water: heal value, Earth: multiplier, Lightning: chain damage, Air: knockback power // ")]
    public float RawElementValue;
    [Tooltip("Additional Inputs for elements // Fire: how many ticks, // ")]
    public float SecondaryElementalValue;

    //All the privated components
    private Transform origin;
    private GameObject Temp;
    private GameObject Temp2;
    private GameObject Temp3;
    private GameObject Temp4;
    private Quaternion lookRotation;

    //Firerate, Cooldown, whatever you might want to call it. Its the thing that stops the weapons from firing 24/7
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(Cooldown);
        CanAttack = true;
    }
    //Give the weapons some pizzaz
    private void ApplyElementalType()
    {
        var PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        var WeaponSystem = GameObject.FindAnyObjectByType<WeaponSystem>();
        switch (Element)
        {
            case WeaponElement.Physical:
                FinalValue = RawValue + WeaponSystem.levelUpModValue;
                break;
            case WeaponElement.Fire:
                FinalValue = RawValue + WeaponSystem.levelUpModValue;
                break;
            case WeaponElement.Water:
                FinalValue = RawValue + WeaponSystem.levelUpModValue;
                PlayerHealth.HpNow += RawElementValue + WeaponSystem.levelUpModValue;
                if(PlayerHealth.HpNow > PlayerHealth.HpMax)
                {
                    PlayerHealth.HpNow = PlayerHealth.HpMax;
                }
                break;
            case WeaponElement.Earth:
                FinalValue = (RawValue + WeaponSystem.levelUpModValue) * RawElementValue;
                break;
        }
    }
    public void Fire(Transform origin, MobileJoystick joystick, LayerMask enemies)
    {
        if(Offset == 0)
        {
            Offset = 1;
        }
        //make sure it looks vertical
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        if (CanAttack)
        {
            Debug.Log(name + " Has Fired");
            switch (Direction)
            {
                case WeaponDirection.OriginPoint:
                    //Give it some effects, set the damage, that kind of thing
                    ApplyElementalType();
                    //Game effects
                    Temp = Instantiate(Effects, origin);
                    //Cast it into the fire, DESTROY IT
                    Destroy(Temp, StayTime);
                    break;
                case WeaponDirection.Horizontal:
                    //Give it some effects, set the damage, that kind of thing
                    ApplyElementalType();
                    //Game effects
                    Temp = Instantiate(Effects, origin);
                    Temp2 = Instantiate(Effects, origin);
                    //Set positions
                    Temp.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(1 * Offset,0,0));
                    Temp.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.eulerAngles.z + 45);
                    Temp2.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(-1 * Offset, 0, 0));
                    Temp2.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.z - 135);
                    //Cast it into the fire, DESTROY IT
                    Destroy(Temp , StayTime);
                    Destroy(Temp2 , StayTime);
                    break;
                case WeaponDirection.Vertical:
                    //Give it some effects, set the damage, that kind of thing
                    ApplyElementalType();
                    //Game effects
                    Temp = Instantiate(Effects, origin);
                    Temp2 = Instantiate(Effects, origin);
                    //Set positions
                    lookRotation = Quaternion.LookRotation(origin.position - Temp.transform.position);
                    Temp.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(0, 0, 1 * Offset));
                    Temp.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.eulerAngles.z + 135);
                    Temp2.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(0, 0, -1 * Offset));
                    Temp2.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.z - 45);
                    //Cast it into the fire, DESTROY IT
                    Destroy(Temp, StayTime);
                    Destroy(Temp2, StayTime);
                    break;
                case WeaponDirection.Omnidirectional:
                    //Give it some effects, set the damage, that kind of thing
                    ApplyElementalType();
                    //Game effects
                    Temp = Instantiate(Effects, origin);
                    Temp2 = Instantiate(Effects, origin);
                    Temp3 = Instantiate(Effects, origin);
                    Temp4 = Instantiate(Effects, origin);
                    //Set positions and rotations
                    lookRotation = Quaternion.LookRotation(origin.position - Temp.transform.position);
                    Temp.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(1 * Offset, 0, 0));
                    Temp.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.eulerAngles.z + 45);
                    
                    Temp2.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(-1 * Offset, 0, 0));
                    Temp2.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.z - 135);
                    
                    Temp3.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(0, 0, 1 * Offset));
                    Temp3.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.eulerAngles.z + 135);
                    
                    Temp4.transform.localPosition = matrix.MultiplyPoint3x4(new Vector3(0, 0, -1 * Offset));
                    Temp4.transform.localRotation = Quaternion.Euler(Effects.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Effects.transform.rotation.z - 45);
                    //Cast it into the fire, DESTROY IT
                    Destroy(Temp, StayTime);
                    Destroy(Temp2, StayTime);
                    Destroy(Temp3, StayTime);
                    Destroy(Temp4, StayTime);
                    break;
                case WeaponDirection.DirectionInput:
                    //Give it some effects, set the damage, that kind of thing
                    ApplyElementalType();
                    //Get movement input
                    float horizontalInput = Input.GetAxisRaw("Horizontal");
                    float verticalInput = Input.GetAxisRaw("Vertical");
                    var direction = new Vector3();
                    //calculate movement input to get the direction
                    if (joystick.axisValue.x != 0 || joystick.axisValue.y != 0)
                    {
                        direction = new Vector3(joystick.axisValue.x, 0, joystick.axisValue.y);
                    }
                    else if (horizontalInput == 0 && verticalInput == 0)
                    {
                        direction = origin.right;
                    }
                    else
                    {
                        direction = (origin.forward * verticalInput + origin.right * horizontalInput);
                    }
                    //apply it
                    Temp = Instantiate(Effects, origin);
                    Temp.transform.localPosition = matrix.MultiplyPoint3x4(direction * Offset);
                    lookRotation = Quaternion.LookRotation(origin.position - Temp.transform.position);
                    Temp.transform.localRotation = Quaternion.Euler(Temp.transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, Temp.transform.rotation.eulerAngles.z);
                    Destroy(Temp, StayTime);
                    //a reset
                    direction = Vector3.zero;
                    break;
            }
            CanAttack = false;
            CoroutineRunner.Instance.StartCoroutine(ResetCooldown());
        }
    }
}

