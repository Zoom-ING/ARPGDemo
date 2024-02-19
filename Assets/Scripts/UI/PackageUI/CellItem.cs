using JKFrame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 物品槽中的实际物品
/// </summary>
public class CellItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image PropImage;

    public Text PropNumText;

    private UI_PackageCell originalCell;

    [HideInInspector] public PropStats propStats;

    private Transform canvas; // 说明窗口所在canvas

    private UI_PropInfo propInfo; // 说明窗口

    private void Start()
    {
        canvas = GameObject.Find("JKFrame/UISystem").transform;
    }

    /// <summary>
    /// 初始化PropItem
    /// </summary>
    /// <param name="propStats">数据</param>
    /// <param name="cell">所属道具槽</param>
    public void Init(PropStats propStats, UI_PackageCell cell)
    {
        this.originalCell = cell;
        this.propStats = propStats;
        PropImage.sprite = Resources.Load<Sprite>(propStats.SpritePrefabPath);
        PropNumText.text = propStats.Num.ToString();
    }

    public void UpdateCell(PropStats propStats)
    {
        this.propStats = propStats;
        PropImage.sprite = Resources.Load<Sprite>(propStats.SpritePrefabPath);
        PropNumText.text = propStats.Num.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 获取说明窗口出现位置
        Vector3 relativePosition = transform.position;
        relativePosition.y = relativePosition.y + 85; //说明窗口的一半高
        relativePosition.x = relativePosition.x - 230; // 一半宽
        Vector3 worldPositive = canvas.TransformPoint(relativePosition);

        propInfo = ResSystem.InstantiateGameObject<UI_PropInfo>("UI_PropInfo", canvas.transform);
        propInfo.transform.position = canvas.InverseTransformPoint(worldPositive);
        propInfo.Init(propStats);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        propInfo.GameObjectPushPool();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(canvas.transform);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "Item") // 交换
        {
            PropStats tempPropStats = new PropStats(propStats);

            transform.SetParent(originalCell.transform);
            transform.position = originalCell.transform.position;

            originalCell.UnInit();
            originalCell.Init(eventData.pointerCurrentRaycast.gameObject.GetComponent<CellItem>().propStats);


            eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_PackageCell>().UnInit();
            eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_PackageCell>().Init(tempPropStats);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "UI_PackageCell") // 放置空位
        {
            transform.SetParent(originalCell.transform);
            transform.position = originalCell.transform.position;
            originalCell.UnInit();
            eventData.pointerCurrentRaycast.gameObject.GetComponent<UI_PackageCell>().Init(propStats);

            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        else
        {
            transform.SetParent(originalCell.transform);
            transform.position = originalCell.transform.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private void OnDisable()
    {
        if (propInfo != null) propInfo.GameObjectPushPool();
    }
}
