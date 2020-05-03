using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EnvLevelEditor;

public class NotPosWhenOnUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EnvLevelEditorView script;

    public void CanRay(bool i)
    {
        Debug.Log(i);
        script.setRay(i);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CanRay(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanRay(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
