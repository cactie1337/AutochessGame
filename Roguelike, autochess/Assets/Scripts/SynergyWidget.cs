using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergyWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Variables")]
    [SerializeField]
    private Synergy synergy;
    [SerializeField]
    [Tooltip("Must be '0' at runtime.")]
    private int currentOutlineIteration = 0;
    [SerializeField]
    [Tooltip("Must be '0' at runtime.")]
    private int currentCenterIteration = 0;
    [SerializeField]
    [Tooltip("The list that will hold the references to our synergy bubbles outline image")]
    private List<Image> outlines = new List<Image>();
    [SerializeField]
    [Tooltip("The list that will hold the references to our synergy bubbles center image")]
    private List<Image> centers = new List<Image>();

    private Color colorOff;
    private Color colorOn;

    [SerializeField]
    private Image synergyIcon;
    [SerializeField]
    private Transform gridParent;
    [SerializeField]
    private GridLayoutGroup grid;

    private UIManager uiManagerScript;

    protected Synergy Synergy { get => synergy; set => synergy = value; }
    protected int CurrentOutlineIteration { get => currentOutlineIteration; set => currentOutlineIteration = value; }
    protected int CurrentCenterIteration { get => currentCenterIteration; set => currentCenterIteration = value; }
    protected List<Image> Outlines { get => outlines; set => outlines = value; }
    protected List<Image> Centers { get => centers; set => centers = value; }
    protected Image SynergyIcon { get => synergyIcon; set => synergyIcon = value; }
    protected Transform GridParent { get => gridParent; set => gridParent = value; }
    protected GridLayoutGroup Grid { get => grid; set => grid = value; }
    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }

    protected virtual void Awake()
    {
        UIManagerScript = UIManager.Instance;
        if (!UIManagerScript)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene. PLease add one before entering playmode!");
        }

        //this must be zero at runtime
        CurrentOutlineIteration = 0;
        CurrentCenterIteration = 0;
    }

    public virtual void Setup (Synergy synergy)
    {
        float[] sizes = SetGridSize(synergy.totalSynergySize);
        for (int i = 0; i < synergy.totalSynergySize; i++)
        {
            GameObject bubble = Instantiate(UIManagerScript.SynergyBubblePrefab, GridParent);
            Image outline = bubble.transform.GetChild(0).GetComponent<Image>();
            if (!outline)
            {
                Debug.LogError("No image found on child 0 of the Synergy Bubble prefab. Please add one or re-arrange your children so the first one has an image for the outline.");
            }
            RectTransform outlineCenter = bubble.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            Image center = bubble.transform.GetChild(1).GetComponent<Image>();
            if (!center)
            {
                Debug.LogError("No image found on child 1 of the Synergy Bubble prefab. Please add one or re-arrange your children so the second one has an image for the center.");
            }

            outline.rectTransform.sizeDelta = new Vector2(sizes[0], sizes[0]);
            outlineCenter.sizeDelta = new Vector2(sizes[1], sizes[1]);
            center.rectTransform.sizeDelta = new Vector2(sizes[1], sizes[1]);


            Synergy = synergy;

           
            colorOn = synergy.color;
            colorOn.a = 1f;

            colorOff = synergy.color;
            colorOff.a = 0f;

            outline.color = colorOff;
            center.color = colorOff;


            Outlines.Add(outline);
            Centers.Add(center);

        }
        SynergyIcon.sprite = synergy.icon;
    }
    protected virtual float[] SetGridSize(int totalSynergySize)
    {
        if (totalSynergySize == 1)
        {
            Grid.cellSize = new Vector2(15, 15);
            return new float[] { 15, 12.5f };
        }
        else if (totalSynergySize < 5)
        {
            Grid.cellSize = new Vector3(10, 10);
            return new float[] { 10, 7.5f };
        }
        else
        {
            Grid.cellSize = new Vector3(7.5f, 7.5f);
            return new float[] { 7.5f, 5 };
        }
    }
    public virtual void AddOutline()
    {
        
        if (CurrentOutlineIteration >= outlines.Count)
        {
            return;
        }

        outlines[CurrentOutlineIteration].color = colorOn;

        CurrentOutlineIteration++;
    }
    public virtual void RemoveOutline()
    {
        if (CurrentOutlineIteration == 0)
        {
            Debug.LogError("You called for an outline decrement when the current iteration was already 0 (meaning you had no outlines).");
            return;
        }

        outlines[CurrentOutlineIteration - 1].color = colorOff;

        CurrentOutlineIteration--;
    }
    public virtual void AddCenterFill()
    {
        if (CurrentCenterIteration >= centers.Count)
        {
            return;
        }

        centers[CurrentCenterIteration].color = colorOn;

        CurrentCenterIteration++;
    }
    public virtual void RemoveCenterFill()
    {
        if (CurrentCenterIteration == 0)
        {
            Debug.LogError("You called for a center decrement when the current iteration was already 0 (meaning you had no centers filled).");
            return;
        }

        centers[CurrentCenterIteration - 1].color = colorOff;

        CurrentCenterIteration--;
    }
    public virtual void ShowWidget()
    {
        transform.parent = UIManagerScript.SynergyPanel.menuTransform;
    }
    public virtual void HideWidget()
    {
        transform.parent = UIManagerScript.SynergyPanel.hiddenPosition.transform;
    }
    public virtual void OnPointerEnter(PointerEventData pointerEventData)
    {
        //print("mouse entered synergy widget");
        UIManagerScript.OpenDesktopTooltip(Synergy);
    }
    public virtual void OnPointerExit(PointerEventData pointerEventData)
    {
        //print("mouse exited synergy widget");
        UIManagerScript.CloseDesktopTooltip();
    }

}
