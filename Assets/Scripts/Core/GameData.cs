using UnityEngine;


[System.Serializable]
public class GameData
{
    
    private double[] razakusPurchases;
    private float[] playerStatMaxValues = new float[8];
    private int[] currencies = new int[2];

    public GameData()
    {
        
        razakusPurchases = RazakusMenuScript.MyInstance.GetRazakusPurchases();

        PlayerScript playerScript = PlayerScript.MyInstance;

        playerStatMaxValues[0] = playerScript.attack.MyMaxValue;
        playerStatMaxValues[1] = playerScript.life.MyMaxValue;
        playerStatMaxValues[2] = playerScript.range.MyMaxValue;
        playerStatMaxValues[3] = playerScript.movementSpeed.MyMaxValue;
        playerStatMaxValues[4] = playerScript.attackSpeed.MyMaxValue;
        playerStatMaxValues[5] = playerScript.critChance.MyMaxValue;
        playerStatMaxValues[6] = playerScript.critDamage.MyMaxValue;
        playerStatMaxValues[7] = playerScript.knockback.MyMaxValue;

        currencies[0] = CurrenciesScript.MyInstance.getGoldValue();
        currencies[1] = CurrenciesScript.MyInstance.getSoulsNumber();
    }
    
    public double[] GetRazakusPurchases()
    {
        return razakusPurchases;
    }

    public float[] GetPlayerStatMaxValues()
    {
        return playerStatMaxValues;
    }

    public int[] GetCurrencies()
    {
        return currencies;
    }

}
