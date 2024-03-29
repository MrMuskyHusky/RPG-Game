﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray interactionRay;
            interactionRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo;
            if(Physics.Raycast(interactionRay, out hitInfo, 10))
            {
                switch (hitInfo.collider.tag)
                {
                    case "NPC":
                        Debug.Log("Talk to NPC");
                        break;

                    case "Item":
                        Debug.Log("Pick up Item");
                        break;
                }

                /*if (hitInfo.collider.CompareTag("NPC"))
                {
                    Debug.Log("Talk to NPC is Triggered");
                }
                if(hitInfo.collider.tag == "Item")
                {
                    Debug.Log("Pick up Item");
                }*/
            }
        }
    }
}
