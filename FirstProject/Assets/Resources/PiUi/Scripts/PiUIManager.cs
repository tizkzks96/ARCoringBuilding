using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiUIManager : MonoBehaviour
{
    public static PiUIManager instance;

    public NameMenuPair[] nameMenu;
    private Dictionary<string, PiUI> dict = new Dictionary<string, PiUI>( );

    public PiUI CurrentMenu { get; set; }
    public string CurrentMenuName { get; set; }

    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            CurrentMenu = null;

            //if not, set instance to this
            instance = this;

        }

        foreach (NameMenuPair pair in nameMenu)
        {
            dict.Add(pair.name, pair.menu);
        }
		transform.localScale = new Vector3(0.1f, 0.1f, 0);
		transform.position = Vector2.zero;
    }


    /// <summary>
    /// Will open/close the menu name passed at the position passed.
    /// </summary>
    /// <param name="menuName">Menu to open or close.</param>
    /// <param name="pos">Position to open menu.</param>
    public void ChangeMenuState(string menuName, Vector2 pos = default(Vector2))
    {
        PiUI currentPi = GetPiUIOf(menuName);

        if (currentPi == CurrentMenu)
        {
            print("close");
            currentPi.CloseMenu( );
        }else
        {
            print("open");
            currentPi.OpenMenu(pos);
        }
    }

    public void OpenMenu(string menuName, Vector2 pos = default(Vector2))
    {
        PiUI currentPi = GetPiUIOf(menuName);
        if (CurrentMenu != null)
        {
            CloseMenu();
        }
        CurrentMenu = currentPi;
        CurrentMenuName = menuName;
        currentPi.OpenMenu(pos);
    }

    public void CloseMenu(Vector2 pos = default(Vector2))
    {
        if (CurrentMenu == null)
            return;
        PiUI currentPi = GetPiUIOf(CurrentMenuName);

        currentPi.CloseMenu();
    }

    /// <summary>
    /// Gets if the passed in piUi is currently opened
    /// </summary>
    /// <param name="piName"></param>
    /// <returns></returns>
    public bool PiOpened(string menuName)
    {
        return GetPiUIOf(menuName).openedMenu;
    }

    /// <summary>
    /// Returns the PiUi for the given menu allowing you to change it as you wish
    /// </summary>
    public PiUI GetPiUIOf(string menuName)
    {

        if (dict.ContainsKey(menuName))
        {
            return dict[menuName];
        }
        else{
            NoMenuOfThatName( );
            return null;
        }
    }

    /// <summary>
    /// After changing the PiUI.sliceCount value and piData data,call this function with the menu name to recreate the menu, at a given position
    /// </summary>
    public void RegeneratePiMenu(string menuName,Vector2 newPos = default(Vector2))
    {
        GetPiUIOf(menuName).GeneratePi(newPos);
    }

    /// <summary>
    /// After changing the PiUI.PiData call this function to update the slices, if sliceCount is changed call RegeneratePiMenu
    /// </summary>
    public void UpdatePiMenu(string menuName)
    {
        GetPiUIOf(menuName).UpdatePiUI( );
    }

    public bool OverAMenu()
    {
        foreach(KeyValuePair<string,PiUI> pi in dict)
        {
            if (pi.Value.overMenu)
            {
                return true;
            }
        }
        return false;
    }



    private void NoMenuOfThatName()
    {
        Debug.LogError("No pi menu with that name, please check the name of which you're calling");
    }

    [System.Serializable]
    public class NameMenuPair
    {
        public string name;
        public PiUI menu;

    }
}
