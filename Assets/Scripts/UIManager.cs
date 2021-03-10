using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TowerButton towerButtonPrefab;
    private static ScriptableTower[] scriptableTowers;
    private static TowerButton[] towerButtons;
    [SerializeField] private Transform buttonStart;
    [SerializeField] public TextMeshProUGUI moneyText, timeText;
    public static TowerButton currentlySelectedTowerButton;
    public static bool isPointerOnUI;
    // Singleton
    public static UIManager Instance;

    private void OnValidate()
    {
        buttonStart = transform.Find("ButtonStart");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        // Etsitään kaikki ScriptableTower -tyyppiset assetit hakemistosta
        // Resources/Towers/
        scriptableTowers = Resources.LoadAll<ScriptableTower>("Towers");
        // Luodaan jokaista tornia varten oma nappulansa
        towerButtons = new TowerButton[scriptableTowers.Length];
        float xPosition = buttonStart.GetComponent<RectTransform>().anchoredPosition.x;
        float yPosition = buttonStart.GetComponent<RectTransform>().anchoredPosition.y;
        float spacing = 100f;
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i] = Instantiate(towerButtonPrefab, transform);
            towerButtons[i].towerName.text = scriptableTowers[i].towerName;
            towerButtons[i].towerCost.text = $"${scriptableTowers[i].towerCost}";
            towerButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPosition, yPosition - (i * spacing));
        }
    }

    private void Update()
    {
        isPointerOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    public void UpdateTimeText(float time)
    {
        timeText.text = $"{time:00}";
    }

    private static ScriptableTower GetTower(TowerButton button)
    {
        for (int i = 0; i < scriptableTowers.Length; i++)
        {
            if (button == towerButtons[i])
            {
                return scriptableTowers[i];
            }
        }
        Debug.Log("Tower not found!");
        return null;
    }

    public static void SetCurrentlySelectedTowerButton(TowerButton button)
    {
        // Tarkistetaan, onko meillä jo nappula valittuna. Muutetaan nappulan
        // väri takaisin normaaliksi, jos on.
        if (currentlySelectedTowerButton != null)
        {
            currentlySelectedTowerButton.image.color = currentlySelectedTowerButton.colors.normalColor;
        }
        button.image.color = button.colors.selectedColor;
        currentlySelectedTowerButton = button;
        BuildingController.currentlySelectedTower = GetTower(button);
    }
  
    public void UpdateMoneyUI()
    {
        moneyText.text = $"{GameStateManager.Instance.playerData.money}€";
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i].interactable = GameStateManager.Instance.playerData.money >=
                                           scriptableTowers[i].towerCost; 
        }
        if (currentlySelectedTowerButton != null && !currentlySelectedTowerButton.interactable)
        {
            currentlySelectedTowerButton.image.color = currentlySelectedTowerButton.colors.normalColor;
            currentlySelectedTowerButton = null;
            BuildingController.currentlySelectedTower = null;
        }
       
    }

}



