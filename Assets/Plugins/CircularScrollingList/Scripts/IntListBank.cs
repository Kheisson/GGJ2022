using AirFishLab.ScrollingList;
using UnityEngine;

// The bank for providing the content for the box to display
// Must be inherit from the class BaseListBank
namespace Plugins.CircularScrollingList.Scripts
{
    public class IntListBank : BaseListBank
    {
        [SerializeField] private int[] levels = {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10
        };

        // This function will be invoked by the `CircularScrollingList`
        // when acquiring the content to display
        // The object returned will be converted to the type `object`
        // which will be converted back to its own type in `IntListBox.UpdateDisplayContent()`
        public override object GetListContent(int index)
        {
            return levels[index];
        }

        public override int GetListLength()
        {
            return levels.Length;
        }
    }
}