﻿using System;
using AquaMai.Fix;
using AquaMai.Helpers;
using AquaMai.Resources;
using Manager;
using UnityEngine;

namespace AquaMai.Utils;

public class PractiseModeUI : MonoBehaviour
{
    private static float windowTop => Screen.height - GuiSizes.PlayerWidth + GuiSizes.PlayerWidth * .22f;
    private static float controlHeight => GuiSizes.PlayerWidth * .13f;
    private static float sideButtonWidth => GuiSizes.PlayerWidth * .1f;
    private static float centerButtonWidth => GuiSizes.PlayerWidth * .28f;
    private static int fontSize => (int)(GuiSizes.PlayerWidth * .02f);

    private static Rect GetButtonRect(int pos, int row)
    {
        float x;
        float width;
        switch (pos)
        {
            case 0:
                x = GuiSizes.PlayerCenter - centerButtonWidth / 2 - sideButtonWidth - GuiSizes.Margin;
                width = sideButtonWidth;
                break;
            case 1:
                x = GuiSizes.PlayerCenter - centerButtonWidth / 2;
                width = centerButtonWidth;
                break;
            case 2:
                x = GuiSizes.PlayerCenter + centerButtonWidth / 2 + GuiSizes.Margin;
                width = sideButtonWidth;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pos), pos, null);
        }

        return new Rect(x, windowTop + (GuiSizes.Margin + controlHeight) * row + GuiSizes.Margin, width, controlHeight);
    }

    public void OnGUI()
    {
        var labelStyle = GUI.skin.GetStyle("label");
        labelStyle.fontSize = fontSize;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        var buttonStyle = GUI.skin.GetStyle("button");
        buttonStyle.fontSize = fontSize;

        GUI.Box(new Rect(
            GuiSizes.PlayerCenter - centerButtonWidth / 2 - sideButtonWidth - GuiSizes.Margin * 2,
            windowTop,
            centerButtonWidth + sideButtonWidth * 2 + GuiSizes.Margin * 4,
            controlHeight * 4 + GuiSizes.Margin * 5
        ), "");

        GUI.Button(GetButtonRect(0, 0), Locale.SeekBackward);
        GUI.Button(GetButtonRect(1, 0), Locale.Pause);
        GUI.Button(GetButtonRect(2, 0), Locale.SeekForward);

        if (PractiseMode.repeatStart == -1)
        {
            GUI.Button(GetButtonRect(0, 1), Locale.MarkRepeatStart);
            GUI.Label(GetButtonRect(1, 1), Locale.RepeatNotSet);
        }
        else if (PractiseMode.repeatEnd == -1)
        {
            GUI.Button(GetButtonRect(0, 1), Locale.MarkRepeatEnd);
            GUI.Label(GetButtonRect(1, 1), Locale.RepeatStartSet);
            GUI.Button(GetButtonRect(2, 1), Locale.RepeatReset);
        }
        else
        {
            GUI.Label(GetButtonRect(1, 1), Locale.RepeatStartEndSet);
            GUI.Button(GetButtonRect(2, 1), Locale.RepeatReset);
        }

        GUI.Button(GetButtonRect(0, 2), Locale.SpeedDown);
        GUI.Label(GetButtonRect(1, 2), $"{Locale.Speed} {PractiseMode.speed * 100:000}%");
        GUI.Button(GetButtonRect(2, 2), Locale.SpeedUp);
        GUI.Button(GetButtonRect(1, 3), Locale.SpeedReset);
    }

    public void Update()
    {
        if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.E8))
        {
            DebugFeature.Seek(-1000);
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.E2))
        {
            DebugFeature.Seek(1000);
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B8) || InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B1))
        {
            DebugFeature.Pause = !DebugFeature.Pause;
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B7) && PractiseMode.repeatStart == -1)
        {
            PractiseMode.repeatStart = DebugFeature.CurrentPlayMsec;
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B7) && PractiseMode.repeatEnd == -1)
        {
            PractiseMode.SetRepeatEnd(DebugFeature.CurrentPlayMsec);
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B2))
        {
            PractiseMode.ClearRepeat();
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B6))
        {
            PractiseMode.SpeedDown();
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B3))
        {
            PractiseMode.SpeedUp();
        }
        else if (InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B5) || InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.B4))
        {
            PractiseMode.SpeedReset();
        }
        else if (
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A1) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A2) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A3) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A4) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A5) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A6) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A7) ||
            InputManager.GetTouchPanelAreaDown(InputManager.TouchPanelArea.A8)
        )
        {
            PractiseMode.ui = null;
            Destroy(this);
        }
    }
}
