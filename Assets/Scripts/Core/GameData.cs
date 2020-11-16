using UnityEngine;


[System.Serializable]
public class GameData
{
    
    private double[] razakusPurchases;
    public GameData()
    {
        razakusPurchases = RazakusMenuScript.MyInstance.GetRazakusPurchases();
    }

    public double[] getRazakusPurchases()
    {
        return razakusPurchases;
    }
}
