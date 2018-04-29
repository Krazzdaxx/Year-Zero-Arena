using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanTakeDamage
{
   void TakeDamage(float I, ICanDealDamage source = null);
   
}
public interface ICanDealDamage
{
    void DealDamage(ICanTakeDamage Target);

}
[RequireComponent(typeof(Rigidbody))]

public class melleeWeapon : MonoBehaviour {
    public bool isInAttackFrame = false;
    //private Rigidbody rb;
    private ICanDealDamage DamageDealer;
    // Use this for initialization
    void Start () {
        DamageDealer = this.gameObject.GetComponentInParent<ICanDealDamage>();
     //   rb = this.gameObject.GetComponent<Rigidbody>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (isInAttackFrame)
        {
            ICanTakeDamage c = other.GetComponent<ICanTakeDamage>();
            if (c != null && c != DamageDealer)
            {
                Debug.Log("yo1");
                DamageDealer.DealDamage(c);

               // DamageDealer.damageCharacter(c);              
                isInAttackFrame = false;
            }
        }
    }
  
}
