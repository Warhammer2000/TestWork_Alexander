using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource buble_AudioSource;
    [SerializeField] private AudioSource snap_AudioSource;

    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<Button> favorite_buttons = new List<Button>();

    private void Start()
    {
        StartCoroutine(AssignButtonsWithInterval());
    }
    private IEnumerator AssignButtonsWithInterval()
    {
        while (true)
        {
            buttons.Clear();
            favorite_buttons.Clear();

            FindAndAssignButtons("ordinaryButton", buttons, snap_AudioSource);
            FindAndAssignButtons("Favorite_button", favorite_buttons, buble_AudioSource);

            yield return new WaitForSeconds(4f);
        }
    }
    private void FindAndAssignButtons(string tag, List<Button> buttonsList, AudioSource audioSource)
    {
        GameObject[] buttonObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var buttonObj in buttonObjects)
        {
            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                buttonsList.Add(btn);
                btn.onClick.AddListener(() => audioSource.Play());
            }
        }
    }
}

