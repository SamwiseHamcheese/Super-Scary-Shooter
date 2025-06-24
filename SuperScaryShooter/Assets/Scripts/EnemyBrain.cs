using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public float Health;
    public GameObject baseObj;
    public GameObject floatingText;
    //public GameObject hitMarker;
    public GameObject canvas;
    //public Slider healthSlider;
    //public Slider easeHealthSlider;
    private float lerpSpeed = 0.05f;
    public int ExpDropped;
    public GameManager GM;
    public EnemyAI Ai;

    [Header("Limiters")]
    public bool Dead = false;
    public bool canMove;
    public bool canAttack;
    private bool activateOnce = true;

    [Header("Movement")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;

    public void Awake()
    {
        //healthSlider.maxValue = Health;
        //easeHealthSlider.maxValue = Health;
        currentMoveSpeed = maxMoveSpeed;
        canvas = GameObject.Find("Canvas");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void Update()
    {
        /*if (healthSlider.value != Health)
        {
            healthSlider.value = Health;
        }
        if (easeHealthSlider.value != healthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, Health, lerpSpeed);
        }*/
        Ai.agent.speed = currentMoveSpeed;
        if (Dead)
        {
            canMove = false;
            canAttack = false;
        }
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;

        /*if (floatingText)
        {
            var go = Instantiate(floatingText, transform.position, Quaternion.identity, this.transform);
            go.GetComponent<TextMeshPro>().text = "-" + damage.ToString();
        }*/

        if (Health <= 0 && this)
        {
            //var go = Instantiate(floatingText, transform.position, Quaternion.identity);
            //go.GetComponent<TextMeshPro>().text = "-" + damage.ToString();
            //go.GetComponent<FloatingText>().DestroyTime = 0.5f;

            StartCoroutine(Death());
        }
    }
    public IEnumerator Death()
    {
        currentMoveSpeed = 0;
        Dead = true;
        //this.transform.GetChild(0).gameObject.SetActive(false);
        if (activateOnce)
        {
            GM.enemiesDead++;
            GM.KillCount++;
            var levelSystem = GameObject.FindAnyObjectByType<LevelSystem>();
            levelSystem.AddExperience(ExpDropped);
            activateOnce = false;
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(baseObj);
    }
}
