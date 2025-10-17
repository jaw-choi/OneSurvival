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
}
