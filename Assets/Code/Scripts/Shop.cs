using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCountText; // update coins

    // will probably need to set these from player pref first, update throughout shop scene, then update player pref
    [SerializeField] private int numTracks; // update number of tracks
    [SerializeField] private int crabDropRate; // moar crabz
    [SerializeField] private int cartQuality; // unlocked train cart qualities

    [SerializeField] private int trackPrice;
    [SerializeField] private TextMeshProUGUI trackPriceText;
    [SerializeField] private int crabPrice;
    [SerializeField] private TextMeshProUGUI crabPriceText;
    [SerializeField] private int cartPrice;
    [SerializeField] private TextMeshProUGUI cartPriceText;

    private int shopMenu; // 1 for decor/upgrade, 2 for decor menu, 3 for upgrade menu
    public GameObject ShopFns, Upgrades; 

    private void Awake()
    {
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
        // maybe a multiplier for each level of upgrade?
        trackPrice = 100;
        crabPrice = 100;
        cartPrice = 100;
    }
    void Start()
    {
        shopMenu = 1;
        ShopFns.SetActive(true);
        Upgrades.SetActive(false);
    }

    // switch to upgrade menu
    public void Upgrade()
    {
        shopMenu = 3;
        ShopFns.SetActive(false);
        Upgrades.SetActive(true);
        trackPriceText.text = trackPrice.ToString();
        crabPriceText.text = crabPrice.ToString();
        cartPriceText.text = cartPrice.ToString();
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
        if (PlayerPrefs.GetInt("coins") >= trackPrice && numTracks < 4) // max # of tracks
        {
            Purchase(trackPrice);
            numTracks++;
            // update price and text here?
            trackPriceText.text = (trackPrice * 2).ToString();
            Debug.Log(numTracks);
        }
    }

    public void Crabs()
    {
        if (PlayerPrefs.GetInt("coins") >= crabPrice && crabDropRate < 6) // cap???
        {
            Purchase(crabPrice);
            crabDropRate++;
            // update price and text here?
            crabPriceText.text = (crabPrice * 2).ToString();
            Debug.Log(crabDropRate);
        }
    }
    public void Carts()
    {
        if (PlayerPrefs.GetInt("coins") >= cartPrice  && cartQuality < 2) // 0 is standard, 1 is economy, 2 is deluxe
        {
            Purchase(cartPrice);
            cartQuality++;
            // update price and text here?
            cartPriceText.text = (cartPrice * 2).ToString();
            Debug.Log(cartPrice);
        }
    }
    public void Purchase(int price)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - price);
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
