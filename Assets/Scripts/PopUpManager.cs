using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpManager : MonoBehaviour
{
    public static PopUpManager instance;

    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] private GameObject messageObj;
    private bool isDisplayingMessage = false;
    private int messagecnt = 0;
    private void Awake()
    {
        instance = this;   
    }
    private void Update()
    {
        if (isDisplayingMessage && messagecnt == 0)
        {
            messageObj.SetActive(true);
            messagecnt = 1;
            StartCoroutine(DisplayMessageForSeconds());
        }

    }
    IEnumerator DisplayMessageForSeconds()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        {
            messageText.text = "";
            isDisplayingMessage = false;
            messageObj.SetActive(false);
            messagecnt = 0;
        }

    }
    public void ShowMessageCannotApply()
    {
        messageText.text = "�ߺ��Ǵ� �ð����� �ٸ� �ð�ǥ��\n���� �� ����!";
        isDisplayingMessage = true;

    }
}
