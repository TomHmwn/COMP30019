using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private Droppable[] itemList; // Stores the game items
    private int itemNum; // Selects a number to choose from the itemList
    private int randNum; // chooses a random number to see if loot os dropped- Loot chance
    private Transform Epos; // enemy position


   
 
    private void Start()
    {
       // itemList = CamFollow.itemListPass;
        Epos = GetComponent<Transform>();
        // Debug.Log(itemList);

    }
    private void Update() {

    }
 
    public void DropItem()
    {
        randNum = Random.Range(0, 101); // 100% total for determining loot chance;
        Debug.Log("Random Number is " + randNum);

        itemNum=-1;
        if (randNum > 45 && randNum <= 50) // Health Heart drop itemList[0] currently
        {
            itemNum = 0;
        }
        else if (randNum > 95 ) // MachineGun
        {

            itemNum = 1;
        }
        else if (randNum > 80 && randNum <= 85)// handGun
        {

            itemNum = 2;
        }
        else if (randNum > 70 && randNum <= 75)// Shotgun
        {

            itemNum = 3;
        }
        else if(randNum > 50 && randNum <= 60){
            itemNum = 4;
        }
        if(itemNum==-1)return;
        Instantiate(itemList[itemNum], Epos.position + (Vector3.up * 0.01f), Quaternion.identity);
    }// End of drop item

}
