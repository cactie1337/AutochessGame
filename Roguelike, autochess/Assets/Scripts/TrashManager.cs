using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class TrashManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color hoveredColor;
    private Color normalColor;

    private Image myImage;
    private UnitDragManager unitDragScript;

    protected UnitDragManager UnitDragScript { get => unitDragScript; set => unitDragScript = value; }

    protected virtual void Awake()
    {
        myImage = GetComponent<Image>();
        UnitDragScript = UnitDragManager.Instance;
        if (!UnitDragScript)
        {
            Debug.LogError("No PawnDragManager singleton instance found in the scene. PLease add a PawnDragManager script to the Game Manager gameobject before entering playmode.");
        }
        //normalColor = Color.black;
        normalColor = new Color(0f, 0f, 0f, 0.5f);
    }
    public virtual void OnPointerEnter(PointerEventData data)
    {
        UnitDragScript.HoveredOnTrash = true;
        myImage.color = hoveredColor;
    }

    public virtual void OnPointerExit(PointerEventData data)
    {
        StartCoroutine(DelayedExitTrashPanel());
    }
    protected virtual IEnumerator DelayedExitTrashPanel()
    {
        yield return new WaitForEndOfFrame();
        {
            UnitDragScript.HoveredOnTrash = false;
            myImage.color = normalColor;
        }
    }
        

}
