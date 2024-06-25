﻿using System.Linq;
using HarmonyLib;
using MAI2.Util;
using Manager;
using MelonLoader;
using Monitor.Game;
using UnityEngine;

namespace AquaMai.UX;

public class LoadLocalBga
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameCtrl), "IsReady")]
    public static void LoadLocalBgaAwake(GameObject ____movieMaskObj)
    {
        var components = ____movieMaskObj.GetComponentsInChildren<Component>(false);
        var movie = components.FirstOrDefault(it => it.name == "Movie");
        if (movie is null) return;

        var music = Singleton<DataManager>.Instance.GetMusic(GameManager.SelectMusicID[0]);
        var moviePath = string.Format(Singleton<OptionDataManager>.Instance.GetMovieDataPath($"{music.movieName.id:000000}") + ".dat");
        if (!moviePath.Contains("dummy")) return;

        var jacket = LoadJacketPng.GetJacketTexture2D(music.movieName.id);
        if (jacket is null)
        {
            MelonLogger.Msg("No jacket found for music " + music);
        }
        else
        {
            // If I create a new RawImage component, the jacket will be not be displayed
            // I think it will be difficult to make it work with RawImage
            // So I change the material that plays video to default sprite material
            // The original player is actually a sprite renderer and plays video with a custom material
            var sprite = movie.GetComponent<SpriteRenderer>();
            sprite.sprite = Sprite.Create(jacket, new Rect(0, 0, jacket.width, jacket.height), new Vector2(0.5f, 0.5f));
            sprite.material = new Material(Shader.Find("Sprites/Default"));
        }

        // movie.gameObject.SetActive(false);
    }
}
