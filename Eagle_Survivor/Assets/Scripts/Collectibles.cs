using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    //Items
    public GameObject[] itemList;
    public int totalItems;
    public int collectedItems = 0;

    private EagleController eagleController;

    private void Awake()
    {
        eagleController = GetComponent<EagleController>();
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
        Debug.Log("Trigger with " + other.gameObject.name);
        if (other.gameObject.tag == "Item")
        {
            collectedItems++;


            if (collectedItems == totalItems)
            {
                Debug.Log("Game Won");
            }
        }
        if (other.gameObject.tag == "Food")
        {
            Debug.Log("Collect food");
            eagleController.timeRemaining += 5.0f;
            other.gameObject.SetActive(false);
        }
    }
}
