using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class FavoritesManager : MonoBehaviour
{
    public static FavoritesManager Instance;

    public List<Contact> favoriteContacts = new List<Contact>();
    public GameObject favoritesPanel;
    [SerializeField] private ObjectPool favoritesObjectPool;

     public static Sprite favoriteOn;
     public static Sprite favoriteOff;

    public delegate void FavoritesChanged();
    public event FavoritesChanged OnFavoritesChanged;
    private void Awake()
    {
        Download_Sprites_From_Resources();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Download_Sprites_From_Resources()
    {
        favoriteOn = Resources.Load<Sprite>("Icons/profile-favorin-on-icon");
        favoriteOff = Resources.Load<Sprite>("Icons/profile-favorin-off-icon");

        Debug.Log(favoriteOn + ", " + favoriteOff);
    }
    public void AddToFavorites(Contact contact)
    {
        if (!favoriteContacts.Contains(contact))
        {
            OnFavoritesChanged?.Invoke();
            favoriteContacts.Add(contact);
            GameObject card = ObjectPool.SharedInstance.GetPooledObject(true);
            SetupFavoriteCard(card, contact);
        }
    }
    private void SetupFavoriteCard(GameObject card, Contact contact)
    {
        if (card != null)
        {
            card.SetActive(true);
            card.transform.SetParent(favoritesPanel.transform, false);
            card.GetComponent<ContactCard>().contact = contact;
            card.GetComponent<ContactCard>().SetupCard(contact);
            card.transform.GetChild(0).GetComponent<Image>().sprite = favoriteOn;
        }
    }
    public void RemoveFromFavorites(Contact contact)
    {
        if (favoriteContacts.Contains(contact))
        {
            foreach (var card in favoritesObjectPool.objectsBelow)
            {
                ContactCard contactCard = card.GetComponent<ContactCard>();
                if (contactCard != null && contactCard.contact.Equals(contact))
                {
                    card.SetActive(false);
                    break;
                }
            }
            favoriteContacts.Remove(contact);
            OnFavoritesChanged?.Invoke();
        }
    }
}
