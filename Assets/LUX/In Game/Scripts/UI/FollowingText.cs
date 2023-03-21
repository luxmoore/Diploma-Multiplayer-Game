using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowingText : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRectTransform;

    private TextMeshProUGUI textMeshPro;
    private RectTransform rectTransform;

    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void UpdateText(string to)
    {
        textMeshPro.SetText(to);
        textMeshPro.ForceMeshUpdate();
    }

    public void Update()
    {
        rectTransform.anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.y;
    }
}
