using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabBehaviour : MonoBehaviour,IPointerClickHandler,IPointerExitHandler,IPointerEnterHandler
{
    public Sprite defaultSprite, selected, hover;
    Image tabImage;
    public UIManager.Tab windowToOpen;

    public static UnityEvent onTabStateChange = new UnityEvent();

    private void Awake()
    {
        tabImage = GetComponent<Image>();

        // Add this instance's function to be called on every tab state change;
        onTabStateChange.AddListener(RenderTabState);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = selected;
        UIManager.instance.OpenWindow(windowToOpen);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
    }

    void RenderTabState()
    {
        if(UIManager.instance.selectedTab == windowToOpen)
        {
            tabImage.sprite = selected;
            return;
        }
        tabImage.sprite = defaultSprite;
    }

}