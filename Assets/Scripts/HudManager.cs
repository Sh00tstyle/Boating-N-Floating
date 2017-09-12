using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour {

    private ShipMovement _movement;
    private HealthScript _health;
    private ResourceManager _resources;

    private enum HudImages { Compass, HealthEmpty, HealthFull, SpeedEmpty, SpeedFull, BombIcon };
    private enum HudTexts { BombCounter };

    private Image[] _images;
    private Text[] _texts;

    public void Awake() {
        _movement = GetComponentInParent<ShipMovement>();
        _health = GetComponentInParent<HealthScript>();
        _resources = GetComponentInParent<ResourceManager>();

        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<Text>();
    }

    public void Update() {
        DrawSpeed();
        DrawHealth();
        DrawBombAmount();
    }

    public void DrawSpeed() {
        _images[(int)HudImages.SpeedFull].fillAmount = _movement.CurrentSpeed / _movement.maxSpeed;
    }

    public void DrawHealth() {
        _images[(int)HudImages.HealthFull].fillAmount = _health.Health / _health.StartingHealth;
    }

    public void DrawBombAmount() {
        _texts[(int)HudTexts.BombCounter].text = "" + _resources.BombAmount;
    }
}
