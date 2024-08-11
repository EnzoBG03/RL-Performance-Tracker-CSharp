# RL Performance Tracker

## Description
RL Performance Tracker est une application C# qui permet de suivre et d'analyser les performances des joueurs dans Rocket League. Il calcule les scores en fonction de diverses actions de jeu et détermine les meilleurs joueurs dans différentes catégories.

## Fonctionnalités
- Saisie des statistiques de jeu pour plusieurs joueurs
- Calcul des scores basé sur différentes actions (touches de balle, tirs cadrés, arrêts, sauvetages miraculeux, passes décisives, buts)
- Détermination des meilleurs joueurs dans les catégories suivantes :
  - Meilleur joueur global (homme du match)
  - Meilleur buteur
  - Meilleur passeur
  - Meilleur gardien
- Prise en compte spéciale des sauvetages miraculeux dans le calcul du score des gardiens (Nouvelle fonctionnalité)
- Saisie des joueurs par paires pour correspondre aux différents modes de jeu (1v1, 2v2, 3v3 ou 4v4) (Nouvelle fonctionnalité)

## Comment utiliser
1. Exécutez le programme
2. Suivez les instructions à l'écran pour saisir les noms des joueurs et leurs statistiques
3. Le programme vous demandera si vous souhaitez ajouter d'autres joueurs après chaque paire
4. Une fois toutes les données saisies, le programme affichera les meilleurs joueurs dans chaque catégorie

## Système de points
- Touche de balle : 2 points
- Tir cadré : 10 points
- Arrêt : 50 points
- Sauvetage miraculeux : 75 points (50 points d'arrêt + 25 points de bonus)
- Passe décisive : 50 points
- But : 100 points

## Prérequis
- Infrastructure .NET (version 7.0 au minimum (le projet a été conçu dans cette version))

## Installation
1. Clonez ce dépôt
2. Ouvrez la solution dans Visual Studio
3. Compilez et exécutez le programme

## Contribution
Les contributions à ce projet sont les bienvenues. N'hésitez pas à forker le projet et à soumettre vos pull requests.

## Versions
- v2.0 : Refonte majeure de la structure du code et ajout de nouvelles fonctionnalités
- v1.1 : Réorganisation du code en fonctions
- v1.0 : Version initiale

## Auteur
Enzo BENOIST-GIMET
