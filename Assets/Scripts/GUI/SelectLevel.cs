using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class SelectLevel : MonoBehaviour
{
    [SerializeField] private GameObject[] _levelButtons;
    [SerializeField] private Sprite _lockedSprite;

    private void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            Level temp = (Level)SaveLoad._savedGame.Levels[$"Level{i}"];
            if (!temp._unlocked)
            {
                _levelButtons[i].GetComponentsInChildren<Image>()[1].sprite = _lockedSprite;
                _levelButtons[i].GetComponentsInChildren<Image>()[1].color = Color.white;
                _levelButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        
    }
    public void LoadLevel(int level)
    {
        if(level != 0)
            FindObjectOfType<SceneLoader>().LoadScene($"Level{level}Scene");
        else
            FindObjectOfType<SceneLoader>().LoadScene($"TutorialScene");
    }
}
