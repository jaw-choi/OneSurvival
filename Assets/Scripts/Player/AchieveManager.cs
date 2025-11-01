using System;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    enum Achieve { unlockCharacter1, unlockCharacter2, unlockCharacter3}
    Achieve[] achieves;
    private void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        if(!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }
    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);
        foreach(Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);

        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnlockCharacter();
    }
    void UnlockCharacter()
    {
        for(int i=0;i<lockCharacter.Length;i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName)==1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch(achieve)
        {
            case Achieve.unlockCharacter1:
                if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
                {
                    isAchieve = BackendGameData.Instance.UserGameData.bestScore >= 20;
                }
                break;
            case Achieve.unlockCharacter2:
                if (UserInfo.IsLoggedIn && BackendGameData.Instance != null)
                {
                    isAchieve = BackendGameData.Instance.UserGameData.gold >100;
                }
                break;
            case Achieve.unlockCharacter3:
                break;

        }
        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);
        }
    }
}
