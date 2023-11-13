using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
   public Transform interactableTransform;
   [SerializeField]public IInteractable interactable;

   public bool activated = false;

   private void OnEnable()
   {
      interactable = interactableTransform.GetComponentInChildren<IInteractable>();
   }

   public void OnTriggerEnter(Collider other)
   {
      if (other.GetComponent<IPlayer>() != null)
      {
         activated = true;
         interactable.Interact();
      }
   }
   
   public void OnTriggerExit(Collider other)
   {
      if (other.GetComponent<IPlayer>() != null)
      {
         activated = false;
         interactable.Interact();
      }
   }

   void Update()
   {
      Color color;
      
      if(activated)
         color = Color.green;
      else
      {
         color = Color.red;
      }
      Debug.DrawLine(transform.position, interactableTransform.position, color);
   }
}
