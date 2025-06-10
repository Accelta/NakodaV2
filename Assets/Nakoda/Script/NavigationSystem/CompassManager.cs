using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompassManager : MonoBehaviour
{
    public static CompassManager Instance { get; private set; }

    [Header("Setup")]
    public Transform player;
    public RectTransform compassBar;
    public GameObject markerPrefab;
    public float compassWidth = 1920f;
    public float maxMarkerDistance = 500f;

    private float degreesPerPixel;
    private List<CompassElement> markers = new();

    private class CompassElement
    {
        public CompassTarget target;
        public Image icon;
        public TextMeshProUGUI distanceText;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        degreesPerPixel = 360f / compassWidth;

        // CompassTarget[] targets = FindObjectsByType<CompassTarget>(FindObjectsSortMode.None);
        // foreach (var target in targets)
        // {
        //     AddMarker(target);
        // }
    }

    void Update()
    {
        foreach (var marker in markers)
        {
            Vector3 direction = marker.target.transform.position - player.position;
            float distance = direction.magnitude;

            float angle = Vector3.SignedAngle(player.forward, direction, Vector3.up);
            float posX = angle / degreesPerPixel;

            marker.icon.rectTransform.anchoredPosition = new Vector2(posX, 0);
            marker.icon.enabled = distance < maxMarkerDistance;

            if (marker.distanceText != null)
            {
                float dist = Vector3.Distance(player.position, marker.target.transform.position);
                marker.distanceText.text = $"{Mathf.RoundToInt(dist)}m";
            }
        }
    }

    public void AddMarker(CompassTarget target)
    {
        if (target.markerData == null) return;

        GameObject iconObj = Instantiate(markerPrefab, compassBar);
        Image icon = iconObj.GetComponent<Image>();
        icon.sprite = target.markerData.icon;
        icon.color = target.markerData.iconColor;

        TextMeshProUGUI distanceText = iconObj.GetComponentInChildren<TextMeshProUGUI>();

        markers.Add(new CompassElement
        {
            target = target,
            icon = icon,
            distanceText = distanceText
        });
    }
    public void RemoveMarker(CompassTarget target)
    {
        CompassElement marker = markers.Find(m => m.target == target);
        if (marker != null)
        {
            markers.Remove(marker);
            Destroy(marker.icon.gameObject);
        #if UNITY_EDITOR
            Debug.Log($"Marker for {target.name} removed from compass.");
        #endif
        }
        
    }
}
