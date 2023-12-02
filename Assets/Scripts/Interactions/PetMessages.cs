using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMessages : MonoBehaviour
{
    private bool _active = true;
    [SerializeField] private string _happyMessage;
    [SerializeField] private string _angryMessage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it detects the player, then the pet interact with it
        if (collision.gameObject.tag == "Player" && _active)
        {
            //Get the pet
            PetController pet = null;
            foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Pet"))
            {
                if(temp.GetComponent<PetController>() != null)
                {
                    pet = temp.GetComponent<PetController>();
                }
            }

            //If the pet isn't already interacting with the player, then it is called
            if (!pet._interacting)
            {
                pet._interacting = true;
                if(pet._petHumor == PetController.PetHumor.Happy)
                    StartCoroutine(pet.Type(_happyMessage, "Pet"));
                else
                    StartCoroutine(pet.Type(_angryMessage, "Pet"));
                pet._interacting = false;
            }

            _active = false;
        }
    }
}
