using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        const int cadre = 10, touche = 2, arret = 50, passe_d = 50, sauve_miracle = 75, but = 100;
        int nb_touche, nb_cadre, nb_arret, nb_miracle, tot_arrets, nb_decisive, nb_but;
        int pts_touche, pts_cadre, pts_arret, pts_miracle, pts_decisive, pts_but, score;
        Dictionary<string, int> scores = new Dictionary<string, int>();
        Dictionary<string, int> arrets = new Dictionary<string, int>();
        Dictionary<string, int> decisives = new Dictionary<string, int>();
        Dictionary<string, int> buts = new Dictionary<string, int>();

        Console.WriteLine("Entrez le nom du joueur : ");
        string Nom_joueur = Console.ReadLine();

        while (!string.IsNullOrEmpty(Nom_joueur))
        {
            Console.WriteLine("Combien de fois " + Nom_joueur + " a-t-il touché le ballon ?");
            nb_touche = int.Parse(Console.ReadLine());
            pts_touche = touche * nb_touche;
            Console.WriteLine("Combien de tirs cadrés " + Nom_joueur + " a-t-il réalisés ?");
            nb_cadre = int.Parse(Console.ReadLine());
            pts_cadre = cadre * nb_cadre;
            Console.WriteLine("Combien d'arrêts " + Nom_joueur + " a-t-il réalisés ? ");
            nb_arret = int.Parse(Console.ReadLine());
            pts_arret = arret * nb_arret;
            Console.WriteLine("Combien de sauvetages miraculeux " + Nom_joueur + " a-t-il réalisés ?");
            nb_miracle = int.Parse(Console.ReadLine());
            pts_miracle = sauve_miracle * nb_miracle;
            tot_arrets = pts_arret + pts_miracle;
            Console.WriteLine("Combien de passes décisives " + Nom_joueur + " a-t-il offertes ?");
            nb_decisive = int.Parse(Console.ReadLine());
            pts_decisive = passe_d * nb_decisive;
            Console.WriteLine("Combien de buts " + Nom_joueur + " a-t-il marqués ?");
            nb_but = int.Parse(Console.ReadLine());
            pts_but = but * nb_but;

            score = pts_touche + pts_cadre + tot_arrets + pts_decisive + pts_but;

            if (!scores.ContainsKey(Nom_joueur))
            {
                scores[Nom_joueur] = score;
            }
            else
            {
                scores[Nom_joueur] += score;
            }
            if (!arrets.ContainsKey(Nom_joueur))
            {
                arrets[Nom_joueur] = nb_arret + nb_miracle;
            }
            else
            {
                arrets[Nom_joueur] += nb_arret + nb_miracle;
            }
            if (!decisives.ContainsKey(Nom_joueur))
            {
                decisives[Nom_joueur] = nb_decisive;
            }
            else
            {
                decisives[Nom_joueur] += nb_decisive;
            }
            if (!buts.ContainsKey(Nom_joueur))
            {
                buts[Nom_joueur] = nb_but;
            }
            else
            {
                buts[Nom_joueur] += nb_but;
            }

            Console.WriteLine("Entrez le nom du joueur (ou appuyez sur Entrée pour terminer) : ");
            Nom_joueur = Console.ReadLine();
        }

        int meilleur_score = scores.Values.Max();
        string meilleur_joueur = scores.FirstOrDefault(x => x.Value == meilleur_score).Key;
        int meilleur_score_buts = buts.Values.Max();
        string meilleur_buteur = buts.FirstOrDefault(x => x.Value == meilleur_score_buts).Key;
        int meilleur_score_decisives = decisives.Values.Max();
        string meilleur_passeur = decisives.FirstOrDefault(x => x.Value == meilleur_score_decisives).Key;
        int meilleur_score_arrets = arrets.Values.Max();
        string meilleur_gardien = arrets.FirstOrDefault(x => x.Value == meilleur_score_arrets).Key;

        Console.WriteLine("Le meilleur joueur est : " + meilleur_joueur + " avec un score de " + meilleur_score);
        Console.WriteLine("Le meilleur buteur est : " + meilleur_buteur + " avec un score de " + meilleur_score_buts);
        Console.WriteLine("Le meilleur passeur est : " + meilleur_passeur + " avec un score de " + meilleur_score_decisives);
        Console.WriteLine("Le meilleur gardien est : " + meilleur_gardien + " avec un score de " + meilleur_score_arrets);
    }
}