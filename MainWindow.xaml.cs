using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;

namespace RL_Performance_Calculator
{
    public partial class MainWindow : Window
    {
        // Conservation des constantes du programme original
        private const int CADRE = 10;
        private const int TOUCHE = 2;
        private const int ARRET = 50;
        private const int PASSE_DECISIVE = 50;
        private const int BONUS_SAUVE_MIRACLE = 25;
        private const int BUT = 100;

        // Classe StatistiquesJoueur du programme original avec ajout du nom
        public class StatistiquesJoueur : INotifyPropertyChanged
        {
            public string Nom { get; set; }
            public int NbTouche { get; set; }
            public int NbCadre { get; set; }
            public int NbArret { get; set; }
            public int NbMiracle { get; set; }
            public int NbDecisive { get; set; }
            public int NbBut { get; set; }
            public int ScoreArrets { get; set; }
            public int ScoreTotal { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        // Dictionnaire conservé du programme original
        private Dictionary<string, StatistiquesJoueur> statistiquesJoueurs = new Dictionary<string, StatistiquesJoueur>();

        public MainWindow()
        {
            InitializeComponent();

            // Animation d'entrée pour la fenêtre
            this.Opacity = 0;
            var fadeIn = (Storyboard)FindResource("FadeInAnimation");
            fadeIn.Begin(this);
        }

        private void CommencerSaisie_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = InputTab;
            InputTab.IsEnabled = true;

            // Animation d'entrée pour l'onglet de saisie
            AnimateTabEntry();

            NomJoueurTextBox.Focus();
        }

        private void AnimateTabEntry()
        {
            // Animation du formulaire depuis la gauche
            var slideInLeft = (Storyboard)FindResource("SlideInFromLeft");
            slideInLeft.Begin(FormulaireGroup);

            // Animation de la liste depuis la droite avec un léger délai
            var slideInRight = (Storyboard)FindResource("SlideInFromRight");
            slideInRight.BeginTime = TimeSpan.FromMilliseconds(200);
            slideInRight.Begin(ListeGroup);
        }

        private void AjouterJoueur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageTextBlock.Text = "";

                // Validation du nom
                string nomJoueur = NomJoueurTextBox.Text.Trim();
                if (string.IsNullOrEmpty(nomJoueur))
                {
                    MessageTextBlock.Text = "Veuillez entrer un nom de joueur.";
                    return;
                }

                // Validation et conversion des statistiques
                if (!int.TryParse(TouchesTextBox.Text, out int nbTouche) || nbTouche < 0)
                {
                    MessageTextBlock.Text = "Le nombre de touches doit être un nombre entier positif.";
                    return;
                }

                if (!int.TryParse(CadresTextBox.Text, out int nbCadre) || nbCadre < 0)
                {
                    MessageTextBlock.Text = "Le nombre de tirs cadrés doit être un nombre entier positif.";
                    return;
                }

                if (!int.TryParse(ArretsTextBox.Text, out int nbArret) || nbArret < 0)
                {
                    MessageTextBlock.Text = "Le nombre d'arrêts doit être un nombre entier positif.";
                    return;
                }

                if (!int.TryParse(MiraclesTextBox.Text, out int nbMiracle) || nbMiracle < 0)
                {
                    MessageTextBlock.Text = "Le nombre de sauvetages miraculeux doit être un nombre entier positif.";
                    return;
                }

                if (!int.TryParse(PassesTextBox.Text, out int nbDecisive) || nbDecisive < 0)
                {
                    MessageTextBlock.Text = "Le nombre de passes décisives doit être un nombre entier positif.";
                    return;
                }

                if (!int.TryParse(ButsTextBox.Text, out int nbBut) || nbBut < 0)
                {
                    MessageTextBlock.Text = "Le nombre de buts doit être un nombre entier positif.";
                    return;
                }

                // Validation logique : les miracles ne peuvent pas dépasser les arrêts
                if (nbMiracle > nbArret)
                {
                    MessageTextBlock.Text = "Le nombre de sauvetages miraculeux ne peut pas être supérieur au nombre total d'arrêts.";
                    return;
                }

                // Utilisation de la méthode SaisirStatistiquesJoueur adaptée
                var stats = new StatistiquesJoueur
                {
                    Nom = nomJoueur,
                    NbTouche = nbTouche,
                    NbCadre = nbCadre,
                    NbArret = nbArret,
                    NbMiracle = nbMiracle,
                    NbDecisive = nbDecisive,
                    NbBut = nbBut
                };

                // Calculs conservés du programme original
                stats.ScoreArrets = (stats.NbArret * ARRET) + (stats.NbMiracle * BONUS_SAUVE_MIRACLE);
                stats.ScoreTotal = CalculerScore(stats);

                // Gestion des joueurs existants (logique du programme original)
                if (!statistiquesJoueurs.ContainsKey(nomJoueur))
                {
                    statistiquesJoueurs[nomJoueur] = stats;
                }
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

                // Mise à jour de l'interface avec animation
                MettreAJourListeJoueurs();
                ViderFormulaireJoueur();

                // Animation de succès pour le message
                var messageAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                MessageTextBlock.BeginAnimation(UIElement.OpacityProperty, messageAnimation);

                // Activer le bouton des résultats s'il y a au moins un joueur
                VoirResultatsButton.IsEnabled = statistiquesJoueurs.Count > 0;

                MessageTextBlock.Text = $"Joueur {nomJoueur} ajouté avec succès !";
                MessageTextBlock.Foreground = new SolidColorBrush(Colors.LimeGreen);
            }
            catch (Exception ex)
            {
                MessageTextBlock.Text = $"Erreur lors de l'ajout du joueur : {ex.Message}";
            }
        }

        // Méthode CalculerScore conservée du programme original
        private int CalculerScore(StatistiquesJoueur stats)
        {
            return (TOUCHE * stats.NbTouche) + (CADRE * stats.NbCadre) + stats.ScoreArrets +
                   (PASSE_DECISIVE * stats.NbDecisive) + (BUT * stats.NbBut);
        }

        private void ViderFormulaireJoueur()
        {
            NomJoueurTextBox.Text = "";
            TouchesTextBox.Text = "0";
            CadresTextBox.Text = "0";
            ArretsTextBox.Text = "0";
            MiraclesTextBox.Text = "0";
            PassesTextBox.Text = "0";
            ButsTextBox.Text = "0";
            NomJoueurTextBox.Focus();
        }

        private void MettreAJourListeJoueurs()
        {
            JoueursListView.ItemsSource = null;
            JoueursListView.ItemsSource = statistiquesJoueurs.Values.ToList();
        }

        private void VoirResultats_Click(object sender, RoutedEventArgs e)
        {
            if (statistiquesJoueurs.Count == 0)
            {
                MessageBox.Show("Aucun joueur n'a été ajouté.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Méthodes AfficherMeilleurJoueur adaptées du programme original
            AfficherResultats();

            ResultsTab.IsEnabled = true;
            MainTabControl.SelectedItem = ResultsTab;

            // Animation d'entrée pour les résultats
            var slideIn = (Storyboard)FindResource("SlideInFromLeft");
            slideIn.Begin(ResultatsPanel);
        }

        // Méthode AfficherResultats adaptée du programme original
        private void AfficherResultats()
        {
            if (statistiquesJoueurs.Count == 0) return;

            // Utilisation des méthodes du programme original adaptées pour WPF
            var meilleurJoueur = statistiquesJoueurs.OrderByDescending(kvp => kvp.Value.ScoreTotal).First();
            var meilleurButeur = statistiquesJoueurs.OrderByDescending(kvp => kvp.Value.NbBut).First();
            var meilleurPasseur = statistiquesJoueurs.OrderByDescending(kvp => kvp.Value.NbDecisive).First();
            var meilleurGardien = statistiquesJoueurs.OrderByDescending(kvp => kvp.Value.NbArret).First();

            MeilleurJoueurText.Text = $"🏆 Homme du match : {meilleurJoueur.Key} ({meilleurJoueur.Value.ScoreTotal} {(meilleurJoueur.Value.ScoreTotal > 1 ? "points" : "point")})";
            MeilleurButeurText.Text = $"⚽ Meilleur buteur : {meilleurButeur.Key} ({meilleurButeur.Value.NbBut} {(meilleurJoueur.Value.NbBut > 1 ? "buts" : "but")})";
            MeilleurPasseurText.Text = $"🎯 Meilleur passeur : {meilleurPasseur.Key} ({meilleurPasseur.Value.NbDecisive} {(meilleurJoueur.Value.NbDecisive > 1 ? "passes" : "passe")})";
            MeilleurGardienText.Text = $"🥅 Meilleur gardien : {meilleurGardien.Key} ({meilleurGardien.Value.NbArret} {(meilleurJoueur.Value.NbArret > 1 ? "arrêts" : "arrêt")})";

            // Mise à jour des statistiques détaillées
            StatistiquesDetailleesListView.ItemsSource = statistiquesJoueurs.Values.OrderByDescending(s => s.ScoreTotal).ToList();
        }

        private void RevenirAccueil_Click(object sender, RoutedEventArgs e)
        {
            // Retour simple à la page d'accueil sans réinitialiser les données
            MainTabControl.SelectedItem = WelcomeTab;

            // Animation d'entrée pour le retour à l'accueil
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            WelcomePanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }
    }
}