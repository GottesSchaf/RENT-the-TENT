using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UKEA_Shop : MonoBehaviour {

    [SerializeField] GameObject shoppingWindow, chairsWindow, tablesWindow, lampsWindow;

    public void OpenShoppingWindow()
    {
        shoppingWindow.SetActive(true);
    }

    public void CloseShoppingWindow()
    {
        //Check which window is opened and close deactivate it
        if (tablesWindow.activeInHierarchy)
        {
            tablesWindow.SetActive(false);
        }
        else if (lampsWindow.activeInHierarchy)
        {
            lampsWindow.SetActive(false);
        }
        else if (chairsWindow.activeInHierarchy)
        {
            chairsWindow.SetActive(false);
        }
        else if (shoppingWindow.activeInHierarchy)
        {
            shoppingWindow.SetActive(false);
        }
    }

    public void MenueChairs() //The window shown when you click on the Chairs menue point
    {
        //Check which window is opened right now, close it and open the Chairs window
        if (tablesWindow.activeInHierarchy)
        {
            tablesWindow.SetActive(false);
            chairsWindow.SetActive(true);
        }
        else if (lampsWindow.activeInHierarchy)
        {
            lampsWindow.SetActive(false);
            chairsWindow.SetActive(true);
        }
        else if (shoppingWindow.activeInHierarchy)
        {
            shoppingWindow.SetActive(false);
            chairsWindow.SetActive(true);
        }
    }

    public void MenueTables() //The window shown when you click on the Tables menue point
    {
        //Check which window is opened right now, close it and open the Tables window
        if (chairsWindow.activeInHierarchy)
        {
            chairsWindow.SetActive(false);
            tablesWindow.SetActive(true);
        }
        else if (lampsWindow.activeInHierarchy)
        {
            lampsWindow.SetActive(false);
            tablesWindow.SetActive(true);
        }
        else if (shoppingWindow.activeInHierarchy)
        {
            shoppingWindow.SetActive(false);
            tablesWindow.SetActive(true);
        }
    }

    public void MenueLamps() //The window shown when you click on the Lamps menue point
    {
        //Check which window is opened right now, close it and open the Lamps window
        if (tablesWindow.activeInHierarchy)
        {
            tablesWindow.SetActive(false);
            lampsWindow.SetActive(true);
        }
        else if (chairsWindow.activeInHierarchy)
        {
            chairsWindow.SetActive(false);
            lampsWindow.SetActive(true);
        }
        else if (shoppingWindow.activeInHierarchy)
        {
            shoppingWindow.SetActive(false);
            lampsWindow.SetActive(true);
        }
    }

    public void MenueLandingPage() //The window shown when you click on the Logo
    {
        //Check which window is opened right now, close it and open the Chairs window
        if (tablesWindow.activeInHierarchy)
        {
            tablesWindow.SetActive(false);
            shoppingWindow.SetActive(true);
        }
        else if (lampsWindow.activeInHierarchy)
        {
            lampsWindow.SetActive(false);
            chairsWindow.SetActive(true);
        }
        else if (chairsWindow.activeInHierarchy)
        {
            chairsWindow.SetActive(false);
            shoppingWindow.SetActive(true);
        }
    }
}
