using Core;
using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI coinWalletText;
        
        private PlayerControl _player;
        private int _coinBalance;
        

        #endregion

        #region Propeties

        public int CoinBalance => _coinBalance;

        #endregion

        #region Methods

        //Initializes UI to start of level defaults
        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
            healthBar.fillAmount = 1f;
            _coinBalance = 0;
            coinWalletText.text = "0";
        }

        private void OnEnable()
        {
            _player.PlayerDamagedEvent += OnPlayerDamagedEvent;
            GameManager.Instance.CreditUIEvent += OnCreditUIEvent;
        }

        private void OnDisable()
        {
            _player.PlayerDamagedEvent -= OnPlayerDamagedEvent;
            GameManager.Instance.CreditUIEvent -= OnCreditUIEvent;

        }
        //Updates Hearts to reflect the damage the player has taken
        private void OnPlayerDamagedEvent(int value)
        {
            healthBar.fillAmount -= value / 100f;
        }
        
        //Updates Coin wallet to new balance
        private void OnCreditUIEvent(int amount)
        {
            _coinBalance += amount;
            coinWalletText.text = _coinBalance.ToString();
        }

        #endregion
    }
}
