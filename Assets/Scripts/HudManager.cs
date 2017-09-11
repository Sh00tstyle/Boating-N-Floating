using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    private enum UiTexts { HEALTH_AMOUNT };
    private enum UiImages { };

    private Text[] _uiTexts;
    private Image[] _uiImages;

    public void Awake() {
        _uiTexts = GetComponentsInChildren<Text>();
        _uiImages = GetComponentsInChildren<Image>();
    }

    public void DrawHealth(float amount) {
        _uiTexts[(int)UiTexts.HEALTH_AMOUNT].text = "HP: " + amount;
    }
}