using Save;
using UnityEngine;

namespace Player
{
    public class PlayerCosmetic : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject cockpitsParent;
        [SerializeField] private GameObject tailsParent;
        [SerializeField] private GameObject wingsParent;
        
        #endregion

        #region Methods

        private void Awake()
        {
            LoadWardrobe();
        }

        /// <summary>
        /// Based on the body data, the 3 parent models on the player gameObject are selected
        /// Transform of parent contains its children 
        /// </summary>
        private void LoadWardrobe()
        {
            var bodyData = DataManager.GetPlayerBodyData();
            foreach (Transform cockpit in cockpitsParent.transform)
            {
                cockpit.gameObject.SetActive(false);
            }
            cockpitsParent.transform.GetChild(bodyData.cockpit).gameObject.SetActive(true);
            
            foreach (Transform wing in wingsParent.transform)
            {
                wing.gameObject.SetActive(false);
            }
            wingsParent.transform.GetChild(bodyData.wings).gameObject.SetActive(true);
            
            foreach (Transform tail in tailsParent.transform)
            {
                tail.gameObject.SetActive(false);
            }
            tailsParent.transform.GetChild(bodyData.tail).gameObject.SetActive(true);
        }

        #endregion
    }
}