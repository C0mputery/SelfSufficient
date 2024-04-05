# Self Sufficient

## ***TO BE CLEAR THE [VIRALITY](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality/) MOD IS REQUIRED IF YOU WANT TO HAVE 5+ PLAYER MATCHES WORK WITHIN CONTENT WARNING.***

## Why This Mod Is Needed
Photon has put a cap onto landfalls Pun/Voice server instances that only allows for matches of 4 people or less to be made.
This causes issues with the Virality mod when matches larger than 4 player are used, since it cant make you know make the match.
What this mod does is allows for you, as the user, to replace the Photon appID that the game uses with one of your own.
This is done by changing a value within the config to your own appID.
To get your own appID you will need to create a Photon account and make a new app. This is a simple process and is free.
These free instances allow for upto 16 player matches.
Additionally this mod will automatically sync the appID of the host when a lobby is joined, given both clients are using the mod.

## Video Tutorial
- Video Tutorial (recommended): [Here](https://www.youtube.com/watch?v=8Q6J9Q1Q9ZQ)

## Installation for client (This is without using a mod manager. If you use something like R2ModMan just install it like anything else).
1. Install BepInEx, and run the game once to generate the BepInEx folder.
2. Install the [Virality](https://thunderstore.io/c/content-warning/p/MaxWasUnavailable/Virality/) mod.
3. Download the latest release dll of Self Sufficient from the [Releases](https://github.com/C0mputery/SelfSufficient/releases) page on GitHub or ThunderStore[https://thunderstore.io/c/content-warning/p/Computery/Self_Sufficient/].
4. Done! That's all that's needed if you are not hosting the mod.

## Installation for host.
***HOSTING WITH YOUR OWN APPID MAY CAUSE YOUR PHOTON ACCOUNT LINKED WITH SAID APPID TO BE TERMINATED, USE AT YOUR OWN RISK*** </br>
So far no account has been terminated because of this mod. </br>
1. Do everything that the setup client does.
2. Put the dll into the plugins folder in the BepInEx folder.
3. Run the game once to generate the config file.
4. Edit the config file with your own appID. (See [Configuration](#Configuration) for more information.)
6. Make sure to restart your game or reload the menu.
7. That's it, have fun hosting 5+ player games.

## Configuration
The config file is located at *RootGameDirectory*\BepInEx\config\Computery.SelfSufficient.cfg.
You will need to edit this file and add your own appID. To get these you will need to create a Photon account and make a new app.

1. To make/login to a photon account go to [Photon Signin](https://id.photonengine.com/account/).
2. Once logged in you will need to create one new app. This is done by clicking on the "Create a new App" button.
4. Select "Pun" from the "Select Photon SDK" dropdown.
5. Then scroll down the page and press the "Create" button.
9. Once you have created the appID you will need to copy it into the config file. The AppID should fill in the Pun section. It is unnecessary to fill in the voice section that as that is handled automatically.
10. After you have done this you can save and close the file.
11. Restart the game and you can now host 5+ player games.

# Special Thanks
- [Anthony Stainton](https://github.com/ItzRock): Tech Support Help! Would not have been able to finish this if I had to help every person in the, in addition they made an excellent tutorial video.
- [Bobbie](https://github.com/legoandmars/Virality): Original idea for syncing AppID's over the Steam Lobby.
