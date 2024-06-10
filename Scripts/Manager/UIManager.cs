using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public GameObject gameClearPanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
    }

    // ���� Ŭ���� �� UIȰ��ȭ
    public void GameClear()
    {
        gameClearPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    // ���� ���� �� UIȰ��ȭ
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }
}
