using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
    private static int _sceneCounter = 0;

    private AsyncOperation _backgroundLevel;

    public EnemyManager enemyManager;
    public MenuManager menuManager;

    public GameObject player;

    public void Start() {
        _sceneCounter++;

        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings) StartCoroutine("LoadLevel");
    }

    public void Update() {
        if(enemyManager.EnemyAmt == 0 && menuManager.ActiveMenu == MenuManager.Menu.Hud) {
            menuManager.ActivateMenu(MenuManager.Menu.Resolution);
        }
    }

    IEnumerator LoadLevel() {
        Debug.LogWarning("ASYNC LOAD STARTED -" + "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        _backgroundLevel = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        _backgroundLevel.allowSceneActivation = false;

        yield return _backgroundLevel;
    }

    public void ActivateLoadedScene() {
        _backgroundLevel.allowSceneActivation = true;
    }
}
