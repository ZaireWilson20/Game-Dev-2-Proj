using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FileSelectUICalls : MonoBehaviour
{
    public void SaveToSlot1()
    {
        SaveLoad.Save(1);
    }

    public void SaveToSlot2()
    {
        SaveLoad.Save(2);
    }

    public void SaveToSlot3()
    {
        SaveLoad.Save(3);
    }

    public void SaveToSlot4()
    {
        SaveLoad.Save(4);
    }

    public void LoadSlot1()
    {
        SaveLoad.Load(1);
    }

    public void LoadSlot2()
    {
        SaveLoad.Load(2);
    }

    public void LoadSlot3()
    {
        SaveLoad.Load(3);
    }

    public void LoadSlot4()
    {
        SaveLoad.Load(4);
    }
}
