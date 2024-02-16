using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ContactsManager contactManager;
    private bool isCoroutineRunning = false;

    [SerializeField] private RectTransform viewport; 
    [SerializeField] private ObjectPool  objectPoolManager;


    [SerializeField] private int loadPageCount;
    private void FixedUpdate()
    {
        Debug.Log(scrollRect.verticalNormalizedPosition);
        if (!isCoroutineRunning && scrollRect.verticalNormalizedPosition < 1f )
        {
            Debug.Log("List Expanded");
         
        }
    }
}
