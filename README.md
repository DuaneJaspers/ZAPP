# ZAPP
<img src="ExtraImages/appointments.png"
     alt="Main screen"/>  

### Uitleg
ZAPP is een case waarbij het de bedoeling is om een Zorgapp te maken die 1ste en 2de lijns-zorgverleners helpt hun taken uit te voeren bij cliënten.

### Functionele beschrijving
- Wanneer de app voor de eerste keer gestart wordt kan een gebruiker inloggen met een Username en Password, na een succesvolle login hoeft er niet opnieuw ingelogd te worden.
- De gebruiker wordt een overzicht van cliënten getoond waar taken moeten worden uitgevoerd en op welke tijdstippen. Ook wordt er alvast de taken van de volgende dag getoond.
- De gebruiker kan op een cliënt klikken om door te gaan naar een detailpagina waar de taken zichtbaar zijn voor die specifieke cliënt, hier is ook ruimte voor een opmerking van de planners.
- op de detailpagina kan de gebruiker navigeren naar het adres tablad waar de gebruiker de adresgegevens van de cliënt kan bekijken.
- De gebruiker kan zich aanmelden als deze bij de client is en kan vervolgens de taken een voor een afvinken, als deze taken voltooid zijn kan de gebruiker zich weer afmelden waarna de client uit de lijst van de overzichtspagina verdwijnt. 

<img src="ExtraImages/login.png"
    alt="login screen">

## Applicatieserver 
Voor de applicatieserver wordt Cockpit CMS gebruikt, een self-hosted headless en api-driven CMS. Deze draait op een locaal netwerk en kan de app voorzien van data.

## Tools
Dit is een Android app geschreven in C# met het Xamarin platform