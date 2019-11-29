using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LookingGlass;

public class View_ColorConfigurator : MonoBehaviour
{
    private Model_ScoreAndDifficulty sadModel;

    [ColorUsage(false, true)]
    public Color colorNormal;
    [ColorUsage(false, true)]
    public Color colorHappy;

    private float normalHue = 5;
    private float happyHue = 270;

    public Material clouds;
    public Material enemyBody;
    public Material enemyAccent;
    public Material enemyBullet;
    public Material enemyExplosion;

    public Camera[] cameras;
    public Holoplay holoplay;

    private int counter;

    private Vector2 sv_SkyNormal = new Vector2(80, 26);

    private Vector2 sv_CloudsHighlight = new Vector2(50, 83);
    private Vector2 sv_CloudsMidlight = new Vector2(87, 80);
    private Vector2 sv_CloudsShadow = new Vector2(86, 44);

    private Vector2 sv_BodyHighlight = new Vector2(100, 67);
    private Vector2 sv_BodyMidlight = new Vector2(100, 44);
    private Vector2 sv_BodyShadow = new Vector2(73, 22);
    private Vector2 sv_Accent = new Vector2(100, 87);
    private Vector2 sv_BulletOutter = new Vector2(93, 75); //6.75HDR
    private Vector2 sv_BulletInner = new Vector2(0, 0);
    private Vector2 sv_ExplosionOutter = new Vector2(100, 100);
    private Vector2 sv_ExplosionInner = new Vector2(0, 100);

    public void Awake()
    {
        sadModel = ServiceLocator.instance.Model.GetComponent<Model_ScoreAndDifficulty>();
    }

    public void SetColors(AestheticMode mode)
    {
        float hue;
        if (mode == AestheticMode.Normal)
        {
            hue = normalHue/360;
            holoplay.background = colorNormal;
        }
        else if (mode == AestheticMode.Happy)
        {
            hue = happyHue/360;
            holoplay.background = colorHappy;
        }
        else
        {
            hue = (((normalHue - sadModel.level * 6) / 360) + 1) % 1;
            Debug.Log(hue);
            holoplay.background = Color.HSVToRGB(hue, sv_SkyNormal.x / 100, sv_SkyNormal.y / 100);
        }

        clouds.SetColor("_Highlight", Color.HSVToRGB(hue, sv_CloudsHighlight.x / 100, sv_CloudsHighlight.y / 100));
        clouds.SetColor("_Midlight", Color.HSVToRGB(hue, sv_CloudsMidlight.x / 100, sv_CloudsMidlight.y / 100));
        clouds.SetColor("_Shadow", Color.HSVToRGB(hue, sv_CloudsShadow.x / 100, sv_CloudsShadow.y / 100));

        enemyAccent.SetColor("_Emissive", Color.HSVToRGB(hue, sv_Accent.x / 100, sv_Accent.y / 100));

        enemyBody.SetColor("_Highlight", Color.HSVToRGB(hue, sv_BodyHighlight.x / 100, sv_BodyHighlight.y / 100));
        enemyBody.SetColor("_Midlight", Color.HSVToRGB(hue, sv_BodyMidlight.x / 100, sv_BodyMidlight.y / 100));
        enemyBody.SetColor("_Shadow", Color.HSVToRGB(hue, sv_BodyShadow.x / 100, sv_BodyShadow.y / 100));

        Vector4 hdr = (Vector4) Color.HSVToRGB(hue, sv_BulletOutter.x / 100, 190 * sv_BulletOutter.y / 100);
        enemyBullet.SetColor("_LanceOutter", hdr);
        enemyBullet.SetColor("_LanceInner", Color.HSVToRGB(hue, sv_BulletInner.x / 100, sv_BulletInner.y / 100));

        enemyExplosion.SetColor("_LanceOutter", Color.HSVToRGB(hue, sv_ExplosionOutter.x / 100, sv_ExplosionOutter.y / 100));
        enemyExplosion.SetColor("_LanceInner", Color.HSVToRGB(hue, sv_ExplosionInner.x / 100, sv_ExplosionInner.y / 100));

        foreach (Camera c in cameras)
        {
            if (c!= null)
            {
                if (mode == AestheticMode.Normal)
                {
                    c.backgroundColor = colorNormal;
                }
                else if (mode == AestheticMode.Happy)
                {
                    c.backgroundColor = colorHappy;
                }
                else
                {
                    c.backgroundColor = Color.HSVToRGB(hue, sv_SkyNormal.x / 100, sv_SkyNormal.y / 100);
                }
            }
        }
        RenderSettings.fogColor = holoplay.background;
    }
}

public enum AestheticMode { Normal, Happy, Shift}
