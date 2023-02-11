using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSelector : MonoBehaviour {
    private bool inProgress = false;

    IEnumerator SetLanguage(int id) { 
        inProgress = true;

        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
        
        inProgress = false;
    }

    public void ChangeLanguage(int id) {
        if (inProgress) return;

        StartCoroutine(SetLanguage(id));
    }
}