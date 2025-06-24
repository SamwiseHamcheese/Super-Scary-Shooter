using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public WeaponScriptableObject weapon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemies"))
        {
            EnemyBrain enemyBrain = other.GetComponent<EnemyBrain>();
            enemyBrain.TakeDamage(weapon.FinalValue);
            if(weapon.Element == WeaponElement.Fire)
            {
                StartCoroutine(Burn(enemyBrain));
            }
        }
    }
    IEnumerator Burn(EnemyBrain enemyBrain)
    {
        for (int i = 0; weapon.SecondaryElementalValue >  0; i++)
        {
            enemyBrain.TakeDamage(weapon.RawElementValue);
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }
}
