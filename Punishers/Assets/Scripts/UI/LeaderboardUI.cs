using TMPro;
using UnityEngine;
using YG;
using YG.Utils.LB;

public class LeaderboardUI : MonoBehaviour
{
    public GameObject leaderboardWindow;
    public TextMeshProUGUI leaderboardText;

    private const string LeaderboardName = "EnemiesKilledLeaderboard";

    private void Start()
    {
        // Убедитесь, что окно лидерборда всегда открыто
        leaderboardWindow.SetActive(true);
        UpdateLeaderboard();
    }

    private void OnEnable()
    {
        YandexGame.onGetLeaderboard += OnGetLeaderboard;
    }

    private void OnDisable()
    {
        YandexGame.onGetLeaderboard -= OnGetLeaderboard;
    }

    public void UpdateLeaderboard()
    {
        YandexGame.GetLeaderboard(LeaderboardName, 10, 3, 3, "small");
    }

    private void OnGetLeaderboard(LBData lb)
    {
        leaderboardText.text = $"";
        if (lb.technoName == LeaderboardName)
        {
            foreach (var player in lb.players)
            {
                leaderboardText.text += $"{player.rank}. {player.name} - {player.score}\n";
            }
        }
    }
}