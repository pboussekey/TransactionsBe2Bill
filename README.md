# TransactionsBe2Bill

Petite application C# permettant d'interroger les webservices de Be2Bill.

Il suffit de remplir le fichier App.config avec vos informations de compte.
L'application s'exécute en ligne de commande et prend un seul paramètre en entrée :
- Une date au format YYYY-MM-DD
- Un entier qui correspond au nombre de jour à retrancher à la date courante pour obtenir le jour souhaité
Par défaut l'application demande le fichier de l'avant veille si ce paramètre n'est pas renseigné.
