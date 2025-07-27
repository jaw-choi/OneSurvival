using UnityEngine;

public class InGameState : IGameState
{
    public void Enter()
    {
        Debug.Log("InGame ���� ����");
    }

    public void Update()
    {
        // ���� ���� �� ó��
        if (GameManager.Instance.IsGameOver)
        {
            GameStateManager.Instance.ChangeState(new GameOverState());
        }
    }

    public void Exit()
    {
        Debug.Log("InGame ���� ����");
    }
}
