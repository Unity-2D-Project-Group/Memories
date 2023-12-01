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
        if (collision.gameObject.tag == "Player" && _active)
        {;
            PetController pet = GameObject.FindGameObjectWithTag("Pet").GetComponent<PetController>();

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
