using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
public class PowerSetController : MonoBehaviour
{
    public TMP_Text powerSetDisplay;
    public GameObject playerObj;
    public GameObject weapon;
    public GameObject util;
    private Player playerScript;
    private Sprite m_powerImg;
    private Sprite m_weaponImg;
    private Sprite s_powerImg;
    private Sprite s_weaponImg;
    public GameObject[] mSet;
    public GameObject[] pSet;
    private string path = "Assets/Sprites/Power_Icons/";
    //private string path2 = "Assets/Resources";

    private void Start()
    {
        playerScript = playerObj.GetComponent<Player>();
    }

    public void SetMWeaponImg(string _name)
    {
#if UNITY_EDITOR
        m_weaponImg = (Sprite)AssetDatabase.LoadAssetAtPath(path + _name + ".png", typeof(Sprite));
#else
            m_weaponImg = Resources.Load<Sprite>(_name);
#endif
    }

    public void SetMPowerImg(string _name)
    {
#if UNITY_EDITOR
        m_powerImg = (Sprite)AssetDatabase.LoadAssetAtPath(path + _name + ".png", typeof(Sprite));
#else
            m_powerImg = Resources.Load<Sprite>(_name);
#endif
    }

    public void SetSWeaponImg(string _name)
    {
#if UNITY_EDITOR
        s_weaponImg = (Sprite)AssetDatabase.LoadAssetAtPath(path + _name + ".png", typeof(Sprite));
#else
            s_weaponImg = Resources.Load<Sprite>(_name);
#endif
    }

    public void SetSPowerImg(string _name)
    {
#if UNITY_EDITOR
        s_powerImg = (Sprite)AssetDatabase.LoadAssetAtPath(path + _name + ".png", typeof(Sprite));
#else
            s_powerImg = Resources.Load<Sprite>(_name);
#endif
    }

    public void ShowTSet()
    {
        weapon.GetComponent<Image>().sprite = s_weaponImg;
        util.GetComponent<Image>().sprite = s_powerImg;

    }

    public void ShowMSet()
    {
        weapon.GetComponent<Image>().sprite = m_weaponImg;
        util.GetComponent<Image>().sprite = m_powerImg;
    }
}
