using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfilePanel : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text emailText;
    [SerializeField] private Text genderText;
    [SerializeField] private Text ipAddressText;
    private Text phoneText;
    private string profileImageURL;

    [SerializeField] private Image favoriteIcon;
    [SerializeField] private Button favoriteButton;

    private Contact currentContact;
    private void Update()
    {
        UpdateButtonState();
    }
    private void Awake()
    {
        favoriteButton.onClick.AddListener(ProfileFavoriteButton);
    }
    public void UpdateUI(Contact contact, ContactCard card)
    {
        nameText.text = $"{contact.first_name} {contact.last_name}";
        emailText.text = $"Email: {contact.email}";

        genderText.text = $"Gender: {contact.gender}";
        ipAddressText.text = $"IP Address: {contact.ip_address}";

        profileImage.sprite = card.icon.sprite;
        if (FavoritesManager.Instance.favoriteContacts.Contains(currentContact))
        {
            favoriteIcon.sprite = FavoritesManager.favoriteOff;
        }
        else
        {
            favoriteIcon.sprite = FavoritesManager.favoriteOn;
        }
        currentContact = contact;
    }
    private void ProfileFavoriteButton()
    {
        Debug.Log(nameof(ProfileFavoriteButton));
        if (FavoritesManager.Instance.favoriteContacts.Contains(currentContact))
        {
            favoriteIcon.sprite = FavoritesManager.favoriteOff;
            FavoritesManager.Instance.RemoveFromFavorites(currentContact);
            Debug.Log("Removed From Favorite out of Profile");
        }
        else
        {
            favoriteIcon.sprite = FavoritesManager.favoriteOn;
            FavoritesManager.Instance.AddToFavorites(currentContact);
            Debug.Log("Added From Profile To Favorite");
        }
    }

    private void UpdateButtonState()
    {
        if (FavoritesManager.Instance.favoriteContacts.Contains(currentContact))
        {
            favoriteButton.gameObject.GetComponent<Image>().sprite = FavoritesManager.favoriteOn;
        }
        else
        {
            favoriteButton.gameObject.GetComponent<Image>().sprite = FavoritesManager.favoriteOff;
        }
    }
}
