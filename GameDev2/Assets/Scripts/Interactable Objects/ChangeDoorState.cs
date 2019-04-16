using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangeDoorState : MonoBehaviour
{
    //public string doorToChange; 

    public enum triggerType { door, dialogue }
    public triggerType t_type;

    private DoorController door;
    public GameObject trigObj;
    private NpcDialogue dialogue;
    //public bool LockOnDoorOpen; 

    [System.Serializable]
    public class DoorToChange
    {
        public string _doorToChange;
        public bool _lockDoor;
    }

    [System.Serializable]
    public class PortalToChange
    {
        public string _portalToChange;
        public bool _open;
    }

    public DoorToChange[] doorsToChange;
    public PortalToChange[] portalsToChange;
    // Start is called before the first frame update
    void Start()
    {
        if (t_type == triggerType.dialogue)
        {
            dialogue = trigObj.GetComponent<NpcDialogue>();
        }
        else if (t_type == triggerType.door)
        {
            door = trigObj.GetComponent<DoorController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doorsToChange.Length > 0)
        {
            foreach (DoorToChange _door in doorsToChange)
            {
                if (t_type == triggerType.door && door.opened && _door._lockDoor)
                {
                    LockDoor(_door._doorToChange);
                }
                else if (t_type == triggerType.door && door.opened && !_door._lockDoor)
                {
                    //Debug.Log("Unlock: " + doorToChange);
                    UnlockDoor(_door._doorToChange);
                }
            }
        }


        if (portalsToChange != null)
        {
            foreach (PortalToChange _portal in portalsToChange)
            {
                if (t_type == triggerType.door && door.opened && _portal._open)
                {
                    Debug.Log("Opened: " + _portal._portalToChange);
                    ActivatePortal(_portal._portalToChange);
                }
                else if (t_type == triggerType.door && door.opened && !_portal._open)
                {
                    //Debug.Log("Unlock: " + doorToChange);
                    DeactivatePortal(_portal._portalToChange);
                }
            }
        }
    }

    private void UnlockDoor(string doorName)
    {
        GlobalControl.Instance.savedDoors.SetDoorState(doorName, false);
    }

    private void LockDoor(string doorName)
    {
        GlobalControl.Instance.savedDoors.SetDoorState(doorName, true);
    }

    private void ActivatePortal(string portalName)
    {
        GlobalControl.Instance.savedDoors.SetPortalState(portalName, true);
    }

    private void DeactivatePortal(string portalName)
    {
        GlobalControl.Instance.savedDoors.SetPortalState(portalName, false);
    }


}
