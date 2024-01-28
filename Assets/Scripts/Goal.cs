using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{

    public GameObject player;
    private PlayerController playerController;
    public GameObject resultsPanel;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LoseText;
    public TextMeshProUGUI PercentText;
    public GameObject RetryButton;
    public GameObject QuitButton;
    public GameObject ContinueButton;
    public GameObject retryPanel;
    public ItemController.ItemType itemWanted;

    int burgers;
    int vegetables;
    int cookies;
    int soap;
    int drinks;
    int pasta;
    int batteries;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.currentState == PlayerController.PlayerStates.Dead)
        {
            retryPanel.SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player") && !resultsPanel.activeSelf)
        {
            //activate the results screen
            CalculateTotalItems();
            resultsPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CalculateTotalItems()
    {
        //
        int bagCount = playerController.bag.Count;
        int total = 0;

        while(playerController.bag.Count > 0)
        {
            ItemController curItem = playerController.bag.Pop().GetComponent<ItemController>();
            if (itemWanted == curItem.itemType)
            {
                total++;
            }
            
        }

        float result = total / (float)bagCount;
        Debug.Log(result);
        PercentText.text = "You delivered " + result.ToString("P1") + " " + itemWanted.ToString();
        if(result < 0.5f)
        {
            WinText.gameObject.SetActive(false);
            LoseText.gameObject.SetActive(true);
        }
        else
        {
            WinText.gameObject.SetActive(true);
            LoseText.gameObject.SetActive(false);
            ContinueButton.SetActive(true);
        }
    }

    public void WinGame()
    {
        SceneManager.LoadScene("Ending");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("Main Game");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
