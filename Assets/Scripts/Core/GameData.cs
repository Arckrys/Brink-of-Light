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

        playerStatMaxValues[0] = playerScript.initAttack;
        playerStatMaxValues[1] = playerScript.initLife;
        playerStatMaxValues[2] = playerScript.initRange;
        playerStatMaxValues[3] = playerScript.initMovementSpeed;
        playerStatMaxValues[4] = playerScript.initAttackSpeed;
        playerStatMaxValues[5] = playerScript.initCritChance;
        playerStatMaxValues[6] = playerScript.initCritDamage;
        playerStatMaxValues[7] = playerScript.initKnockback;

        currencies[0] = CurrenciesScript.MyInstance.GetGoldValue();
        currencies[1] = CurrenciesScript.MyInstance.GetSoulsNumber();
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
