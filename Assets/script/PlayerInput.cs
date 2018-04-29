using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;
public interface IControleable
{

   void setInput(Vector3 input);
   void attack();
}
public class PlayerInput : NetworkBehaviour {
    private IControleable myCharacter;
    private Vector3 rawInput = Vector3.zero;
    public bool test = false;
    // Use this for initialization
    void Start ()
    {
        myCharacter = gameObject.GetComponent<IControleable>();
	}
	
	// Update is called once per frame
	void Update () {

        if (!isLocalPlayer && test == false)
            return;

        rawInput.x = CrossPlatformInputManager.GetAxis("Horizontal");
        rawInput.z = CrossPlatformInputManager.GetAxis("Vertical");
        rawInput = Camera.main.transform.TransformDirection(rawInput);// transform.TransformDirection(moveDirection);
        rawInput = new Vector3(rawInput.x, 0, rawInput.z);

        myCharacter.setInput(rawInput);

        if (CrossPlatformInputManager.GetButtonUp("Attack"))
        {
            myCharacter.attack();
        }
    }
}
