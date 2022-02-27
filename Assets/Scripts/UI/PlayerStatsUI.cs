using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private Image healthBar;
        private PlayerControl _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        }

        private void OnEnable()
        {
            _player.PlayerDamagedEvent += OnPlayerDamagedEvent;
        }

        private void OnDisable()
        {
            _player.PlayerDamagedEvent -= OnPlayerDamagedEvent;
        }

        private void OnPlayerDamagedEvent(int value)
        {
            healthBar.fillAmount -= value / 100f;
        }
    }
}
