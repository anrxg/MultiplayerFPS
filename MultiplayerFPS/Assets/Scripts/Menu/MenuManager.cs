using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menus;
    public static MenuManager instance;

    void Awake()
    {
        instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                OpenMenu(menus[i]);
                return; 
            }
        }

        Debug.LogWarning("Menu not found: " + menuName);
    }

    public void OpenMenu(Menu menuToOpen)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].isOpen && menus[i] != menuToOpen)
            {
                CloseMenu(menus[i]); 
            }
        }

        if (!menuToOpen.isOpen)
        {
            menuToOpen.Open();
        }
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
