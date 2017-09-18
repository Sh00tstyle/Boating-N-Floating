using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionMenuManager : MonoBehaviour {

    private float _lastTimeStamp;

    private MenuManager _menuManager;

    private Image[] _hudImages;
    private Text[] _hudTexts;

    private enum HudImages { PanelBackground, ButtonBackground };
    private enum HudTexts { ObjectiveText, CompletionTimeText, CompletionTimeAmount, BootyPointsText, BootyPointsAmount, ButtonText };

    private void Awake() {
        _menuManager = gameObject.GetComponentInParent<MenuManager>();

        _hudImages = gameObject.GetComponentsInChildren<Image>();
        _hudTexts = gameObject.GetComponentsInChildren<Text>();
    }

    public void Update() {
        if(gameObject.activeSelf && Input.GetKeyDown(KeyCode.U)) {
            _lastTimeStamp = Time.time;
            _menuManager.ActivateMenu(MenuManager.Menu.Hud);

            //Reload level and spawn everything
            print("Reenter playmode to restart please!!");
        }
    }

    public void ActivateResolutionScreen() {
        _hudTexts[(int)HudTexts.CompletionTimeAmount].text = (Time.time - _lastTimeStamp) + "s";

        _hudTexts[(int)HudTexts.BootyPointsAmount].text = (Time.time - _lastTimeStamp) / 60f + "pts";
    }
}
