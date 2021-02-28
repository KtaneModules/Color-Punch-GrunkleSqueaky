using EmikBaseModules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPunchScript : ModuleScript
{
    internal override ModuleConfig ModuleConfig
    {
        get
        {
            return new ModuleConfig(kmNeedyModule: Needy);
        }
    }

    public KMAudio Audio;
    public KMNeedyModule Needy;
    public KMSelectable[] Buttons;
    public TextMesh[] Texts;

    private TextColor _solution;

    private void Start()
    {
        Needy.Assign(onNeedyActivation: OnNeedyActivation,
            onNeedyDeactivation: OnNeedyDeactivation,
            onTimerExpired: () => delegate () 
            {
                Needy.HandleStrike();
                OnNeedyDeactivation().Invoke();
            },
            moduleScript: this);

        Buttons.Assign(onInteract: HandleButtons);
    }

    private Action OnNeedyActivation()
    {
        return () =>
        {
            var colors = new Stack<TextColor>(Helper.EnumAsArray<TextColor>().Shuffle());


            for (int i = 0; i < Texts.Length; i++)
            {
                Texts[i].text = colors.Pop().ToString();
                Texts[i].color = ToColor(colors.Pop());
            }

            _solution = colors.Pop();
        };
    }

    private Action OnNeedyDeactivation()
    {
        return () =>
        {
            for (int i = 0; i < Texts.Length; i++)
            {
                Texts[i].text = string.Empty;
            }
        };
    }

    private bool HandleButtons(int arg1)
    {
        Buttons[arg1].Button(Audio, Buttons[arg1].transform, 0.8f, gameSound: KMSoundOverride.SoundEffect.ButtonPress);

        if (!IsNeedyActive)
        {
            return false;
        }

        IsNeedyActive = false;

        if ((TextColor)arg1 == _solution)
        {
            Needy.HandlePass();           
        }
        else
        {
            Needy.HandleStrike();
        }

        OnNeedyDeactivation().Invoke();

        return false;      
    }

    private Color ToColor(TextColor textColor)
    {
        switch (textColor)
        {
            case TextColor.Red:
                return Color.red;

            case TextColor.Blue:
                return Color.blue;

            case TextColor.Green:
                return Color.green;

            case TextColor.Yellow:
                return Color.yellow;

            case TextColor.Black:
                return Color.black;

            default:
                throw new NotImplementedException("Unexpected TextColor passed into ToColor: {0}".Form(textColor));
        }
    }

}
