using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactsManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private TextAsset jsonTextFile;
    [SerializeField] private Transform contentPanel;
    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private List<GameObject> cardPool = new List<GameObject>();
    [SerializeField] private int poolSize = 15;
    private ContactsList contactsList;
    public int currentStartIndex = 0;

    private float viewportHeight = 1563.1f;
    private float cardHeight = 71;
    private int dataIndex;

    private int totalDataCount;
    private float totalScrollableHeight;
    private float currentScrollPosition;
    private float scrollValue;
   
    private int firstVisibleIndex;
    private int lastVisibleIndex;
    private float contentY;
    private int totalCards;
    private int visibleCardCount;
    void Awake()
    {
        LoadContactsData();
        InitializeCardPool();
    }
    private void Update()
    {
        UpdateCardVisibility();
    }
    void Start()
    {
        UpdateCardVisibility();
    }

    private void LoadContactsData()
    {
        contactsList = JsonUtility.FromJson<ContactsList>(jsonTextFile.text);
    }

    private void InitializeCardPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject card = Instantiate(cardPrefab, contentPanel);
            card.SetActive(false);
            cardPool.Add(card);
        }
    }
    private void UpdateCardVisibility()
    {
        float normalizedPosition = scrollRect.verticalNormalizedPosition; 
        viewportHeight = scrollRect.viewport.rect.height;

         visibleCardCount = Mathf.CeilToInt(viewportHeight / cardHeight) + 131;

        totalCards = contactsList.data.Count;
        firstVisibleIndex = Mathf.RoundToInt((1f - normalizedPosition) * (totalCards - visibleCardCount));
        firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, totalCards - visibleCardCount);
        lastVisibleIndex = firstVisibleIndex + visibleCardCount;

        for (int i = 0; i < cardPool.Count; i++)
        {
            GameObject card = cardPool[i];
            bool shouldBeVisible = i >= firstVisibleIndex && i < lastVisibleIndex;

            if (shouldBeVisible)
            {
                if (!card.activeSelf) card.SetActive(true);
                Contact contact = contactsList.data[i];
                card.GetComponent<ContactCard>().SetupCard(contact);
            }
            else
            {
                if (card.activeSelf) card.SetActive(false);
            }
        }
    }
}
