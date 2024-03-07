using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    //Items
    private GameObject[] itemList;
    private int totalItems;
    private int collectedItems = 0;

    private void Awake()
    {
        itemList = GameObject.FindGameObjectsWithTag("Item");
        totalItems = itemList.Length;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            collectedItems++;
            other.gameObject.SetActive(false);
            if (collectedItems == totalItems)
            {
                Debug.Log("Game Won");
            }
        }
    }
}
