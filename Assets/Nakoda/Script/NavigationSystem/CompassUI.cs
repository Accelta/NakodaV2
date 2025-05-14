using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassUI : MonoBehaviour
{
    public Transform player;
    public RectTransform compassBar;
    public GameObject markerPrefab;

    public float maxDistance = 500f;
    public float compassWidth = 1920f;
    public float degreesPerPixel = 360f / 1920f;

    private List<CompassElement> activeMarkers = new();

    private class CompassElement
    {
        public CompassTarget target;
        public Image icon;
    }

    void Start()
    {
        CompassTarget[] targets = FindObjectsByType<CompassTarget>(FindObjectsSortMode.None);
        foreach (var t in targets)
        {
            AddMarker(t);
        }
    }

    void Update()
    {
        foreach (var marker in activeMarkers)
        {
            Vector3 direction = marker.target.transform.position - player.position;
            float angle = Vector3.SignedAngle(player.forward, direction, Vector3.up);
            float posX = angle / degreesPerPixel;

            marker.icon.rectTransform.anchoredPosition = new Vector2(posX, 0);
            float dist = Vector3.Distance(player.position, marker.target.transform.position);
            marker.icon.enabled = dist < maxDistance;
        }
    }

    public void AddMarker(CompassTarget target)
    {
        GameObject obj = Instantiate(markerPrefab, compassBar);
        Image icon = obj.GetComponent<Image>();
        icon.sprite = target.markerData.icon;
        icon.color = target.markerData.iconColor;

        activeMarkers.Add(new CompassElement
        {
            target = target,
            icon = icon
        });
    }
}
