using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MorseLevelTest
{
    [UnityTest]
    public IEnumerator Dot() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime - 0.1f);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.morseObejcts.Count > 0);
        Assert.AreEqual(game.morseString[0], MorseGame.MorseType.DOT);

    }
    [UnityTest]
    public IEnumerator Dash() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.morseObejcts.Count > 0);
        Assert.AreEqual(game.morseString[0], MorseGame.MorseType.DASH);
    }

    [UnityTest]
    public IEnumerator RetryButton() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        GameObject rGo = new();
        MorseRetryButton r = rGo.AddComponent<MorseRetryButton>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.morseObejcts.Count > 0);
        r.OnClick(null);
        Assert.IsTrue(game.morseObejcts.Count == 0);
    }


    [UnityTest]
    public IEnumerator SOSWin() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;
        game.longPressTime = 0.2f;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        GameObject rGo = new();
        MorseRetryButton r = rGo.AddComponent<MorseRetryButton>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        Assert.IsFalse(game.gameInProgress);
    }


    [UnityTest]
    public IEnumerator NotSOSNotWinV1() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;
        game.longPressTime = 0.2f;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        GameObject rGo = new();
        MorseRetryButton r = rGo.AddComponent<MorseRetryButton>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.gameInProgress);
    }


    [UnityTest]
    public IEnumerator NotSOSNotWinV2() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;
        game.longPressTime = 0.2f;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        GameObject rGo = new();
        MorseRetryButton r = rGo.AddComponent<MorseRetryButton>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);
        Assert.IsTrue(game.gameInProgress);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.gameInProgress);
    }

    [UnityTest]
    public IEnumerator NotSOSNotWinV3() {
        GameObject paddigObj = new();

        GameObject gameGo = new();
        MorseGame game = gameGo.AddComponent<MorseGame>();
        game.dotPrefab = paddigObj;
        game.dashPrefab = paddigObj;
        game.longPressTime = 0.2f;

        GameObject controllerGo = new();
        MorseTouch controller = controllerGo.AddComponent<MorseTouch>();

        GameObject rGo = new();
        MorseRetryButton r = rGo.AddComponent<MorseRetryButton>();

        yield return new WaitForSeconds(0.1f);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(game.longPressTime);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);
        controller.OnTouchBegin(null);

        yield return new WaitForSeconds(0.01f);

        controller.OnTouchEnd(null);

        Assert.IsTrue(game.gameInProgress);
    }
}
