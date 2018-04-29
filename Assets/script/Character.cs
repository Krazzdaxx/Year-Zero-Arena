using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]

[RequireComponent(typeof(Health))]

public class Character : NetworkBehaviour, ICanTakeDamage, ICanDealDamage, IControleable
{
    public bool isPlayerChar = false;
    [Header("variables")]
    public CharacterStats stats;
    float speed = 6.0F;
    float jumpSpeed = 8.0F;
    float gravity = 20.0F;
    [HideInInspector]
    public float mainAttackDamage = 10;

    private Vector3 moveDirection = Vector3.zero;

    [Header("References")]
    [SerializeField] CharacterController controller;
    [SerializeField] Health myHealth;
    public Animator anim;
    public Transform mesh;
    [HideInInspector]
    public Transform LockTarget = null;

    [HideInInspector] public bool isLockedOn = false;
    [Header("Events")]
    public UnityEvent onAttack;
    public UnityEvent OnDamageTaken;
    public bool test = false;
    void Awake()
    {
        speed = stats.speed;
        jumpSpeed = stats.jumpSpeed;
        float gravity = stats.gravity;
        mainAttackDamage = stats.mainAttackDamage;
    }
    public override void OnStartLocalPlayer()
    {
        GameController._instance.Player = this;
    }
    /// <summary>
    /// called by playerInput or by aiInput
    /// </summary>
    /// <param name="input"></param>
    public void setInput(Vector3 input)
    {
        moveDirection = input;
    }
    /// <summary>
    /// called by gameController
    /// </summary>
    public void toggleLockOn()
    {
        isLockedOn = !isLockedOn;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myHealth = this.gameObject.GetComponent<Health>();

    }
    //used for target lock mechanic , to be moved to separate script for reseuse
    void OnMouseDown()
    {
        if (GameController._instance != null)
        {
            GameController._instance.SetPlayerLock(this);
        }
        else
        {
            Debug.LogWarning("no game controller prefab in scene");
        }
    }
    /// <summary>
    /// starts the attack animation and the onAttack event
    /// called by playerinput.cs
    /// </summary>
    public void attack()
    {
       // Debug.Log( "yo1");
        anim.SetTrigger("Attack");
        onAttack.Invoke();
    }
    /// <summary>
    /// DEPRECIATED to be removed 
    /// </summary>
    /// <param name="other"></param>
    public void damageCharacter(Character other)
    {
        if (isServer || test)
            other.updateHealth(-mainAttackDamage);

    }
    /// <summary>
    /// called by attacker
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="source"></param>
    public void TakeDamage(float amount,ICanDealDamage source)
    {
        Debug.Log("yo3");
        updateHealth(-amount);
       
    }

    /// <summary>
    /// called by melleeWeapon
    /// </summary>
    /// <param name="Target"></param>
    public void DealDamage(ICanTakeDamage Target)
    {
        if (isServer || test)
        {
            Debug.Log("yo2");


            Target.TakeDamage(mainAttackDamage,this);  }
    }
    /// <summary>
    /// Called by self
    /// </summary>
    /// <param name="val"></param>
    private void updateHealth(float val)
    {
        if(isServer || test)
        {
            Debug.Log("yo4");
            myHealth.updateHealth(val);
            if (val < 0)
            {
                OnDamageTaken.Invoke();

            }

        }
       
    }
    private void rotateChar()
    {
        if (!isLockedOn) //look in movement direction
        {
            if (moveDirection.magnitude > 0.1)
            {
               
                transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            }
        }
        else //look at target
        {
            
            if (LockTarget != null)
            {
                Vector3 lockTargetDir = LockTarget.position - this.mesh.transform.position;
                transform.rotation = Quaternion.LookRotation(lockTargetDir, Vector3.up);

            }
            else
            {
                isLockedOn = false;
            }

        }

    }
    void Update()
    {
        //character rotation
        rotateChar();
        //moveDirection setby playerInput.cs using setInput();
        moveDirection *= speed;

        //animation code
        anim.SetFloat("Forward", moveDirection.magnitude, 0.1f, Time.deltaTime);
        
        //gravity
        // moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

   
}
