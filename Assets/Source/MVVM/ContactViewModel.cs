using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactViewModel : MonoBehaviour
{
    public static event Action<Contact, ContactCard> OnContactSelected;
    private ProfilePanel profilePanel; 


    private void Awake()
    {
        profilePanel = GameObject.Find("Profile").GetComponent<ProfilePanel>();
    }
    public void SetCurrentContact(Contact contact, ContactCard card)
    {
        if (OnContactSelected != null)
        {
            OnContactSelected(contact, card);
        }
    }
    private void OnEnable()
    {
        ContactViewModel.OnContactSelected += UpdateUI;
    }

    private void OnDisable()
    {
        ContactViewModel.OnContactSelected -= UpdateUI;
    }

    private void UpdateUI(Contact contact, ContactCard card)
    {
        profilePanel.UpdateUI(contact, card);
    }
}
