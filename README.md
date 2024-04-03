# Self Sufficient
***TO BE CLEAR THE [VIRALITY](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality/) MOD IS REQUIRED IF YOU WANT TO HAVE 5+ PLAYER MATCHES WORK WITHIN CONTENT WARNING.***

Photon has put a cap onto landfalls Pun/Voice server instance that only allows for matches of 4 people or less to be made.
This causes issues with the Virality mod when larger than 4 player lobbys are used as it cannot start a lobby.
What this mod does is it allows you, as the user, to replace the Photon appID's that the game uses with your own.
This is done by changing 2 values within the config (*RootGameDirectory*\BepInEx\config\SelfSufficient.cfg) to your own appID's.
To get your own appID's you will need to create a Photon account and create a new app. This is a simple process and is free.

## Installation
1. Install BepInEx, and run the game once to generate the BepInEx folder.
2. Install the [Virality](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality/) mod. (If you want to play with 5+ people in content warning.)
3. Download the latest release DLL of Self Sufficient from the [Releases](https://github.com/C0mputery/SelfSufficient/releases) page.
4. Put the dll into the plugins folder in the BepInEx folder.
5. Run the game once to generate the config file.
6. Edit the config file with your own appID's. (See [Configuration](#Configuration) for more information.)
7. That's it!

## Configuration
The config file is located in the BepInEx\config folder and is called SelfSufficient.cfg.
You will need to edit this file with your own appID's. To get these you will need to create a Photon account and create a new app.

1. To make/login to a photon account go to [Photon](https://id.photonengine.com/account/).
2. Once logged in you will need to create 2 new apps. This is done by clicking on the "Create a new App" button.
3. The first app will be for the Pun server. This is the appID that is used for the server that the game connects to	.
4. To chose the server type you will need to select "Pun" from the "Select Photon SDK" dropdown.
5. The second app will be for the Voice server. This is the appID that is used for the voice chat that the game uses.
6. To chose the server type you will need to select "Voice" from the "Select Photon SDK" dropdown.
7. Once you have created the apps you will need to copy the respective appID's into the config file	.
8. After you have done this you can save and close the file.

