using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MonoBehaviour {

    public enum HudImages { };
    public enum HudTexts { GameTitle, InsertCoinText };

    private MenuManager _menuManager;

    public void Awake() {
        _menuManager = GetComponentInParent<MenuManager>();
    }

    public void ActivateStartScreen() {
        //Nothing to init
    }

    public void Update() {
        if(gameObject.activeSelf) AnimateInsertCoin();

        if (Input.GetKeyDown(KeyCode.W)) _menuManager.ActivateMenu(MenuManager.Menu.Hud);
    }

    public void AnimateInsertCoin() {
        //TODO
    }
}
