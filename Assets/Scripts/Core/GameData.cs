[System.Serializable]
public class GameData
{
    private double[] razakusPurchases;
    private float[] playerStatMaxValues = new float[8];
    private int[] currencies = new int[2];
    private int urbiusLevel;
    private int igeirusLevel;

    // Initiate data to save (stats, seller purchases and currencies)
    public GameData()
    {
        razakusPurchases = RazakusMenuScript.MyInstance ? RazakusMenuScript.MyInstance.GetRazakusPurchases() : null;

        PlayerScript playerScript = PlayerScript.MyInstance;

        playerStatMaxValues[0] = playerScript.initAttack;
        playerStatMaxValues[1] = playerScript.initLife;
        playerStatMaxValues[2] = playerScript.initRange;
        playerStatMaxValues[3] = playerScript.initMovementSpeed;
        playerStatMaxValues[4] = playerScript.initAttackSpeed;
        playerStatMaxValues[5] = playerScript.initCritChance;
        playerStatMaxValues[6] = playerScript.initCritDamage;
        playerStatMaxValues[7] = playerScript.initKnockback;

        urbiusLevel = playerScript.MyUrbiusLevel;
        igeirusLevel = playerScript.MyIgeirusLevel;

        if (CurrenciesScript.MyInstance)
        {
            currencies[0] = CurrenciesScript.MyInstance.GetGoldValue();
            currencies[1] = CurrenciesScript.MyInstance.GetSoulsNumber();
        }
        else
        {
            currencies[0] = 0;
            currencies[1] = 0;
        }
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

    public void SetCurrencies(int gold, int souls)
    {
        //currencies[0] = gold;
        currencies[1] = souls;
    }

    public int GetUrbiusLevel()
    {
        return urbiusLevel;
    }
    
    public int GetIgeirusLevel()
    {
        return igeirusLevel;
    }
}
