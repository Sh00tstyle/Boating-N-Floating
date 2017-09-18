using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject[] disableOnPauseObjs;

    public GameObject[] menus;

    public enum Menu { Start, Resolution, Death, Hud };

    private Menu _activeMenu;

    private bool _paused;

    public void Start() {
        ActivateMenu(Menu.Start); //enable again later
    }

    public void ActivateMenu(Menu targetMenu) {
        for (int i = 0; i < menus.Length; i++) {
            if ((int)targetMenu == i) {
                menus[i].SetActive(true);
                InitMenu(i);
            } else {
                menus[i].SetActive(false);
            }
        }

        _activeMenu = targetMenu;
    }

    private void InitMenu(int menu) {
        switch (menu) {
            case (int)Menu.Start:
                StartMenuManager startManager = menus[menu].GetComponent<StartMenuManager>();
                startManager.ActivateStartScreen();

                PauseObjects();
                break;

            case (int)Menu.Resolution:
                ResolutionMenuManager resolutionManager = menus[menu].GetComponent<ResolutionMenuManager>();
                resolutionManager.ActivateResolutionScreen();

                PauseGame();
                break;

            case (int)Menu.Death:
                DeathMenuManager deathManager = menus[menu].GetComponent<DeathMenuManager>();
                deathManager.ActivateDeathScreen();

                PauseGame();
                break;

            case (int)Menu.Hud:
                OverlayHudManager hudManager = menus[menu].GetComponent<OverlayHudManager>();
                hudManager.ActivateHud();

                ResumeGame();
                break;

            default:
                break;
        }
    }

    public void PauseGame() {
        _paused = true;
        Time.timeScale = 0f; //Pause

        PauseObjects();
    }

    public void PauseObjects() {
        for (int i = 0; i < disableOnPauseObjs.Length; i++) {
            disableOnPauseObjs[i].SetActive(false);
        }
    }

    public void ResumeGame() {
        _paused = false;
        Time.timeScale = 1f; //Resume

        ResumeObjects();
    }

    public void ResumeObjects() {
        for (int i = 0; i < disableOnPauseObjs.Length; i++) {
            disableOnPauseObjs[i].SetActive(true);
        }
    }

    public bool Paused {
        get { return _paused; }
    }

    public Menu ActiveMenu {
        get { return _activeMenu; }
    }
}
