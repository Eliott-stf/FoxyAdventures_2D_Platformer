using UnityEngine;
using System.Collections.Generic;

//State des valeurs initiale du joueur 
public static class PlayerState
{
    public static int numberOfJumpsAllowed = 1;
    public static int numberOfDashes = 0;
    public static int currentHealth = 3;
    //State pour que la porte dans la scene de nuit soit déja débloqué pour retourner dans la cave 
    public static bool caveDoorsUnlocked = false;

    public static bool frontDoorUnlocked = false;

    //State pour que l'intro ne se rejoue pas quand on retourne dans la scène 
    public static bool introPlayed = false;

    public static string doorExitSceneName = "TestTransition";

    public static HashSet<string> collectedItems = new HashSet<string>();
    public static HashSet<string> killedEnemies = new HashSet<string>();
}
