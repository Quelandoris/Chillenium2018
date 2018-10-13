using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour {

    public enum  Artifacts {test,swordofthesea,tentacleextention,icedaggers,bombs,mines,invisibilitycloak,sandguardian,fogfigurine,duplicitydisk,shiningshield,stunningspectacles,}; //list items here
    public Artifacts artifact = Artifacts.test; 
    public Artifacts[] inventory = new Artifacts[5];
    public int curInventory=0;
    public GameObject bomb;
    public Sprite TestSprite;


    void Update ()
    {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                switch (inventory[curInventory])
                {
                case Artifacts.test:
                    //Do a thing
                    break;
                case Artifacts.swordofthesea:
                    var curDirection = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    Physics2D.CircleCastAll(PLayerControls.transform.position, 3f, Vector2)
                    Vector3 vectorToCollider = (collider.transform.position - player.transform.position).Normalize();
                    // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
                    if (Vector3.Dot(vectorToCollider, curDirection)) > 0.5)
                       {
                        //Damage the enemy
                       }
                    break;
                case Artifacts.tentacleextention:
                    var curDirection = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    //extend+pull enemy
                    break;
                case Artifacts.icedaggers:
                    var curDirection = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    Physics2D.CircleCastAll(PLayerControls.transform.position, 3f, Vector2)
                    Vector3 vectorToCollider = (collider.transform.position - player.transform.position).Normalize();
                    // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
                    if (Vector3.Dot(vectorToCollider, curDirection)) > 0.5)
                       {
                        //Freeze the enemy for 6 seconds
                       }
                    break;
                case Artifacts.bombs:
                   Instantiate(Bomb)
                    //time activated
                    break;
                case Artifacts.mines:
                   //contact activated
                    break;
                case Artifacts.sandguardian:
                    //Do a thing
                    break;
                case Artifacts.fogfigurine:
                    //Do a thing
                    break;
                case Artifacts.duplicitydisk:
                    //Do a thing
                    break;
                case Artifacts.shiningshield:
                    //Do a thing
                    break;
                case Artifacts.stunningspectacles:
                    //Do a thing
                    break;
                default:
                        break;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                curInventory--;
                if (curInventory < 0) curInventory = 4;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                curInventory++;
                if (curInventory > 4) curInventory = 0;
            }
        }
}
