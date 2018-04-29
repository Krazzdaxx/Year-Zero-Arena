using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Hero", menuName = "Character", order = 1)]
public class CharacterStats : ScriptableObject {

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float mainAttackDamage = 10;
    public Transform Prefab;
    public Transform CharSelectPic;

}
