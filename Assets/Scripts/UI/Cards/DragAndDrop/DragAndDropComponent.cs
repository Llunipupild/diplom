using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Cards.DragAndDrop
{
    public class DragAndDropComponent : MonoBehaviour, IDragHandler
    {
        public float ScaleFactor { get; set; }
        private RectTransform _rectTransform;
        private Vector3 _startedPosition;
        
        private void Start()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _startedPosition = _rectTransform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == null) {
                return;
            }

            Vector2 eventDataDelta = eventData.delta / ScaleFactor;
            if (eventDataDelta.x > Screen.width){
                ReturnOnStartedPosition();
            }
            _rectTransform.anchoredPosition += eventData.delta / ScaleFactor;
        }
        
        private void ReturnOnStartedPosition()
        {
            _rectTransform.position = _startedPosition;
        }
    }
}