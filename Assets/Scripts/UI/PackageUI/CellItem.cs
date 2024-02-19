using JKFrame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ��Ʒ���е�ʵ����Ʒ
/// </summary>
public class CellItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image PropImage;

    public Text PropNumText;

    private UI_PackageCell originalCell;

    [HideInInspector] public PropStats propStats;

    private Transform canvas; // ˵����������canvas

    private UI_PropInfo propInfo; // ˵������

    private void Start()
    {
        canvas = GameObject.Find("JKFrame/UISystem").transform;
    }

    /// <summary>
    /// ��ʼ��PropItem
    /// </summary>
    /// <param name="propStats">����</param>
    /// <param name="cell">�������߲�</param>
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
        // ��ȡ˵�����ڳ���λ��
        Vector3 relativePosition = transform.position;
        relativePosition.y = relativePosition.y + 85; //˵�����ڵ�һ���
        relativePosition.x = relativePosition.x - 230; // һ���
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
        if (eventData.pointerCurrentRaycast.gameObject.name == "Item") // ����
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
        else if (eventData.pointerCurrentRaycast.gameObject.name == "UI_PackageCell") // ���ÿ�λ
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
