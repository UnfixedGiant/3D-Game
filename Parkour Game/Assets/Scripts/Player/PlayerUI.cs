using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    [Header("Prompt Message")]
    private TextMeshProUGUI promptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Updates the text to what the prompt message is.
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

}
