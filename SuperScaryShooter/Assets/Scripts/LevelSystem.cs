using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [Header("Experience")]
    [SerializeField] AnimationCurve experienceCurve;

    [SerializeField] int currentLvl, totalExperience;
    int previousLvlsExp, nextLvlsExp;
    private void FixedUpdate()
    {
        CheckforLevelup();
    }
    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckforLevelup();
    }
    public void CheckforLevelup()
    {
        if(totalExperience >= nextLvlsExp)
        {
            currentLvl++;
            UpdateLevel();
        }
    }
    void UpdateLevel()
    {
        previousLvlsExp = (int)experienceCurve.Evaluate(currentLvl);
        nextLvlsExp = (int)experienceCurve.Evaluate(currentLvl + 1);
        var WeaponSystem = GameObject.FindAnyObjectByType<WeaponSystem>();
        WeaponSystem.levelUpModValue = currentLvl * 2;
    }
}
