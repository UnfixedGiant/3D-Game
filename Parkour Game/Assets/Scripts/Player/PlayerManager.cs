using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Singleton makes sure that there is only one instance of the player manager in the game.
    #region Singleton
    public static PlayerManager instance;
    void awake ()
    {
        instance = this;
    }

    #endregion

    public GameObject Player;
}
