using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class LightSwitchGlassGameLevelTest
{
    [UnityTest]
    public IEnumerator ControllerTest() {
        GameObject gameGo = new();
        GameObject glass = new();
        SpriteRenderer paddingRenderer = glass.AddComponent<SpriteRenderer>();
        LightSwitchGlassGame game = gameGo.AddComponent<LightSwitchGlassGame>();
        Sprite[] paddingSprites = { null, null, null, null, null };
        game.glassTextures = paddingSprites;
        game.glass = glass;

        GameObject controllerGo = new();
        GlassTap controller = controllerGo.AddComponent<GlassTap>();
        controller.game = game;

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(game.tapCount == 0);

        controller.OnClick(new TouchData(Vector3.zero, 0, controllerGo, 1));

        Assert.IsTrue(game.tapCount == 1);
    }

    [UnityTest]
    public IEnumerator Level() {
        GameObject gameGo = new();
        GameObject glass = new();
        SpriteRenderer paddingRenderer = glass.AddComponent<SpriteRenderer>();
        LightSwitchGlassGame game = gameGo.AddComponent<LightSwitchGlassGame>();
        Sprite[] paddingSprites = { null, null, null, null, null };
        game.glassTextures = paddingSprites;
        game.glass = glass;

        GameObject controllerGo = new();
        GlassTap controller = controllerGo.AddComponent<GlassTap>();
        controller.game = game;

        yield return new WaitForSeconds(0.1f);
        TouchData testTouch = new TouchData(Vector3.zero, 0, controllerGo, 1);
        for (int i = 0; i < game.tapPerLevel; i++) {
           Assert.IsTrue(game.currentLevel == 0);
           controller.OnClick(testTouch);
        }

        Assert.IsTrue(game.currentLevel == 1);
    }

    [UnityTest]
    public IEnumerator Break() {
        GameObject gameGo = new();
        GameObject glass = new();
        SpriteRenderer paddingRenderer = glass.AddComponent<SpriteRenderer>();
        LightSwitchGlassGame game = gameGo.AddComponent<LightSwitchGlassGame>();
        Sprite[] paddingSprites = { null, null, null, null, null };
        game.glassTextures = paddingSprites;
        game.glass = glass;

        GameObject controllerGo = new();
        GlassTap controller = controllerGo.AddComponent<GlassTap>();
        controller.game = game;

        yield return new WaitForSeconds(0.1f);

        TouchData testTouch = new TouchData(Vector3.zero, 0, controllerGo, 1);
        for(int j = 0; j < game.levels; j++) {
            for (int i = 0; i < game.tapPerLevel; i++) {
                Assert.IsTrue(glass.activeSelf);
                controller.OnClick(testTouch);
            }

        }

        Assert.IsFalse(glass.activeSelf);
    }
}
