using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHudManager : MonoBehaviour {

    public Transform rotator;
    public Transform destination;

    public float trackingDistance;

    private ShipMovement _movement;
    private HealthScript _health;

    private enum HudImages { ObjectiveIndicatorNoArrow, ObjectiveIndicatorArrow, HealthEmpty, HealthRegen, HealthFull, SpeedEmpty, SpeedFull, IndicatorRight, IndicatorLeft };
    private enum HudTexts { };

    private Image[] _images;
    private Text[] _texts;

    public void Awake() {
        _movement = GetComponentInParent<ShipMovement>();
        _health = GetComponentInParent<HealthScript>();

        _images = GetComponentsInChildren<Image>();
        _texts = GetComponentsInChildren<Text>();
    }

    public void Update() {
        DrawSpeed();
        DrawHealth();
        DrawObjectiveIndicator();
    }

    public void DrawSpeed() {
        _images[(int)HudImages.SpeedFull].fillAmount = _movement.CurrentSpeed / _movement.maxSpeed;
    }

    public void DrawHealth() {
        _images[(int)HudImages.HealthFull].fillAmount = _health.Health / _health.StartingHealth;
    }

    public void DrawObjectiveIndicator() {
        if (destination == null) return;

        Vector3 fakeDestination = new Vector3(destination.position.x, transform.parent.position.y, destination.position.z); //Y axis has to be on the ships Y-pos
        Vector3 deltaVec = destination.position - transform.parent.position;

        if (deltaVec.magnitude > trackingDistance) {
            _images[(int)HudImages.ObjectiveIndicatorArrow].enabled = true;

            rotator.transform.LookAt(destination);
        } else {
            _images[(int)HudImages.ObjectiveIndicatorArrow].enabled = false;
        }
    }

    public void ActivateIndicator(bool isRight) {
        if (isRight) _images[(int)HudImages.IndicatorRight].enabled = true;
        else _images[(int)HudImages.IndicatorLeft].enabled = true;
    }

    public void DisableIndicator(bool isRight) {
        try {
            if (isRight) _images[(int)HudImages.IndicatorRight].enabled = false;
            else _images[(int)HudImages.IndicatorLeft].enabled = false;
        } catch {}
    }
}
