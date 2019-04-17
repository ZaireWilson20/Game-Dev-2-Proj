using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    public Animator anim;
    private string level;
    public bool endingCutScene;
    private bool movingCharacter;
    public CharacterMovement[] charactersToMove;
    private Player pscript;

    [System.Serializable]
    public class CharacterMovement
    {
        public GameObject character;
        public GameObject positionToMove;
    }

    // Start is called before the first frame update
    void Start()
    {
        pscript = FindObjectOfType<Player>();
    }

    public void FadeToLevel(string levelName)
    {
        if (endingCutScene)
        {
            GlobalControl.Instance.savedScene.inCutScene = false;
        }
        Debug.Log("FADING OUT");
        anim.SetTrigger("FadeOut");
        level = levelName;
    }

    public void LoadOnFadeComplete()
    {
        pscript.SavePlayer();
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void StartLevelTransition()
    {
        anim.SetTrigger("TranFade");
    }
    public void CharacterTransition()
    {
        if (movingCharacter)
        {
            foreach(CharacterMovement c in charactersToMove)
            {
                c.character.gameObject.transform.position = c.positionToMove.gameObject.transform.position;
            }
        }
        anim.SetTrigger("FadeIn");
        movingCharacter = false;
    }

    public void MoveCharacterInScene()
    {
        movingCharacter = true;
        StartLevelTransition();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
