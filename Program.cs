using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private const int CADRE = 10;
    private const int TOUCHE = 2;
    private const int ARRET = 50;
    private const int PASSE_DECISIVE = 50;
    private const int BONUS_SAUVE_MIRACLE = 25;
    private const int BUT = 100;

    private class StatistiquesJoueur
    {
        public int NbTouche { get; set; }
        public int NbCadre { get; set; }
        public int NbArret { get; set; }
        public int NbMiracle { get; set; }
        public int NbDecisive { get; set; }
        public int NbBut { get; set; }
        public int ScoreArrets { get; set; }
        public int ScoreTotal { get; set; }
    }

    private static Dictionary<string, StatistiquesJoueur> statistiquesJoueurs = new Dictionary<string, StatistiquesJoueur>();

    static void Main()
    {
        Bienvenue();
        SaisirDonneesJoueurs();
        AfficherResultats();
    }

    private static void Bienvenue()
    {
        Console.WriteLine("Bienvenue sur RL Performance Tracker !");
        Console.WriteLine();
        Console.WriteLine("Ce programme vous permet d'analyser les performances des joueurs de Rocket League en détail.");
        Console.WriteLine();
        Console.WriteLine("Vous allez pouvoir saisir les statistiques des joueurs, incluant :");
        Console.WriteLine("- Les touches de balle");
        Console.WriteLine("- Les tirs cadrés");
        Console.WriteLine("- Les arrêts et sauvetages miraculeux");
        Console.WriteLine("- Les passes décisives");
        Console.WriteLine("- Les buts");
        Console.WriteLine();
        Console.WriteLine("RL Performance Tracker calculera automatiquement les scores et déterminera :");
        Console.WriteLine("- Le meilleur joueur global (l'homme du match)");
        Console.WriteLine("- Le meilleur buteur");
        Console.WriteLine("- Le meilleur passeur");
        Console.WriteLine("- Le meilleur gardien");
        Console.WriteLine();
        Console.WriteLine("Préparez-vous à entrer dans l'univers des statistiques de Rocket League !");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Appuyez sur n'importe quelle touche pour démarrer");
        Console.ReadKey();
        Console.Clear();
    }

    private static void SaisirDonneesJoueurs()
    {
        int joueurCount = 0;
        bool continuer = true;

        while (continuer)
        {
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"Entrez le nom du joueur {joueurCount + 1} : ");
                string nomJoueur = Console.ReadLine();
                if (string.IsNullOrEmpty(nomJoueur))
                {
                    continuer = false;
                    break;
                }

                SaisirStatistiquesJoueur(nomJoueur);
                joueurCount++;
            }

            if (continuer && joueurCount % 2 == 0)
            {
                Console.WriteLine("Voulez-vous ajouter d'autres joueurs ? (O/N)");
                continuer = Console.ReadLine().Trim().ToUpper() == "O";
            }
        }

        if (joueurCount % 2 != 0)
        {
            Console.WriteLine("Attention : Le nombre de joueurs est impair. Cela peut ne pas correspondre aux modes de jeu standard de Rocket League.");
        }
    }

    private static void SaisirStatistiquesJoueur(string nomJoueur)
    {
        var stats = new StatistiquesJoueur
        {
            NbTouche = DemanderNombre($"Combien de fois {nomJoueur} a-t-il touché le ballon ?"),
            NbCadre = DemanderNombre($"Combien de tirs cadrés {nomJoueur} a-t-il réalisés ?"),
            NbArret = DemanderNombre($"Combien d'arrêts {nomJoueur} a-t-il réalisés ?"),
            NbDecisive = DemanderNombre($"Combien de passes décisives {nomJoueur} a-t-il offertes ?"),
            NbBut = DemanderNombre($"Combien de buts {nomJoueur} a-t-il marqués ?")
        };

        Console.WriteLine($"Parmi les {stats.NbArret} arrêts, combien y avait-il de sauvetages miraculeux ?");
        stats.NbMiracle = DemanderNombre("Nombre de sauvetages miraculeux : ");

        while (stats.NbMiracle > stats.NbArret)
        {
            Console.WriteLine("Le nombre de sauvetages miraculeux ne peut pas être supérieur au nombre total d'arrêts.");
            stats.NbMiracle = DemanderNombre("Nombre de sauvetages miraculeux : ");
        }

        stats.ScoreArrets = (stats.NbArret * ARRET) + (stats.NbMiracle * BONUS_SAUVE_MIRACLE);
        stats.ScoreTotal = CalculerScore(stats);

        if (!statistiquesJoueurs.ContainsKey(nomJoueur))
            statistiquesJoueurs[nomJoueur] = stats;
        else
        {
            var existingStats = statistiquesJoueurs[nomJoueur];
            existingStats.NbTouche += stats.NbTouche;
            existingStats.NbCadre += stats.NbCadre;
            existingStats.NbArret += stats.NbArret;
            existingStats.NbMiracle += stats.NbMiracle;
            existingStats.NbDecisive += stats.NbDecisive;
            existingStats.NbBut += stats.NbBut;
            existingStats.ScoreArrets += stats.ScoreArrets;
            existingStats.ScoreTotal += stats.ScoreTotal;
        }
    }

    private static int DemanderNombre(string question)
    {
        Console.WriteLine(question);
        return int.Parse(Console.ReadLine());
    }

    private static int CalculerScore(StatistiquesJoueur stats)
    {
        return (TOUCHE * stats.NbTouche) + (CADRE * stats.NbCadre) + stats.ScoreArrets +
               (PASSE_DECISIVE * stats.NbDecisive) + (BUT * stats.NbBut);
    }

    private static void AfficherResultats()
    {
        AfficherMeilleurJoueur("Le meilleur joueur", j => j.ScoreTotal);
        AfficherMeilleurJoueur("Le meilleur buteur", j => j.NbBut);
        AfficherMeilleurJoueur("Le meilleur passeur", j => j.NbDecisive);
        AfficherMeilleurJoueur("Le meilleur gardien", j => j.NbArret);
    }

    private static void AfficherMeilleurJoueur(string titre, Func<StatistiquesJoueur, int> critereFn)
    {
        var meilleur = statistiquesJoueurs.OrderByDescending(kvp => critereFn(kvp.Value)).First();
        Console.WriteLine($"{titre} est : {meilleur.Key} avec un score de {critereFn(meilleur.Value)}");
    }
}
