using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prayed : MonoBehaviour
{
    // Start is called before the first frame update
    public int god_ID;
    public void PrayWoodSword() {
        GameObject.Find("GameManager").GetComponent<renment>().blessing(god_ID);
    }

}
