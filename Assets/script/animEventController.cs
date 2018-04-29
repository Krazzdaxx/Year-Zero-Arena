using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// called from animation controller to activate weapon
/// </summary>
public class animEventController : MonoBehaviour {
    public melleeWeapon wp;

    public void setAttackFrame()
    {
    //    Debug.Log("yo2");

        wp.isInAttackFrame = true;
    }
    public void removeAttackFrame()
    {
       // Debug.Log("yo3");

        wp.isInAttackFrame = false;

    }
}
