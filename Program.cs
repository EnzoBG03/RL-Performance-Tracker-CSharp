using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private const int CADRE = 10;
    private const int TOUCHE = 2;
    private const int ARRET = 50;
    private const int PASSE_DECISIVE = 50;
    private const int SAUVE_MIRACLE = 75;
    private const int BUT = 100;

    private static Dictionary<string, int> scores = new Dictionary<string, int>();
    private static Dictionary<string, int> arrets = new Dictionary<string, int>();
    private static Dictionary<string, int> decisives = new Dictionary<string, int>();
    private static Dictionary<string, int> buts = new Dictionary<string, int>();

    static void Main()
    {
        SaisirDonneesJoueurs();
        AfficherResultats();
    }

    private static void SaisirDonneesJoueurs()
    {
        while (true)
        {
            Console.WriteLine("Entrez le nom du joueur (ou appuyez sur Entrée pour terminer) : ");
            string nomJoueur = Console.ReadLine();
            if (string.IsNullOrEmpty(nomJoueur)) break;

            int score = CalculerScoreJoueur(nomJoueur);
            MettreAJourStatistiques(nomJoueur, score);
        }
    }

    private static int CalculerScoreJoueur(string nomJoueur)
    {
        int nbTouche = DemanderNombre($"Combien de fois {nomJoueur} a-t-il touché le ballon ?");
        int nbCadre = DemanderNombre($"Combien de tirs cadrés {nomJoueur} a-t-il réalisés ?");
        int nbArret = DemanderNombre($"Combien d'arrêts {nomJoueur} a-t-il réalisés ?");
        int nbMiracle = DemanderNombre($"Combien de sauvetages miraculeux {nomJoueur} a-t-il réalisés ?");
        int nbDecisive = DemanderNombre($"Combien de passes décisives {nomJoueur} a-t-il offertes ?");
        int nbBut = DemanderNombre($"Combien de buts {nomJoueur} a-t-il marqués ?");

        return CalculerScore(nbTouche, nbCadre, nbArret, nbMiracle, nbDecisive, nbBut);
    }

    private static int DemanderNombre(string question)
    {
        Console.WriteLine(question);
        return int.Parse(Console.ReadLine());
    }

    private static int CalculerScore(int nbTouche, int nbCadre, int nbArret, int nbMiracle, int nbDecisive, int nbBut)
    {
        return (TOUCHE * nbTouche) + (CADRE * nbCadre) + (ARRET * nbArret) + 
               (SAUVE_MIRACLE * nbMiracle) + (PASSE_DECISIVE * nbDecisive) + (BUT * nbBut);
    }

    private static void MettreAJourStatistiques(string nomJoueur, int score)
    {
        MettreAJourDictionnaire(scores, nomJoueur, score);
        MettreAJourDictionnaire(arrets, nomJoueur, score / ARRET);
        MettreAJourDictionnaire(decisives, nomJoueur, score / PASSE_DECISIVE);
        MettreAJourDictionnaire(buts, nomJoueur, score / BUT);
    }

    private static void MettreAJourDictionnaire(Dictionary<string, int> dict, string cle, int valeur)
    {
        if (!dict.ContainsKey(cle))
            dict[cle] = valeur;
        else
            dict[cle] += valeur;
    }

    private static void AfficherResultats()
    {
        AfficherMeilleurJoueur("Le meilleur joueur", scores);
        AfficherMeilleurJoueur("Le meilleur buteur", buts);
        AfficherMeilleurJoueur("Le meilleur passeur", decisives);
        AfficherMeilleurJoueur("Le meilleur gardien", arrets);
    }

    private static void AfficherMeilleurJoueur(string titre, Dictionary<string, int> dict)
    {
        var meilleur = dict.OrderByDescending(x => x.Value).First();
        Console.WriteLine($"{titre} est : {meilleur.Key} avec un score de {meilleur.Value}");
    }
}
