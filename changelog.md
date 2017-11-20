### 2017-11-20
[Slushnas]
* Added multiple hex and enum conversions to multiple messages.
* Fixed support for UpdateString and UpdateFloat messages.
* Fixed some issues with the Trade class and AcceptTrade message.
* Added support for the CharacterError message and added some info to the enum for it.
* Fixed up LogOff and CharacterDelete messages.
* Changed data types in ACCharGenResult and added some enum conversions.
* Added support for the AccountBooted message.
* Added support for Fellowship AssignNewLeader message and added some fixes to the FellowshipFullUpdate message.
* Fixed alignment issue with PlayerTeleport and DeleteObject messages.
* Fixed some issues with the CharacterCreate message and added some supporting enums.
* Renamed LifestoneMaterialize message to PositionAndMovement.
* Added PositionPack class and converted UpdatePosition message to use the PositionPack class.
* Added MovementData and MovementDataUnpack classes and did some refactoring so that the MovementEvent and PositionAndMovement messages as well as the PhysicsDesc class parse movement data correctly.
* Renamed MovementAction class to ActionNode because it is named that way in the client and also corrected parsing of the data.
* Fixed JumpPack class parsing and by extension the Jump message.
* Added the display of packed bitfield items and hex conversions to PhysicsDesc and PublicWeenieDesc classes.
* Added the display of PhysicsState items to the SetState message.
* Fixed parsing of the ChatServerData message and added some supporting enums.
* Made the CM_Movement class public.
* Added support for the Join, Quit, Stalemate, Recv_JoinGameResponse, and Recv_GameOver chess messages and added some related enums.
* Reworked the StatMod class to use the EnchantmentTypeEnum and fixed an issue with the enum order.
* Removed the ContentProfile class from CM_Login and CM_Trade as they are duplicates of CM_Inventory.ContentProfile.
* Added ContainerProperties enum and added it to ContentProfile display.
* Fixed display of some messages that use the INVENTORY_LOC enum.
* Added some int properties to the STypeInt enum.
* Added bitfield items and hex conversion to many fields in the PlayerDescription class.
* Added Weenie class IDs enum (WCLASSID) with data from the client_portal.dat file.

### 2017-11-03
[Slushnas]
##### Interface Change
* Fixed bug when moving to next highlighted row where rows on the last page would not be selected.
* Added double buffering to treeview class by using a custom override. This should prevent most flicker in the treeview when updating.
I also had to set the build platform target to *any* instead of *x64* to avoid loading errors in the form designer. This is expected behavior that is explained here: https://support.microsoft.com/en-us/help/963017/cannot-add-controls-from-64-bit-assemblies-to-the-toolbox-or-use-in-de
* Refactored treeview node expansion state and TopNode code.
* Renamed the utility function Utility.FormatGuid to Utility.FormatHex and **_Display Guid as Hex_** to **_Display Data as Hex_** since it is used for more fields than just Guids. (Example: bitmasks)
* Added wait cursor when loading a pcap and when toggling **_Display Data as Hex_**.
* Fixed bug where the currently viewed line number was not being updated properly when moving between highlighted items.

##### Other Changes
* Fixed spellbook parsing in the AppraisalInfo message.
* Added support for the House_Recv_UpdateRestrictions, and House_Recv_UpdateHAR messages and added an enum.
* Added RadarColor, RDBBitmask, and CoverageMask enums.
* Added RestrictionDB field to PublicWeenieDescription.
* Fixed up multiple fields in PublicWeenieDescription.
* Added enum conversion to VendorProfile.
* Did some cleanup to the Login_CharacterSet message.
* Added changelog to the solution.

### 2017-10-29
[Slushnas]
* Added support for the Modify, Add, and Delete book response events and added hex conversions for book "flags" fields.
* Added handler for the AllegianceUpdateDone message.
* Fixed parsing of PlayerModule structure. This fixes parsing for messages like CharacterOptionsEvent and PlayerDescription.
* Fixed gender enum and added a UIElement enum to support gameplay options in the PlayerModule structure.
* Added ulong override for the Utility.FormatGuid function.
* Fixed some gleaned cooldown spell IDs based on ACE DB.

### 2017-10-22
[Slushnas]
##### Interface Changes
* Added **_Expand All_** and **_Collapse All_** options to the parsed data treeview right-click menu.
* When toggling **_Display Guid as Hex_** the interface will now restore your position in the treeview after updating instead of scrolling you to the top or bottom.

### 2017-10-20
[Slushnas]
##### Interface Change
* Expanded the treeview pane a little to reduce the chance of seeing horizontal scrollbars.

##### Other Changes

* Added support for the AppraiseDone message.
* Fixed support for SendClientContractTrackerTable (0x0314) and SendClientContractTracker (0x0315) messages.
* Added a custom enum for contract names.
* Added support for the AttackDoneEvent.
* Added some missing character title enum IDs.
* Fixed FriendsUpdate message.
* Fixed AddOrSetCharacterTitle and SetDisplayCharacterTitle messages.
* Other small formatting fixes.

### 2017-10-18
[Slushnas]
##### Interface Changes
* The default method for opening files is now the "as messages" mode. This mainly affects opening files from the "Find Opcode In Files" dialog.
* The "Use Highlighting" checkbox will be unchecked and disabled when viewing a file in "as messages" mode as it was not designed for use in this mode.
* Fixed a bug where files opened in "as messages" mode could get highlighting applied when it shouldn't be. As a result, larger messages
will now load faster in some cases.
* You can now hit the Enter key in the Opcode box to start a search in the "Find Opcode In Files" dialog.

##### Other Changes
* Renamed Contract cooldown spell enum as it looks like it is shared for all contracts.
* Added gender enum.
* Renamed lock related weenie errors as they apply to both chests and doors.
* Added enum and Guid conversions to lots of variables.
* Added initial support for the AllegianceUpdate event (0x0020) and AllegianceInfoResponseEvent (0x027C). There is a chance that character nodes will not be grouped correctly 
in the hierarchy but all data should be parsed.
* Fixed several housing messages: BuyHouse, Recv_HouseProfile, RentHouse, Recv_UpdateRentPayment, and Recv_AvailableHouses.

### 2017-10-14
[Slushnas]
* Fixed parsing of CreateTinkeringTool and SalvageOperationsResultData messages.
* Added support for InventoryServerSaysFailedEvent, ViewContentsEvent, and InventoryPutObjIn3DEvent messages.
* Added GUID conversion and enums to many message variables.
* Renamed UpdateStackSize variable from maxNumPages to ts to indicate it is a timestamp.

### 2017-10-13
[Slushnas]
* Added support for the following communication messages: SetAFKMode, SetAFKMessage, ModifyCharacterSquelch, ModifyAccountSquelch, ModifyGlobalSquelch, and SetSquelchDB.
* Added some enums for the newly supported squelch messages.

### 2017-10-09
[Slushnas]
* The treeview will now update when toggling "Display Guid as Hex."

### 2017-10-06
[Slushnas]
* Overhauled Magic message handling to support more messages.
* Renamed spell id field to be consistent throughout.
* Commented out some messages that don't appear to be used.

### 2017-10-04
[Slushnas]
* Added support for ChannelBroadcast messages.
* Added GroupChatType enums from ACE and added a few that were found in the client.
* Changed an SMainCat enum to a more descriptive name.
* Added a couple of enum display conversions for inventory and sounds.

### 2017-09-30
[Slushnas]
* Added support for Magic__UpdateEnchantment message (02C2).
* Added some gleaned spell enums to support the UpdateEnchantment message.
* Changed GetAndWieldItem message to display GUID format.

### 2017-09-24
[OptimShi]
* Corrected the reading of PList<HousePayment> and added proper structure to the TreeView

### 2017-09-22
[OptimShi]
* Implemented "Actions" in the Movement Event (F74C)

### 2017-09-16
[OptimShi]
* Added PlayerDescriptionEvent 0x0013
* Added ability to hit the "Enter" key in the Highlight Opcode text box to perform the highlight.

### 2017-09-15
* Changelog Created.