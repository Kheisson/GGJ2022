using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        [SerializeField] private TextMeshProUGUI coinWalletText;
        
        private PlayerControl _player;
        private int coinBalance;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
            healthBar.fillAmount = 1f;
            coinBalance = 0;
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

        private void OnPlayerDamagedEvent(int value)
        {
            healthBar.fillAmount -= value / 100f;
        }

        private void OnCreditUIEvent(int amount)
        {
            coinBalance += amount;
            coinWalletText.text = coinBalance.ToString();
        }
    }
}
