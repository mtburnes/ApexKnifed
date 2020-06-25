# This document is created to help understand the datatypes that I'm using.

 Plugin development is really hindered by a lack of documentation for any of the used datatypes. This document is created to help create some documentation on the objects I run into using this. It's main purpose is to be used as a "quick reference guide" and "API documentation".

## Plugin Functions
 ### onPlayerKilled
 This function is used as some kind of interrupt that is ran whenever someone is killed. A kill on the server will trigger an event, which runs this function. OnPlayerKilled's sole parameter (AFAIK) is a `Kill`, which has info regarding a particular player kill event.

## Data Types
 ### Kill
 Data | Type | Info
 ---- | ---- | ----
 Killer | CPlayerInfo | Player info on the killer
 Victim | CPlayerInfo | Player info on the victim
 KillerLocation | Point3D | Killer's location as a 3D point
 VictimLocation | Point3D | Victim's location as a 3D point
 DamageType | string | What weapon/method was used to kill the victim. See: [this](https://github.com/AdKats/Procon-1/blob/master/src/PRoCon.Core/Players/Items/DamageTypes.cs)
 Headshot | boolean | Determines if the kill was a headshot
 IsSuicide | boolean | Determines if the victim killed himself (Is the Killer and Victim the same)
 DateTime | TimeOfDeath | Precise time of the kill event.
 Distance | Double | The distance between the killer and the victim.

 ### CPlayerInfo
 Data | Type | Info
 ---- | ---- | ----
 ClanTag | string | A particular player's Clan Tag.
 SoldierName | string | A player's in game name (**not unique** `GUID` is to be used for identification)
 GUID | string | The unique identifier used to identify an account
 TeamID | int | ?
 SquadID | int | ?
 Score | int | A player's score as shown in the leaderboards
 Kills | int | A player's kills as shown in the leaderboards
 Deaths | int | A player's deaths as shown in the leaderboards
 Ping | int | A player's ping. In milliseconds.
 Rank | int | A soldier's rank (*possibly erroniously not set*. common values between 0-140)
 Type | int | ?
 Kdr | float | A soldier's **K**ills to **D**eaths **R**atio
 JoinTime | int | The time at which the player joined the server
 SessionTime | int | The time a player has been connected. (`CurrentTime - JoinTime`)
 