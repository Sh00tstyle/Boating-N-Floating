using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour {

    public GameObject player;
    public GameObject upgradeContainer;
    public Text goldUi;

    private Canvas _upgradeCanvas;
    private Image[] _upgradeImages;
    private Outline[] _upgradeOutlines;
    private Text[] _upgradeStatus;
    private UpgradeInfo[] _upgradeInfos;

    private int _verticalIndex;
    private int _horizontalIndex;

    private Color _colorGreen;

    private ResourceManager _resourceManager;

    public void Awake() {
        _upgradeCanvas = GetComponent<Canvas>();
        _upgradeCanvas.enabled = false;

        _upgradeImages = GetComponentsInChildren<Image>();
        _upgradeOutlines = GetComponentsInChildren<Outline>();
        _upgradeStatus = upgradeContainer.GetComponentsInChildren<Text>();
        _upgradeInfos = upgradeContainer.GetComponentsInChildren<UpgradeInfo>();

        _colorGreen = new Color(0f, 148f / 255f, 7f / 255f);
        _resourceManager = player.GetComponent<ResourceManager>();

        DrawCosts();
    }

    public void Update() {
        if(_upgradeCanvas.enabled == true) {
            ProcessUpgradeSelection();
        }
    }

    public void ActivateUpgradeWindow() {
        _upgradeCanvas.enabled = true;

        _verticalIndex = 0;
        _horizontalIndex = 0;

        DrawGoldAmount();
    }

    private void ProcessUpgradeSelection() {
        if (Input.GetKeyDown(KeyCode.W)) _verticalIndex -= 3;
        if (Input.GetKeyDown(KeyCode.S)) _verticalIndex += 3;
        if (Input.GetKeyDown(KeyCode.A)) _horizontalIndex--;
        if (Input.GetKeyDown(KeyCode.D)) _horizontalIndex++;

        if (_horizontalIndex > 2) _horizontalIndex = 0;
        if (_horizontalIndex < 0) _horizontalIndex = 2;

        if (_verticalIndex > 3) _verticalIndex = 0;
        if (_verticalIndex < 0) _verticalIndex = 3;

        for(int i = 0; i < _upgradeOutlines.Length; i++) {
            if (i == _verticalIndex + _horizontalIndex) _upgradeOutlines[i].enabled = true;
            else _upgradeOutlines[i].enabled = false;
        }

        if(Input.GetKeyDown(KeyCode.U)) {
            PurchaseUpgrade();
        }
    }

    private void DrawCosts() {
        for(int i = 0; i < _upgradeStatus.Length; i++) {
            _upgradeStatus[i].text = "" + _upgradeInfos[i].goldCost;
            _upgradeStatus[i].color = Color.black;
        }
    }

    private void PurchaseUpgrade() {
        int index = _verticalIndex + _horizontalIndex;

        UpgradeInfo info = _upgradeInfos[index];

        if(!_resourceManager.SpendGold(info.goldCost)) return;
        info.ActivateUpgrade(player);

        _upgradeStatus[index].text = "Purchased";
        _upgradeStatus[index].color = _colorGreen;

        DrawGoldAmount();
    }

    private void DrawGoldAmount() {
        goldUi.text = "" + _resourceManager.GoldAmount;
    }
}
