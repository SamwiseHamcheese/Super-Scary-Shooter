using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health Values")]
    [Tooltip("What is the max amount of hp you got")]
    public float HpMax;
    [Tooltip("Wha is the hp at rn")]
    public float HpNow;

    [Header("The Heath Bar Features")]
    [Tooltip("The main Health Bar")]
    [SerializeField] private Slider healthSlider;
    [Tooltip("The super secret health bar that is used to make the other health bar look cool")]
    [SerializeField] private Slider easeHealthSlider;
    private float lerpSpeed = 0.1f;

    [Header("Regen Feature")]
    [Tooltip("Do you want a regen feature?")]
    public bool CanRegen = true;
    [Tooltip("How much health you get back")]
    public float regenAmount = 15f;
    [Tooltip("The Rate that you get a health regen")]
    [SerializeField] private float Tick = 0.005f;
    [Tooltip("The Delay between regenerating")]
    public float RegenDelay = 2.5f;
    private WaitForSeconds regenTick;
    private Coroutine regen;

    // Start is called before the first frame update
    void Start()
    {
        regenTick = new WaitForSeconds(Tick);
        HpNow = HpMax;
        healthSlider.maxValue = HpNow;
        easeHealthSlider.maxValue = HpNow;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthSlider.value != HpNow)
        {
            healthSlider.value = HpNow;
        }
        if (easeHealthSlider.value != healthSlider.value)
        {
            if(easeHealthSlider.value > healthSlider.value)
            {
                StartCoroutine(DelayEaseBarChange());
            }
            else
            {
                easeHealthSlider.value = healthSlider.value;
            }
        }
        if (HpNow <= 0)
        {
            //GameOver
            SceneManager.LoadScene(0);
        }
    }
    public void TakeDamage(float damage)
    {
        HpNow -= damage;
        if(CanRegen)
        {
            ActivateRegen();
        }
    }
    IEnumerator DelayEaseBarChange()
    {
        yield return new WaitForSeconds(0.5f);
        easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, HpNow, lerpSpeed);
    }
    public void ActivateRegen()
    {
            if (HpNow != HpMax)
            {
                Debug.Log("ActivateRegen");
                if (regen != null)
                    StopCoroutine(regen);
                regen = StartCoroutine(RegenHealth());
            }
    }
    private IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(RegenDelay);
        Debug.Log("Regenerating Lost Health");
        while (HpNow != HpMax)
        {
            HpNow += regenAmount;
            healthSlider.value = HpNow;
            if (HpNow > HpMax)
            {
                HpNow = HpMax;
                healthSlider.value = HpNow;
            }
            yield return regenTick;
        }
        regen = null;
    }

}
