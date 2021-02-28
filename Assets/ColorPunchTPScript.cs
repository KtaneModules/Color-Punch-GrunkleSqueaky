using EmikBaseModules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPunchTPScript : TPScript
{
    public ColorPunchScript Module;

    internal override ModuleScript ModuleScript
    {
        get
        {
            return Module;
        }
    }

#pragma warning disable 414
    new private const string TwitchHelpMessage = @"Use '!{0} red' to press the red button. Acceptable button names are 'red', 'blue', 'green', 'yellow', 'black', 'r', 'b', 'g', 'y', 'k'";
#pragma warning restore 414

    internal override IEnumerator ProcessTwitchCommand(string command)
    {
        string[] validColors = new string[] { "RED", "BLUE", "GREEN", "YELLOW", "BLACK", "R", "B", "G", "Y", "K" };
        string[] parameters = command.Trim().ToUpperInvariant().Split(' ');
        int buttonIndex = 0;

        if ((parameters.Length == 2) && ((parameters[0] == "PRESS") || (parameters[0] == "SUBMIT")))
        {
            buttonIndex = Array.IndexOf(validColors, parameters[1]) % 5;
        }
        else if (parameters.Length == 1)
        {
            buttonIndex = Array.IndexOf(validColors, parameters[0]) % 5;
        }
        else
        {
            yield return SendToChatError("pls format ur command properly.");
        }
        if (buttonIndex > -1)
        {
            yield return null;
            Module.Buttons[buttonIndex].OnInteract();
        }
        else
        {
            yield return SendToChatError("that button doesn't exist");
        }
    }

    internal override IEnumerator TwitchHandleForcedSolve()
    {
        while (true)
        {
            if (!Module.IsNeedyActive)
            {
                yield return true;
                continue;
            }
       
            Module.Buttons[(int)Module.solution].OnInteract();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
