using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
   /// <summary>
   /// Test Script for Networking!!!!
   /// </summary>

    //speed variable
    public float speed = 5f;

   
    void Update()
    {
       //if this object is only in the owner then he can move
        if (!IsOwner) return;


        //get input axis 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //movement vector
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement.Normalize(); 

        //move the object
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        
    }
}