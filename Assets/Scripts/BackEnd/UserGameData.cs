[System.Serializable]
public class UserGameData
{
	public int gold;            
	public int score;          
	public float playTime;
	public int bestScore;

	public void Reset()
	{
		gold = 0;
		score = 0;
		playTime = 0f;
		bestScore = 0;
	}
}

