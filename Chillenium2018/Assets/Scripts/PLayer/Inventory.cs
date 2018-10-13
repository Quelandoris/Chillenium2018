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
    public GameObject sandguardian;
    public Sprite TestSprite;
    private Rigidbody2D myRB;

    private void Start()
    {
        myRB = GetComponent<Rigidbody2D>();   
    }


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
                    /*var curDirection = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    RaycastHit2D[] circlehit = Physics2D.CircleCastAll(transform.position, 3f, curDirection);
                    foreach (RaycastHit2D ray in circlehits3)
                    Vector3 vectorToCollider = (collider.transform.position - player.transform.position).Normalize();
                    // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
                    if (Vector3.Dot(vectorToCollider, curDirection)) > 0.5)
                       {
                        //Damage the enemy
                       }
                    break;
                case Artifacts.tentacleextention:
                    var curDirection2 = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    //extend+pull enemy
                    break;
                case Artifacts.icedaggers:
                    /*var curDirection3 = transform.InverseTransformDirection(myRB.velocity) + transform.position;
                    RaycastHit2D[] circlehits3 = Physics2D.CircleCastAll(transform.position, 3f, curDirection3);
                    foreach (RaycastHit2D ray in circlehits3)
                    Vector3 vectorToCollider3 = (collider.transform.position - player.transform.position).Normalize();
                    // 180 degree arc, change 0 to 0.5 for a 90 degree "pie"
                    if (Vector3.Dot(vectorToCollider3, curDirection3)) > 0.5)
                       {
                        //Freeze the enemy for 6 seconds
                       }*/
                    break;
                case Artifacts.bombs:
                    Instantiate(bomb, transform.position, transform.rotation);
                    //time activated
                    break;
                case Artifacts.mines:
                   //contact activated
                    break;
                case Artifacts.sandguardian:
                    Instantiate(sandguardian, transform.position, transform.rotation);
                    break;
                case Artifacts.fogfigurine:
                    //Do a thing
                    break;
                case Artifacts.duplicitydisk:
                    //creates decoy
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
