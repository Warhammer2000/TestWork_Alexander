using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ContactCard : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private Button favoriteButton;
    [SerializeField] private Text nameText;
    [SerializeField] private Text emailText;
    [SerializeField] private Text phoneText;
    [SerializeField] private Text ipAddressText;
    [SerializeField] private Sprite placeholderSprite;
    [HideInInspector] public Image icon;
    private Text genderText;

    [Header("Other Settings")]

    [SerializeField] private Canvas _canvasProfile;
    [SerializeField] private FavoritesManager favoritesManager;

    private Contact contactData;
    private ContactViewModel _contactViewModel;

    [HideInInspector]
    public Contact contact;

    private string pendingImageUrl;
    private string pendingLocalPath;
    private void Start()
    {
        Initialize();
        transform.localScale = new Vector3(1,1,1);
    }
    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(pendingImageUrl))
        {
            StartCoroutine(LoadImage(pendingImageUrl, pendingLocalPath));
            pendingImageUrl = null;
            pendingLocalPath = null;
        }
    }
    private void Update()
    {
        UpdateButtonState();
    }
    private void UpdateButtonState()
    {
        if (FavoritesManager.Instance.favoriteContacts.Contains(contactData))
        {
            favoriteButton.gameObject.GetComponent<Image>().sprite = FavoritesManager.favoriteOn;
        }
        else
        {
            favoriteButton.gameObject.GetComponent<Image>().sprite = FavoritesManager.favoriteOff; 
        }
    }
    private void Initialize()
    {
        _canvasProfile = GameObject.Find("CanvasProfile").GetComponent<Canvas>();
        _contactViewModel = FindObjectOfType<ContactViewModel>();

        favoritesManager = GameObject.Find("Favorite").GetComponent<FavoritesManager>();
        favoriteButton.onClick.AddListener(OnAddToFavoritesButtonClicked);
    }
    public void OnButtonClick()
    {
        if (_contactViewModel != null)
        {
            _canvasProfile.sortingOrder = 1;

            _contactViewModel.SetCurrentContact(contactData, this);
        }
    }
    public  void SetupCard(Contact contact)
    {
        contactData = contact;
        this.contact = contact;

        nameText.text = $"{contact.first_name} {contact.last_name}";
        emailText.text = $"Email: {contact.email}";

        //genderText.text = $"Gender: {contact.gender}";
        ipAddressText.text = $"IP Address: {contact.ip_address}";

        string imageUrl = GetImageUrl(contact);
        string localFileName = GenerateLocalFileName(contact);

        string localPath = Path.Combine(Application.persistentDataPath, localFileName);
        TryLoadImage(imageUrl, localPath);
    }
    private void TryLoadImage(string imageUrl, string localPath)
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(LoadImage(imageUrl, localPath));
        }
        else
        {
            pendingImageUrl = imageUrl;
            pendingLocalPath = localPath;
        }
    }
    private IEnumerator LoadImage(string imageUrl, string localPath)
    {
        if (File.Exists(localPath))
        {
            byte[] imageBytes = File.ReadAllBytes(localPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            icon.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                icon.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                byte[] imageBytes = texture.EncodeToPNG();
                File.WriteAllBytes(localPath, imageBytes);
            }
            else
            {
                Debug.LogError($"Failed to load image from {imageUrl}\nError: {uwr.error}");
                icon.sprite = placeholderSprite;
            }
        }
    }
    private string GetImageUrl(Contact contact)
    {
        return "https://picsum.photos/200";
    }
    public void OnAddToFavoritesButtonClicked()
    {
        if (FavoritesManager.Instance.favoriteContacts.Contains(contact))
        {
            FavoritesManager.Instance.RemoveFromFavorites(contact);
        }
        else
        {
            FavoritesManager.Instance.AddToFavorites(contact);
        }
    }
    private string GenerateLocalFileName(Contact contact)
    {
        return $"contact_{contact.id}.png";
    }
}
