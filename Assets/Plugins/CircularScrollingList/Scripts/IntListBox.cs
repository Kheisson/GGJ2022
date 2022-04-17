using AirFishLab.ScrollingList;
using UnityEngine;
using UnityEngine.UI;

// The box used for displaying the content
// Must be inherited from the class ListBox
namespace AirFishLab.Fixes
{
    public class IntListBox : ListBox
    {
        [SerializeField]
        private Text _contentText;

        [SerializeField] private GameObject onButton;
        [SerializeField] private GameObject offButton;
        private RectTransform _rectTransform;
        private Button _button;

        public string Text => _contentText.text;
        
        // This function is invoked by the `CircularScrollingList` for updating the list content.
        // The type of the content will be converted to `object` in the `IntListBank` (Defined later)
        // So it should be converted back to its own type for being used.
        // The original type of the content is `int`.
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
        }

        public void TurnButton(bool mode)
        {
            onButton.SetActive(mode);
            offButton.SetActive(!mode);
            _button.enabled = mode;
        }

        protected override void UpdateDisplayContent(object content)
        {
            _contentText.text = ((int) content).ToString();
            _rectTransform.localScale = (int) content % 5 == 0 ? new Vector3(1.5f, 1.5f, 1f) : Vector3.one;
        }
    }
}