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

        private void LoadWardrobe()
        {
            var bodyData = DataManager.GetPlayerData().bodyData;
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