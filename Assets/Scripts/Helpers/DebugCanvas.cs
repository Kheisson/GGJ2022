using Core;
using Spawn;
using TMPro;
using UnityEngine;

public class DebugCanvas : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private bool debug;
    private string _debugString;

    private void Update()
    {
        if(!debug) Destroy(gameObject);
        _debugString = @"$""Level Name: {SpawnManager.Instance.LevelName}\n"" +
                       $""Enemy waves: {SpawnManager.Instance.EnemyWaves}\n"" +
                       $""Enemies already spawned: {SpawnManager.Instance.EnemiesInLevel}\n"" +
                       $""Screen Width: {GameSettings.ScreenBoundaries.x}\n"" +
                       $""Screen Height: {GameSettings.ScreenBoundaries.y}\n""";
        textMeshProUGUI.text = _debugString;
    }

}
