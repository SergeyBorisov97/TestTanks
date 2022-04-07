using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private int tanksDestroyed;

    public Text tanksDestroyedText;

    //public Text positionText;

    public PlayerTankController player;

    void Start()
    {
        player = GetComponent<PlayerTankController>();
    }

    void Update()
    {
        UpdateDestroyedTanks();

        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        //positionText.text = "Player Position: " + ((Vector2)(player.gameObject.transform.position)).ToString();
    }

    void UpdateDestroyedTanks()
	{
        tanksDestroyedText.text = string.Format("Tanks destroyed: {0}", player.enemiesDestroyed);
    }
}