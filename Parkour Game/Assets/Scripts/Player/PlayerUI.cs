using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    [Header("Prompt Message")]
    private TextMeshProUGUI promptText;

    [Header("Coins")]
    [SerializeField] // Add this attribute
    private TextMeshProUGUI coinText;
    public float coins;

    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Updates the text.
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
        coinText.text = "Coins: " + player.coins.ToString();
    }

}
