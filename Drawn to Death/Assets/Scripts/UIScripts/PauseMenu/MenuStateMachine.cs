using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;



// Script to navigate between different menus easier
public class MenuStateMachine
{
    public MenuBaseState currentMenu { get; set; }
    public Stack<MenuBaseState> menuStack { get; set; }
    private MenuBaseState initialState;


    public void Initialize(MenuBaseState startingState)
    {

        // These lines will start the stack to keep track where we are in the menus
        initialState = startingState;
        menuStack = new Stack<MenuBaseState>();
        menuStack.Push(startingState);


        currentMenu = startingState;
        currentMenu.EnterState();
    }


    public void ChangeState(MenuBaseState newState)
    {
        menuStack.Push(newState);

        currentMenu.ExitState();
        currentMenu = newState;
        currentMenu.EnterState();
    
    }

    public void GoBackState()
    {
        PopState();
        MenuBaseState previousState = menuStack.Peek();
        currentMenu.ExitState();
        currentMenu = previousState;
        currentMenu.EnterState();

       
    }


    public void PopState()
    {
        menuStack.Pop();

    }


    // Checks if we have reached the end of the stack
    public bool EndOfStack()
    {
        if (menuStack.Peek() == initialState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  

}
