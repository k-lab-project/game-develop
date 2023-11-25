using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DragAndDrop : MonoBehaviour
{
    Vector2 MousePosition;
    private float Speed = 1250.0f;
    private GameObject Other;
    public RectTransform transform_cursor;
    [SerializeField] private GameObject Trash_Button;
    private Vector3 RememberPosition;
    private Vector3 changePos;
    private bool isUIMoving=false;
    private bool Editable=false;
    private bool othernone = false;

    
    private void Start()
    {
        RememberPosition = transform_cursor.position;
        
        
        if (this.gameObject.name.Substring(1, 1) == "1")
        {
            Other = GameObject.Find(this.gameObject.name.Substring(0, 1) + "2");
            changePos= gameObject.transform.position - Other.gameObject.transform.position;
            
        }
        else if (this.gameObject.name.Substring(1, 1) == "2")
        {
            Other = GameObject.Find(this.gameObject.name.Substring(0, 1) + "1");
            changePos = gameObject.transform.position - Other.gameObject.transform.position;
            
        }
        else
        {
            othernone = true;
        }
    }
    private void Update()
    {
        if (Other == null && !othernone)
        {
            if (this.gameObject.name.Substring(1, 1) == "1")
            {
                Other = GameObject.Find(this.gameObject.name.Substring(0, 1) + "2");
                changePos = gameObject.transform.position - Other.gameObject.transform.position;

            }
            else if (this.gameObject.name.Substring(1, 1) == "2")
            {
                Other = GameObject.Find(this.gameObject.name.Substring(0, 1) + "1");
                changePos = gameObject.transform.position - Other.gameObject.transform.position;

            }
        }
        if (Editable)
        {
            Update_MousePosition();
            Other.GetComponent<DragAndDrop>().Other_Update_MousePosition();   
        }

    }

    public void checkDestroy()
    {
        if (AddClassButton.instance.checkingdrag)
        {
            DestroyPrefab();
            EndDrag();
        }
    }
    private void DestroyPrefab()
    {
        GameObject trash=GameObject.Find("TrashButtonParent").transform.Find("TrashButton").gameObject;
        if (((Input.mousePosition.x <= trash.transform.position.x + 15) && (Input.mousePosition.x >= trash.transform.position.x - 15)) && ((Input.mousePosition.y <= trash.transform.position.y + 15) && (Input.mousePosition.y >= trash.transform.position.y - 15)))
        {
            SugangBasketManager.instance.RemoveSubject(gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text.ToString());   
            Destroy(this.gameObject);
            Destroy(Other.gameObject);
            
            UIRecycleViewControllerSample.instance.LoadData();
            SugangBasketManager.instance.DisplayClass();
        }
    }
     
    public void OnDrag()
    {
        if (AddClassButton.instance.checkingdrag)
        {
            Editable = true;
            GameObject.Find("TrashButtonParent").transform.Find("TrashButton").gameObject.SetActive(true);
        }
    }

    private void EndDrag()
    {
        GameObject.Find("TrashButtonParent").transform.Find("TrashButton").gameObject.SetActive(false);
        Editable = false;
        Vector2 mousePos = Input.mousePosition;
        StartCoroutine(MoveUIToRemember(this.gameObject, new Vector3(mousePos.x, mousePos.y, 0), RememberPosition));
        Other.GetComponent<DragAndDrop>().OtherEndDrag();
        
    }

    public void OtherEndDrag()
    {
        //transform_cursor.position = RememberPosition;
        Vector2 mousePos = Input.mousePosition;
        StartCoroutine(MoveUIToRemember(this.gameObject, new Vector3(mousePos.x + changePos.x, mousePos.y + changePos.y, 0), RememberPosition));
    }
    public void Update_MousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        transform_cursor.position = new Vector3(mousePos.x , mousePos.y , 0);
    }
    public void Other_Update_MousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        
        transform_cursor.position = new Vector3(mousePos.x + changePos.x, mousePos.y + changePos.y, 0);
    }
    IEnumerator MoveUIToRemember(GameObject obj, Vector3 Start, Vector3 End)
    {
        isUIMoving = true;

        float journeyLength = Vector3.Distance(Start, End);
        float startTime = Time.time;

        while (isUIMoving)
        {
            float distanceCovered = (Time.time - startTime) * Speed;
            float journeyFraction = distanceCovered / journeyLength;
            Vector3 newPosition = Vector3.Lerp(Start, End, journeyFraction);
            this.gameObject.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z); // 2D 위치 업데이트
            if (journeyFraction >= 1.0f)
            {
                isUIMoving = false;
            }

            yield return null;
        }
    }
    //- transform_cursor.rect.width / 2- transform_cursor.rect.y
}
