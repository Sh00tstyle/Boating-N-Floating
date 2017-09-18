using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenuManager : MonoBehaviour {

    public EnemyManager enemyManager;

    private float _lastTimeStamp;

    private MenuManager _menuManager;

    private Image[] _hudImages;
    private Text[] _hudTexts;

    private enum HudImages { PanelBackground, ButtonBackground };
    private enum HudTexts { DeathText, ObjectiveProgressText, ObjectiveProgressStatus, ButtonText };

    private void Awake() {
        _menuManager = gameObject.GetComponentInParent<MenuManager>();

        _hudImages = gameObject.GetComponentsInChildren<Image>();
        _hudTexts = gameObject.GetComponentsInChildren<Text>();
    }

    public void Update() {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
            _menuManager.ActivateMenu(MenuManager.Menu.Hud);
        }
    }

    public void ActivateDeathScreen() {
        _hudTexts[(int)HudTexts.ObjectiveProgressStatus].text = (enemyManager.EnemyStartAmt - enemyManager.EnemyAmt) + "/" + enemyManager.EnemyStartAmt;
    }
}
