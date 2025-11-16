using System.Collections;
using System.Diagnostics;
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

    [SerializeField] private bool debug;

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
        trackPrice = (int)(100 * (Mathf.Pow(2f, (float)numTracks)));
        crabPrice = (int)(100 * (Mathf.Pow(2f, (float)crabDropRate)));
        cartPrice = (int)(100 * (Mathf.Pow(2f, (float)crabDropRate)));
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
        if (PlayerPrefs.GetInt("coins") >= trackPrice && numTracks < 3) // max # of tracks
        {
            Purchase(trackPrice);
            numTracks++;
            // update price and text here?
            PlayerPrefs.SetInt("numTracks", numTracks);
            trackPrice = (int)(100 * (Mathf.Pow(2f, (float)numTracks)));
            trackPriceText.text = (trackPrice).ToString();
            //Debug.Log(numTracks);
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
            crabPrice = (int)(100 * (Mathf.Pow(2f, (float)crabDropRate)));
            crabPriceText.text = (crabPrice).ToString();
            //Debug.Log(crabDropRate);
        }
    }
    public void Carts()
    {
        if (PlayerPrefs.GetInt("coins") >= cartPrice  && cartQuality < 2) // 0 is economy, 1 is standard, 2 is deluxe
        {
            Purchase(cartPrice);
            cartQuality++;
            // update price and text here?
            PlayerPrefs.SetInt("cartQuality", cartQuality);
            cartPrice = (int)(100 * (Mathf.Pow(2f, (float)cartQuality)));
            cartPriceText.text = (cartPrice).ToString();
            //Debug.Log(cartPrice);
        }
    }
    public void Purchase(int price)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - price);
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
