using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Health : NetworkBehaviour
{
    [SyncVar (hook = "onChangeHealth")]
    public float health = 100;
    public float maxHp = 100;

    public Image healthDisplay;
   
// Use this for initialization

    void Start ()
    {
		
	}
    //server only
	public void updateHealth(float newVal)
    {
        Debug.Log("est0");

        health += newVal;
        Debug.Log(health);
        // onChangeHealth(health);//used for testmode
        //    Debug.Log(health / maxHp + "percent remaining");
        if (health <= 0)
        {
            RpcDeath();
        }
    }

    //also called by sync var hook on server
    public void onChangeHealth(float _health)
    {
        Debug.Log("est1");

        healthDisplay.fillAmount = Mathf.Lerp(0.5f, 1, (_health / maxHp));

      
    }
    [ClientRpc]
    public void RpcDeath()
    {
        this.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

     
    }
	
}
