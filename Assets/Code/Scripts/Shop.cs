using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText; // update coins

    // will probably need to set these from player pref first, update throughout shop scene, then update player pref
    [SerializeField] private int numTracks; // update number of tracks
    [SerializeField] private int crabDropRate; // moar crabz
    [SerializeField] private int cartQuality; // unlocked train cart qualities

    [Header("Upgrade Track")]
    [SerializeField] private int trackPrice;
    [SerializeField] private TextMeshProUGUI trackPriceText;
    [SerializeField] private GameObject trackUpgradePanel;

    [Header("Upgrade Crabs")]
    [SerializeField] private int crabPrice;
    [SerializeField] private TextMeshProUGUI crabPriceText;
    [SerializeField] private GameObject crabUpgradePanel;

    [Header("Upgrade Carts")]
    [SerializeField] private int cartPrice;
    [SerializeField] private TextMeshProUGUI cartPriceText;
    [SerializeField] private GameObject cartUpgradePanel;

    private int shopMenu; // 1 for decor/upgrade, 2 for decor menu, 3 for upgrade menu

    [Header("Other")]
    public GameObject ShopFns, Upgrades;
    [SerializeField] private bool debug;

    [SerializeField] private Color unavailable; // #BFBFBF

    private void Awake()
    {
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
        // maybe a multiplier for each level of upgrade?
    }
    void Start()
    {
        if (debug) // debug
        {
            PlayerPrefs.SetInt("coins", 2000);
            PlayerPrefs.SetInt("numTracks", 0);
            PlayerPrefs.SetInt("crabDropRate", 0);
            PlayerPrefs.SetInt("cartQuality", 0);
        }
        shopMenu = 1;
        ShopFns.SetActive(true);
        Upgrades.SetActive(false);
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();

        numTracks = PlayerPrefs.GetInt("numTracks");
        crabDropRate = PlayerPrefs.GetInt("crabDropRate");
        cartQuality = PlayerPrefs.GetInt("cartQuality");
        trackPrice = (int)(25 * (numTracks + 1)); //(Mathf.Pow(2f, (float)numTracks)));
        crabPrice = (int)(25 * (crabDropRate + 1)); //(Mathf.Pow(2f, (float)crabDropRate)));
        cartPrice = (int)(50 * (cartQuality + 1)); //(Mathf.Pow(2f, (float)crabDropRate)));

        CheckBlur();
    }

    // switch to upgrade menu
    public void Upgrade()
    {
        shopMenu = 3;
        ShopFns.SetActive(false);
        Upgrades.SetActive(true);
        if (numTracks != 3)
        {
            trackPriceText.text = trackPrice.ToString();
        }
        else
        {
            trackUpgradePanel.GetComponent<Button>().interactable = false;
        }

        if (crabDropRate != 3)
        {
            crabPriceText.text = crabPrice.ToString();
        }
        else
        {
            crabUpgradePanel.GetComponent<Button>().interactable = false;
        }

        if (cartQuality != 2)
        {
            cartPriceText.text = cartPrice.ToString();
        }
        else
        {
            cartUpgradePanel.GetComponent<Button>().interactable = false;
        }
        
    }

    // switch to decor menu
    public void Decor()
    {
        shopMenu = 2;
    }

    // switch to shop main menu
    public void ShopMain()
    {
        shopMenu = 1;
        ShopFns.SetActive(true);
    }

    // go back to main shop menu or kiosk (kiosk or main menu?)
    public void Back()
    {
        if (shopMenu == 1)
        {
            SceneManager.LoadScene("Temp");
        }
        else if (shopMenu == 2)
        {
            ShopMain();
        }
        else if (shopMenu == 3)
        {
            Upgrades.SetActive(false);
            ShopMain();
        }
    }

    public void Track()
    {
        //int coins = 2000;
        if (PlayerPrefs.GetInt("coins") >= trackPrice && numTracks < 3) // max # of tracks
        {
            Purchase(trackPrice);
            numTracks++;
            // update price and text here?
            PlayerPrefs.SetInt("numTracks", numTracks);
            trackPrice = (int)(25 * (numTracks + 1));
            trackPriceText.text = (trackPrice).ToString();
            //Debug.Log(numTracks);

            if (numTracks == 3)
            {
                ApplyBlur(trackUpgradePanel);
                trackPriceText.text = "";
                trackUpgradePanel.GetComponent<RectTransform>().Find("Text").Find("Desc").GetComponent<TMP_Text>().text = "Fully Upgraded";
                trackUpgradePanel.GetComponent<RectTransform>().Find("Images").Find("PriceIcon").gameObject.SetActive(false);
                trackUpgradePanel.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void Crabs()
    {
        if (PlayerPrefs.GetInt("coins") >= crabPrice && crabDropRate < 3) // cap???
        {
            Purchase(crabPrice);
            crabDropRate++;
            // update price and text here?
            PlayerPrefs.SetInt("crabDropRate", crabDropRate);
            crabPrice = (int)(25 * (crabDropRate + 1));
            crabPriceText.text = (crabPrice).ToString();
            //Debug.Log(crabDropRate);

            if (crabDropRate == 3)
            {
                ApplyBlur(crabUpgradePanel);
                crabPriceText.text = "";
                crabUpgradePanel.GetComponent<RectTransform>().Find("Text").Find("Desc").GetComponent<TMP_Text>().text = "Fully Upgraded";
                crabUpgradePanel.GetComponent<RectTransform>().Find("Images").Find("PriceIcon").gameObject.SetActive(false);
                crabUpgradePanel.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void Carts()
    {
        if (PlayerPrefs.GetInt("coins") >= cartPrice && cartQuality < 2) // 0 is economy, 1 is standard, 2 is deluxe
        {
            Purchase(cartPrice);
            cartQuality++;
            // update price and text here?
            PlayerPrefs.SetInt("cartQuality", cartQuality);
            cartPrice = (int)(50 * (cartQuality + 1));
            cartPriceText.text = (cartPrice).ToString();
            //Debug.Log(cartPrice);

            if (cartQuality == 2)
            {
                ApplyBlur(cartUpgradePanel);
                cartPriceText.text = "";
                cartUpgradePanel.GetComponent<RectTransform>().Find("Text").Find("Desc").GetComponent<TMP_Text>().text = "Fully Upgraded";
                cartUpgradePanel.GetComponent<RectTransform>().Find("Images").Find("PriceIcon").gameObject.SetActive(false);
                cartUpgradePanel.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void Purchase(int price)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - price);
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();

        CheckBlur();
    }

    private void CheckBlur()
    {
        if (PlayerPrefs.GetInt("coins") < trackPrice)
        {
            //blur
            ApplyBlur(trackUpgradePanel);
            trackUpgradePanel.GetComponent<Button>().interactable = false;

        }
        if (PlayerPrefs.GetInt("coins") < crabPrice)
        {
            // blur
            ApplyBlur(crabUpgradePanel);
            crabUpgradePanel.GetComponent<Button>().interactable = false;
        }
        if (PlayerPrefs.GetInt("coins") < cartPrice)
        {
            // blur
            ApplyBlur(cartUpgradePanel);
            cartUpgradePanel.GetComponent<Button>().interactable = false;
        }
    }

    private void ApplyBlur(GameObject upgrade)
    {
        Transform imageChild = upgrade.GetComponent<RectTransform>().Find("Images");
        Transform textChild = upgrade.GetComponent<RectTransform>().Find("Text");

        foreach (Transform image in imageChild)
        {
            image.gameObject.GetComponent<UnityEngine.UI.Image>().color = unavailable;
        }
        foreach (Transform text in textChild)
        {
            text.gameObject.GetComponent<TMP_Text>().color = unavailable;
        }
    }
}
