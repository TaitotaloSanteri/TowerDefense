using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TowerButton towerButtonPrefab;
    public ScriptableTower[] scriptableTowers;
    public TowerButton[] towerButtons;
    public static bool isPointerOnButton;
    [SerializeField] private GraphicRaycaster gr;

    private void Awake()
    {
        // Etsitään kaikki ScriptableTower -tyyppiset assetit hakemistosta
        // Resources/Towers/
        scriptableTowers = Resources.LoadAll<ScriptableTower>("Towers");
        // Luodaan jokaista tornia varten oma nappulansa
        towerButtons = new TowerButton[scriptableTowers.Length];
        float xPosition = Screen.width * 0.4f;
        float yPosition = Screen.height * 0.4f;
        float spacing = 100f;
        for (int i = 0; i < towerButtons.Length; i++)
        {
            towerButtons[i] = Instantiate(towerButtonPrefab, transform);
            towerButtons[i].towerName.text = scriptableTowers[i].towerName;
            towerButtons[i].towerCost.text = $"${scriptableTowers[i].towerCost}";
            towerButtons[i].GetComponent<RectTransform>().anchoredPosition = 
                                        new Vector3(xPosition, yPosition - (i * spacing), 0f);
        }
    }
    private void OnValidate()
    {
        gr = GetComponent<GraphicRaycaster>();
    }
    private void Update()
    {
        var eventData = new PointerEventData(EventSystem.current);
        Debug.Log(EventSystem.current.currentSelectedGameObject);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            eventData.position = Input.mousePosition;
            List<RaycastResult> list = new List<RaycastResult>();
            gr.Raycast(eventData, list);
            TowerButton tb = list[0].gameObject.GetComponent<TowerButton>();
            if (tb)
            {
                isPointerOnButton = true;
            }
        }
        else
        {
            isPointerOnButton = false;
        }
    }

}



