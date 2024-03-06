using System;
using Proto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ChatMessageItem : MonoBehaviour
    {
        // TODO Dynamic size
        public const float DEFAULT_HEIGHT = 40;
        public const float DEFAULT_WIDTH = 400;
        public const float DEFAULT_ICON_WIDTH = 40;
        public const float DEFAULT_CONTENT_WIDTH = 360;

        private RectTransform _rectTransform;
        private LayoutElement _layoutElement;
        private TextMeshProUGUI _content;


        private float fontSize => _content.fontSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _layoutElement = GetComponent<LayoutElement>();
            _content = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Bind(int idx, ChatMessage message)
        {
            _content.text = message.Content;
            _layoutElement.preferredHeight = _content.preferredHeight;
            _content.rectTransform.sizeDelta = new Vector2(DEFAULT_CONTENT_WIDTH, _content.preferredHeight);
            if (_content.preferredHeight > DEFAULT_HEIGHT)
            {
                _rectTransform.sizeDelta = new Vector2(DEFAULT_WIDTH, _content.preferredHeight);
            }
            else
            {
                _rectTransform.sizeDelta = new Vector2(DEFAULT_WIDTH, DEFAULT_HEIGHT);
            }
        }
    }
}