using UnityEngine;

public class InGameState : IGameState
{
    public void Enter()
    {
        Debug.Log("InGame 상태 진입");
    }

    public void Update()
    {
        // 게임 진행 중 처리
        if (GameManager.Instance.IsGameOver)
        {
            GameStateManager.Instance.ChangeState(new GameOverState());
        }
    }

    public void Exit()
    {
        Debug.Log("InGame 상태 종료");
    }
}
