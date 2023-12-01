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
        PetController pet = null;
        if (collision.gameObject.tag == "Player" && _active)
        {
            foreach(GameObject temp in GameObject.FindGameObjectsWithTag("Pet"))
            {
                if(temp.GetComponent<PetController>() != null)
                {
                    pet = temp.GetComponent<PetController>();
                }
            }

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
